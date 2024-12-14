namespace AcopioAPIs.DTOs.Common
{
    public class UserInsertDto
    {
        public DateTime UserCreatedAt { get; set; }
        public required string UserCreatedName { get; set; }
    }
}
