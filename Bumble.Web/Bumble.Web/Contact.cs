using System;
using System.Runtime.Serialization;
using ServiceStack;

namespace Bumble.Web
{
    [Route("/contact", "POST,GET,PUT,PATCH,DELETE")]
    public class Contact
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public object Data { get; set; }
        
        [IgnoreDataMember]
        public DateTime UpdatedTime { get; set; }
        
        [IgnoreDataMember]
        public bool IsDeleted { get; set; }
        
        public ulong RowVersion { get; set; }
    }
}