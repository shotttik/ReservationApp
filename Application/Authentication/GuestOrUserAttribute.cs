using Microsoft.AspNetCore.Authorization;

namespace Application.Authentication
{
    public sealed class GuestOrUserAttribute :AuthorizeAttribute
    {
        public const string GuestOrUserPolicyPrefix = "GuestOrUser";
        public GuestOrUserAttribute() : base(GuestOrUserPolicyPrefix)
        {
        }
    }

}
