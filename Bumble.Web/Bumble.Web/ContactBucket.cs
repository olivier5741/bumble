using System;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace Bumble.Web
{
    [Route("/{TenantKey}/cms/bucket", "POST,GET,PUT,DELETE")]
    [Route("/cms/bucket", "POST,GET,PUT,DELETE")]
    public class ContactBucket : IBelonger
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; } // TODO enforce not empty in database
        
        [Ignore]
        public string TenantKey { get; set; }
        
        public string Name { get; set; }
    }
}