using System;
using System.Runtime.Serialization;
using ServiceStack;

namespace Bumble.Web
{
    [DataContract]
    public class BumbleSession : AuthUserSession
    {
        [DataMember]
        public Guid TenantId { get; set;}
    }
}