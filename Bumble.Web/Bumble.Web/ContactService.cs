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
            Db.Save(request,true);
            return Db.LoadSingleById<Contact>(request.Id);
        }
        
        public Contact Get(Contact request)
        {
            return Db.LoadSingleById<Contact>(request.Id);
        }
        
        public Contact Put(Contact request)
        {
            Db.Delete<ContactTag>(t => t.ContactId == request.Id);
            Db.Save(request,true);
            return Db.LoadSingleById<Contact>(request.Id);
        }
        
        public Contact Patch(Contact request)
        {
            // TODO reentrant but not transactional
            
            if (request.Tags != null)
            {
                Db.Delete<ContactTag>(t => t.ContactId == request.Id);
                request.Tags?.ForEach(t => t.ContactId = request.Id);
                Db.SaveAll(request.Tags);
            }
            
            Db.UpdateNonDefaults(request, c => c.Id == request.Id);
            return Db.LoadSingleById<Contact>(request.Id);
        }
        
        public object Delete(Contact request)
        {
            Db.DeleteById<Contact>(request.Id);
            return null;
        }
    }
}