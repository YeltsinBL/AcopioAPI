namespace AcopioAPIs.DTOs.Common
{
    public class UpdateDto
    {
        public DateTime UserModifiedAt { get; set; }
        public required string UserModifiedName { get; set; }
    }
}
