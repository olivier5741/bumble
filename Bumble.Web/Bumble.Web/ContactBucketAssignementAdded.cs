using System;
using System.Collections.Generic;

namespace Bumble.Web
{
    public class ContactBucketAssignementAdded
    {
        public DateTime Time { get; set; }
        public Guid? TenantId { get; set; }
        public string TenantKey { get; set; }
        public ContactBucket Bucket { get; set; }
        public Contact Contact { get; set; }
    }
}