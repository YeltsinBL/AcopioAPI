using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Proveedor
{
    public class ProveedorPersonDto:PersonDto
    {
        public int ProveedorPersonId { get; set; }
        public int ProveedorId { get; set; }
        public bool ProveedorPersonStatus { get; set; }
    }
}
