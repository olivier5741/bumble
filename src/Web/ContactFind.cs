using System;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace Bumble.Web
{
    // Bug load select not working for sqlite 
    
    [Route("/{TenantKey}/cms/contact/find","GET")]
    [Route("/cms/contact/find","GET")]
    public class ContactFind : QueryDb<Contact>, ILeftJoin<Contact,ContactTag>
    {
        public Guid TenantId { get; set; }
        
        [Ignore]
        public string TenantKey { get; set; }
        
        public string NameContains { get; set; }
        public string ContactTagValue { get; set; } // TODO should be renamed to tag
    }
}