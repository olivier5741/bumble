using System;

namespace Bumble.Web
{
    public interface IBelonger
    {
        Guid? TenantId { get; set; }
        string TenantKey { get; set; }
    }
}