using System;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Bumble.Web
{
    public class ContactBucketService : Service
    {
        public ContactBucket Post(ContactBucket request)
        {
            request.Id = Guid.NewGuid();
            Db.Save(request);
            return request;
        }
        
        public ContactBucket Get(ContactBucket request)
        {
            return Db.SingleById<ContactBucket>(request.Id).CheckIfBelongs(request);
        }
        
        public ContactBucket Put(ContactBucket request)
        {
            Db.SingleById<ContactBucket>(request.Id).CheckIfBelongs(request);
            Db.Save(request);
            return Db.SingleById<ContactBucket>(request.Id);
        }
        
        public object Delete(ContactBucket request)
        {
            Db.SingleById<ContactBucket>(request.Id).CheckIfBelongs(request);
            Db.DeleteById<ContactBucket>(request.Id);
            return null;
        }
    }
}