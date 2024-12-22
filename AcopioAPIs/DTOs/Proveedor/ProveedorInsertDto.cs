using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Proveedor
{
    public class ProveedorInsertDto: UserInsertDto
    {
        public required string ProveedorUT { get; set; }
        public required List<PersonInsertDto> ProveedorPerson { get; set; }
    }

}
