using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.User
{
    public class UserDto:PersonDto
    {
        public int UserId { get; set; }
        public required string UserName { get; set; }
        public bool UserStatus { get; set; }
        public int TypePersonId { get; set; }
        public required string TypePersonName { get; set; }
        public required List<UserResultModuleDto> UserModules { get; set; }
    }
}
