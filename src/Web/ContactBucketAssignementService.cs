using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Bumble.Web
{
    public class ContactBucketAssignementService : Service
    {
        private readonly ContactBucketService _bucketService;
        private readonly IMessagePublisher _bus;

        public ContactBucketAssignementService(ContactBucketService bucketService, IMessagePublisher bus)
        {
            _bucketService = bucketService;
            _bus = bus;
        }
        
        public ContactBucketAssignement Post(ContactBucketAssignement request)
        {
            request.Id = Guid.NewGuid();

            var bucket = _bucketService.Get(new ContactBucket {Id = request.Id, Key = request.ContactBucketKey}).CheckIfBelongs(request);
            
            if(bucket == null)
                throw new ArgumentException($"Bucket does not exist");

            request.ContactBucketId = bucket.Id;
            
            Db.Save(request);
            
            var @event = 
                new ContactBucketAssignementAdded().PopulateWith(
                    Db.LoadSingleById<ContactBucketAssignement>(request.Id));
            _bus.Publish(@event,$"tenant_id.{@event.TenantId}.bucket_id.{@event.Bucket.Id}");

            return request;
        }

        public ContactBucketAssignement Get(ContactBucketAssignement request)
        {
            return Db.Single<ContactBucketAssignement>(c => c.Id == request.Id).CheckIfBelongs(request);
        }

        public object Delete(ContactBucketAssignement request)
        {
            var assignement = Db.SingleById<ContactBucketAssignement>(request.Id).CheckIfBelongs(request);
            Db.DeleteById<Contact>(request.Id);
            
            var @event = new ContactBucketAssignementRemoved().PopulateWith(assignement);
            _bus.Publish(@event,$"tenant_id.{@event.TenantId}.bucket_id.{@event.Bucket.Id}");
            
            return null;
        }
    }
}