using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Login
{
    public class RegisterPasswordRequest:UpdateDto
    {
        public required string UserName { get; set; }
        public required string UserPassword { get; set; }
    }
}
