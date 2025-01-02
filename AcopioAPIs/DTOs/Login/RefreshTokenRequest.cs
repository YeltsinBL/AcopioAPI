namespace AcopioAPIs.DTOs.Login
{
    public class RefreshTokenRequest
    {
        public required string TokenExpirado { get; set; }
        public required string RefreshToken { get; set; }
    }
}
