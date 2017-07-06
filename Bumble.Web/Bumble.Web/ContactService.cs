using System;
using ServiceStack;
using ServiceStack.OrmLite;

namespace Bumble.Web
{
    public class ContactService : Service
    {
        public Contact Post(Contact request)
        {
            request.Id = Guid.NewGuid();
            Db.Save(request);
            return request;
        }
        
        public Contact Get(Contact request)
        {
            return Db.SingleById<Contact>(request.Id);
        }
        
        public Contact Put(Contact request)
        {
            Db.Save(request);
            return Db.SingleById<Contact>(request.Id);
        }
        
        public Contact Patch(Contact request)
        {
            Db.UpdateNonDefaults(request, c => c.Id == request.Id);
            return Db.SingleById<Contact>(request.Id);
        }
        
        public object Delete(Contact request)
        {
            Db.DeleteById<Contact>(request.Id);
            return null;
        }
    }
}