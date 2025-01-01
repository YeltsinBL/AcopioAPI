using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Proveedor
{
    public class ProveedorInsertDto: InsertDto
    {
        public required string ProveedorUT { get; set; }
        public required List<PersonInsertDto> ProveedorPersons { get; set; }
    }

}
