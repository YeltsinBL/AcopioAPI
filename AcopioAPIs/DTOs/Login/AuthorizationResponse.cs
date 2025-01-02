namespace AcopioAPIs.DTOs.Login
{
    public class AuthorizationResponse
    {
        public required string Token { get; set; }
        public required string RefreshToken { get; set; }
        public bool Resultado { get; set; }
    }
}
