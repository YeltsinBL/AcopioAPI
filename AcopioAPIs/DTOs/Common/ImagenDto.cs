namespace AcopioAPIs.DTOs.Common
{
    public class ImagenDto
    {
        public int ImagenId { get; set; }
        public required string ImagenUrl { get; set; }
        public string? ImagenComentario { get; set; }
    }
}
