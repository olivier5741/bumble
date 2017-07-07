using System;
using System.Runtime.Serialization;
using ServiceStack.DataAnnotations;

namespace Bumble.Web
{
    public class ContactTag
    {
        [AutoIncrement]
        public long Id { get; set; }
        
        [ForeignKey(typeof(Contact), OnDelete = "CASCADE", OnUpdate = "CASCADE")]
        public Guid ContactId { get; set; }
        
        public string Value { get; set; }
    }
}