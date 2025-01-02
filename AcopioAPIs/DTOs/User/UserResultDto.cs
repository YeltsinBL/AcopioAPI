using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.User
{
    public class UserResultDto: PersonResult
    {
        public int UserId { get; set; }
        public required string TypePersonName { get; set; }
        public required string UserName { get; set; }
        public bool UserStatus { get; set; }
        public bool UserResetPassword { get; set; }
    }
}
