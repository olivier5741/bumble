using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Bumble.Web
{
    public class ContactBucketAssignementService : Service
    {
        public ContactBucketAssignement[] Post(ContactBucketAssignement[] request)
        {
            request.ToList().ForEach(r => r.Id = Guid.NewGuid());
            Db.SaveAll(request);
            return request;
        }
        
        public ContactBucketAssignement Post(ContactBucketAssignement request)
        {
            request.Id = Guid.NewGuid();
            Db.Save(request);
            return request;
        }
        
        public ContactBucketAssignement Get(ContactBucketAssignement request)
        {
            return Db.SingleById<ContactBucketAssignement>(request.Id);
        }
        
        public object Delete(ContactBucketAssignement[] request)
        {
            Db.DeleteByIds<Contact>(request.Select(r => r.Id));
            return null;
        }
        
        public object Delete(ContactBucketAssignement request)
        {
            Db.DeleteById<Contact>(request.Id);
            return null;
        }
    }
}