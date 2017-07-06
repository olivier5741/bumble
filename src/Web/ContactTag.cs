using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace Bumble.Web
{
    public class ContactTag
    {
        [IgnoreDataMember]
        public long Id { get; set; }
        
        [IgnoreDataMember]
        [ForeignKey(typeof(Contact), OnDelete = "CASCADE", OnUpdate = "CASCADE")]
        public Guid ContactId { get; set; }
        
        public string Value { get; set; }
    }
}