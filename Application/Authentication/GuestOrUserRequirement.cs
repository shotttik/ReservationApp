using Microsoft.AspNetCore.Authorization;

namespace Application.Authentication
{
    public sealed class GuestOrUserRequirement :IAuthorizationRequirement
    {

        public GuestOrUserRequirement()
        {
        }
    }
}
