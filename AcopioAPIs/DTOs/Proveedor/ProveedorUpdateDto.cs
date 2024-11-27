namespace AcopioAPIs.DTOs.Proveedor
{
    public class ProveedorUpdateDto
    {
        public int ProveedorId { get; set; }
        public string PersonDNI { get; set; }
        public string PersonName { get; set; }
        public string PersonPaternalSurname { get; set; }
        public string PersonMaternalSurname { get; set; }
        public string ProveedorUT { get; set; }
        public bool ProveedorStatus { get; set; }
        public string UserModifiedName { get; set; }
        public DateTime UserModifiedAt { get; set; }
    }
}
