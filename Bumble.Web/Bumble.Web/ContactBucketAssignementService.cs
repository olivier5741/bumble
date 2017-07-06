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

            PublishMessage(
                new ContactBucketAssignementAdded().PopulateWith(
                    Db.LoadSingleById<ContactBucketAssignement>(request.Id)));

            return request;
        }

        public ContactBucketAssignement Get(ContactBucketAssignement request)
        {
            return Db.SingleById<ContactBucketAssignement>(request.Id).CheckIfBelongs(request);
        }

        public object Delete(ContactBucketAssignement request)
        {
            var assignement = Db.SingleById<ContactBucketAssignement>(request.Id).CheckIfBelongs(request);
            Db.DeleteById<Contact>(request.Id);
            
            PublishMessage(
                new ContactBucketAssignementRemoved().PopulateWith(assignement));
            
            return null;
        }
    }
}