using System;
using System.Collections.Generic;
using System.Linq;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Bumble.Web
{
    public class ContactBucketAssignementService : Service
    {
        
        public ContactBucketAssignement Post(ContactBucketAssignement request)
        {
            request.Id = Guid.NewGuid();
            Db.Save(request);
            return request;
        }
        
        public ContactBucketAssignement Get(ContactBucketAssignement request)
        {
            return Db.SingleById<ContactBucketAssignement>(request.Id).CheckIfBelongs(request);
        }
        
        public object Delete(ContactBucketAssignement request)
        {
            Db.SingleById<ContactBucketAssignement>(request.Id).CheckIfBelongs(request);
            Db.DeleteById<Contact>(request.Id);
            return null;
        }
    }
}