namespace AcopioAPIs.DTOs.Login
{
    public class AuthorizationRequest
    {
        public required string UserName { get; set; }
        public required string UserPassword { get; set; }
    }
}
