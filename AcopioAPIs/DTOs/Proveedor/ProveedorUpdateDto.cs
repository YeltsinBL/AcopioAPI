using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Proveedor
{
    public class ProveedorUpdateDto: UpdateDto
    {
        public int ProveedorId { get; set; }
        public required string ProveedorUT { get; set; }
        public required List<ProveedorPersonaUpdateDto> ProveedorPersons { get; set; }
    }
}
