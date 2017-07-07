using System;
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
            return Db.LoadSingleById<Contact>(request.Id).CheckIfBelongs(request);
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
            
           Db.LoadSingleById<Contact>(request.Id).CheckIfBelongs(request);
            
           Db.UpdateNonDefaults(request, c => c.Id == request.Id);// TODO : might be better contact.PopulateWithNonDefaultValues(viewModel);

            if (request.Tags != null)
                ResetContactTags(request);

            var contact = Db.LoadSingleById<Contact>(request.Id);
            
            var @event = new ContactUpdated().PopulateWith(contact);
            _bus.Publish(@event,$"tenant_id.{@event.TenantId}");

            return contact;
        }

        private void ResetContactTags(Contact request)
        {
            Db.Delete<ContactTag>(t => t.ContactId == request.Id);
            request.Tags?.ForEach(t => t.ContactId = request.Id);
            Db.SaveAll(request.Tags);
        }

        public object Delete(Contact request)
        {
            var contact = Db.LoadSingleById<Contact>(request.Id).CheckIfBelongs(request);
            contact.IsDeleted = true;
            Db.Update(contact); // TODO this might be to simple
            var @event = new ContactDeleted().PopulateWith(contact);
            _bus.Publish(@event,$"tenant_id.{@event.TenantId}");
            return null;
        }
    }
}