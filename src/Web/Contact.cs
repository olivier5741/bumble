using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using ServiceStack;
using ServiceStack.DataAnnotations;

namespace Bumble.Web
{
    
    [Route("/cms/contact", "POST,GET,PUT,PATCH,DELETE")]
    [Route("/cms/contact/{Id}", "GET,DELETE")]
    [Route("/cms/contact/{UniqueIdentifier}", "GET,DELETE")]
    [Route("/{TenantKey}/cms/contact", "POST,GET,PUT,PATCH,DELETE")]
    [Route("/{TenantKey}/cms/contact/{Id}", "GET,DELETE")]
    [Route("/{TenantKey}/cms/contact/{UniqueIdentifier}", "GET,DELETE")]
    // Bug Unable to bind to request 'Contact' : '32476283271' is an Invalid value for 'Id'
    public class Contact : IBelonger, ISoftDelete
    {
        [CheckConstraint(nameof(TenantId) + " IS NOT NULL")]
        public Guid? Id { get; set; }
        
        [CheckConstraint(nameof(TenantId) + " IS NOT NULL")]
        public Guid? TenantId { get; set; } // TODO enforce not empty in database
        
        [Ignore]
        public string TenantKey { get; set; }
        
        [Ignore]
        public string UniqueIdentifier { get; set; }
        
        public string Name { get; set; }
        
        public string PhoneNumber { get; set; }
        
        public string MobilePhoneNumber { get; set; }
        
        public string Email { get; set; }

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