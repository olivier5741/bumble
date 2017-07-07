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

            if (request.Key == null)
                request.Key = request.Name.ToLower().ToLowercaseUnderscore().Replace(" ", "").Replace("_","-");
            
            Db.Save(request);
            return request;
        }
        
        public ContactBucket Get(ContactBucket request)
        {
            return Db.Single<ContactBucket>(c => c.Id == request.Id || c.Key == request.Key).CheckIfBelongs(request);
        }
        
        public ContactBucket Put(ContactBucket request)
        {
            var bucket = Db.Single<ContactBucket>(c => c.Id == request.Id || c.Key == request.Key).CheckIfBelongs(request);
            
            request.Id = bucket.Id;
            request.Key = bucket.Key;
            
            Db.Save(bucket.PopulateWith(request));
            return Db.SingleById<ContactBucket>(request.Id);
        }
        
        public object Delete(ContactBucket request)
        {
            var bucket = Db.Single<ContactBucket>(c => c.Id == request.Id || c.Key == request.Key).CheckIfBelongs(request);
            
            Db.DeleteById<ContactBucket>(bucket.Id);
            
            return null;
        }
    }
}