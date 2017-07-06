using System;
using ServiceStack;

namespace Bumble.Web
{
    [Route("/cms/bucket", "POST,GET,PUT,DELETE")]
    public class ContactBucket : IBelonger
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; } // TODO enforce not empty in database
        public string Name { get; set; }
    }
}