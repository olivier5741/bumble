using System;
using System.Collections.Generic;

namespace Bumble.Web
{
    public class ContactDeleted
    {
        public DateTime Time { get; set; } = DateTime.Now;
        public Guid Id { get; set; }
        public Guid? TenantId { get; set; }
        public string TenantKey { get; set; }
        public string Name { get; set; }
        public List<ContactTag> Tags { get; set; }
        public object Data { get; set; }
    }
}