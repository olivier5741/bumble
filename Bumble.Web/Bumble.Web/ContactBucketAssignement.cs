using System;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace Bumble.Web
{
    [Route("/{TenantKey}/cms/bucket/assignement","POST,GET,DELETE")]
    [Route("/cms/bucket/assignement","POST,GET,DELETE")]
    public class ContactBucketAssignement : IBelonger
    {
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; } // TODO enforce not empty in database
        
        [Ignore]
        public string TenantKey { get; set; }

        [ForeignKey(typeof(Contact), OnDelete = "CASCADE", OnUpdate = "CASCADE")]
        public Guid ContactId { get; set; }
        
        [ForeignKey(typeof(ContactBucket), OnDelete = "CASCADE", OnUpdate = "CASCADE")]
        public Guid ContactBucketId { get; set; }
    }
}