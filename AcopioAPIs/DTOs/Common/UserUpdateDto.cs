namespace AcopioAPIs.DTOs.Common
{
    public class UserUpdateDto
    {
        public DateTime UserModifiedAt { get; set; }
        public required string UserModifiedName { get; set; }
    }
}
