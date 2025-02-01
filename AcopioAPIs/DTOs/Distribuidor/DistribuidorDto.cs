namespace AcopioAPIs.DTOs.Distribuidor
{
    public class DistribuidorDto
    {
        public int DistribuidorId { get; set; }
        public required string DistribuidorRuc { get; set; }
        public required string DistribuidorNombre { get; set; }
        public bool DistribuidorStatus { get; set; }
    }
}
