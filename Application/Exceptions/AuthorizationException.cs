namespace Application.Exceptions
{
    internal class AuthorizationException :Exception
    {
        public AuthorizationException(string message) : base(message) { }
    }
}
