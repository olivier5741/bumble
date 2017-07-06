using System;
using System.Runtime.Serialization;
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
        
        [Reference]
        [IgnoreDataMember]
        public Contact Contact { get; set; }
        
        [ForeignKey(typeof(ContactBucket), OnDelete = "CASCADE", OnUpdate = "CASCADE")]
        public Guid ContactBucketId { get; set; }
        
        [Reference]
        [IgnoreDataMember]
        public ContactBucket ContactBucket { get; set; }
    }
}