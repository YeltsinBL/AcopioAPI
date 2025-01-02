using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.User
{
    public class UserUpdateDto: PersonDto
    {
        public int UserId { get; set; }
        public int TypePersonId { get; set; }
        public DateTime UserModifiedAt { get; set; }
        public required string UserModifiedName { get; set; }
        public List<UserModuleDto>? UserModules { get; set; }
    }
}
