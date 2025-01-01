using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.User
{
    public class UserDeleteDto:UpdateDto
    {
        public int UserId { get; set; }
    }
}
