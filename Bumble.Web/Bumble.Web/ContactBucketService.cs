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
            return Db.SingleById<ContactBucket>(request.Id);
        }
        
        public ContactBucket Put(ContactBucket request)
        {
            Db.Save(request);
            return Db.SingleById<ContactBucket>(request.Id);
        }
        
        public object Delete(ContactBucket request)
        {
            Db.DeleteById<ContactBucket>(request.Id);
            return null;
        }
    }
}