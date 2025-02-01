using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Distribuidor
{
    public class DistribuidorInsertDto: InsertDto
    {
        public required string DistribuidorRuc { get; set; }
        public required string DistribuidorNombre { get; set; }
    }
}
