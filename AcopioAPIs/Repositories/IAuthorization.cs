using AcopioAPIs.DTOs.Login;

namespace AcopioAPIs.Repositories
{
    public interface IAuthorization
    {
        Task<bool> VerifyRegisterPassword(AuthorizationRequest authorizationRequest);
        Task<AuthorizationResponse> RefreshTokenResponse(RefreshTokenRequest refreshTokenRequest);
        Task<AuthorizationResponse> TokenResponse(AuthorizationRequest authorizationRequest);
        Task<bool> RegisterNewPassword(RegisterPasswordRequest resetPasswordRequest);
    }
}
