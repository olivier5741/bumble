using System;

namespace Bumble.Web
{
    public static class BumbleAuthorizationExts
    {
        public static TAgg CheckIfBelongs<TAgg, TRequest>(this TAgg agg, TRequest request)
            where TRequest : IBelonger where TAgg : IBelonger
        {
            if (request.TenantId != agg.TenantId)
                throw new UnauthorizedAccessException();
            return agg;
        }
    }
}