using System;

namespace Bumble.Web
{
    public interface IBelonger
    {
        Guid? TenantId { get; set; }
    }
}