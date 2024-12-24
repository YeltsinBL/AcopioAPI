namespace AcopioAPIs.DTOs.Proveedor
{
    public class ProveedorResultDto
    {
        public int ProveedorId { get; set; }
        public required string ProveedorUT { get; set; }
        public string? PersonDNI { get; set; }
        public required string ProveedorNombre { get; set; }
        public bool ProveedorStatus { get; set; }
    }

    public class ProveedorGroupedDto
    {
        public int ProveedorId { get; set; }
        public required string ProveedorUT { get; set; }
        public bool ProveedorStatus { get; set; }
        public required List<PersonaDto> Personas { get; set; }
    }

    public class PersonaDto
    {
        public required string PersonDNI { get; set; }
        public required string ProveedorNombre { get; set; }
        public bool ProveedorPersonStatus { get; set; }
    }

}
