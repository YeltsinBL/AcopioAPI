namespace AcopioAPIs.DTOs.Proveedor
{
    public class ProveedorDTO
    {
        public int ProveedorId { get; set; }
        public required string ProveedorUT { get; set; }
        public bool ProveedorStatus { get; set; }
        public required List<ProveedorPersonDto> ProveedorPerson { get; set; }
    }
}
