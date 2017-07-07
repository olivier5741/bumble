using System;
using System.Linq;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Bumble.Web
{
    public class ContactService : Service
    {
        private readonly IMessagePublisher _bus;

        public ContactService(IMessagePublisher bus)
        {
            _bus = bus;
        }
        
        public Contact Post(Contact request)
        {
            request.Id = Guid.NewGuid();
            Db.Save(request, true);

            var @event = new ContactCreated().PopulateWith(request);
            _bus.Publish(@event,$"tenant_id.{@event.TenantId}");
            
            return Db.LoadSingleById<Contact>(request.Id);
        }

        public Contact Get(Contact request)
        {
            var query = Db.From<Contact>();

            if (request.Id.HasValue)
                query.Or(c => c.Id == request.Id);
            else
            {
                if (request.UniqueIdentifier != null)
                {
                    query.Or(c => c.PhoneNumber == request.UniqueIdentifier && c.TenantId == request.TenantId);
                    query.Or(c => c.MobilePhoneNumber == request.UniqueIdentifier && c.TenantId == request.TenantId);
                    query.Or(c => c.Email == request.UniqueIdentifier && c.TenantId == request.TenantId);
                }
                
                if (request.PhoneNumber != null)
                    query.Or(c => c.PhoneNumber == request.PhoneNumber && c.TenantId == request.TenantId);
            
                if (request.MobilePhoneNumber != null)
                    query.Or(c => c.MobilePhoneNumber == request.MobilePhoneNumber  && c.TenantId == request.TenantId);
                
                if (request.Email != null)
                    query.Or(c => c.Email == request.Email  && c.TenantId == request.TenantId);
            }
            
            var contact = Db.Single(query).CheckIfBelongs(request); // Load select not working on sqlite
            Db.LoadReferences(contact);
            return contact;
        }

        public Contact Put(Contact request)
        {
            // TODO reentrant but not transactional

            Db.LoadSingleById<Contact>(request.Id).CheckIfBelongs(request);
            
            Db.Update(request);
            ResetContactTags(request);
            
            var @event = new ContactUpdated().PopulateWith(request);
            _bus.Publish(@event,$"tenant_id.{@event.TenantId}");
            
            return Db.LoadSingleById<Contact>(request.Id);
        }

        public Contact Patch(Contact request)
        {
            // TODO reentrant but not transactional
            
            var contact = Get(request).CheckIfBelongs(request);
            
            if(contact == null)
                throw new ArgumentNullException();

            contact.PopulateWithNonDefaultValues(request);
            
            Db.Update(contact);

            if (request.Tags != null)
                ResetContactTags(request);
            
            var @event = new ContactUpdated().PopulateWith(contact);
            _bus.Publish(@event,$"tenant_id.{@event.TenantId}");

            return contact;
        }

        private void ResetContactTags(Contact request)
        {
            Db.Delete<ContactTag>(t => t.ContactId == request.Id);
            request.Tags?.ForEach(t => t.ContactId = request.Id.Value);
            Db.SaveAll(request.Tags);
        }

        public object Delete(Contact request)
        {
            var contact = Get(request).CheckIfBelongs(request);
            
            if(contact == null)
                throw new ArgumentNullException();
            
            contact.IsDeleted = true;
            Db.Update(contact); // TODO this might be to simple
            var @event = new ContactDeleted().PopulateWith(contact);
            _bus.Publish(@event,$"tenant_id.{@event.TenantId}");
            return null;
        }
    }
}