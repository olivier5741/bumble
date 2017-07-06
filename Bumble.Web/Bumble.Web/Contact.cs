using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace Bumble.Web
{
    [Route("/cms/contact", "POST,GET,PUT,PATCH,DELETE")]
    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        [Reference]
        public List<ContactTag> Tags { get; set; }

        public object Data { get; set; }

        [IgnoreDataMember]
        public DateTime UpdatedTime { get; set; }

        [IgnoreDataMember]
        public bool IsDeleted { get; set; }

        public ulong RowVersion { get; set; }
    }
}