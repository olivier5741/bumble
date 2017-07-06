using System;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace Bumble.Web
{
    [Route("/cms/bucket/assignement","POST,GET,DELETE")]
    public class ContactBucketAssignement
    {
        public Guid Id { get; set; }
        
        [ForeignKey(typeof(Contact), OnDelete = "CASCADE", OnUpdate = "CASCADE")]
        public Guid ContactId { get; set; }
        
        [ForeignKey(typeof(ContactBucket), OnDelete = "CASCADE", OnUpdate = "CASCADE")]
        public Guid ContactBucketId { get; set; }
    }
}