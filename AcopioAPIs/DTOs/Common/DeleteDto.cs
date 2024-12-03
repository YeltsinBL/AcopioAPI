namespace AcopioAPIs.DTOs.Common
{
    public class DeleteDto
    {
        public int Id { get; set; }

        public required string UserModifiedName { get; set; }

        public DateTime UserModifiedAt { get; set; }
    }
}
