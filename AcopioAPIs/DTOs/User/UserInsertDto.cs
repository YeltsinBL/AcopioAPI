using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.User
{
    public class UserInsertDto : PersonInsertDto
    {
        public int TypePersonId { get; set; }
        public required string UserName { get; set; }
        public required string UserPassword { get; set; }
        public DateTime UserCreatedAt { get; set; }
        public required string UserCreatedName { get; set; }
    }
}
