namespace AcopioAPIs.DTOs.Proveedor
{
    public class ProveedorResultDto
    {
        public int PersonId { get; set; }
        public string PersonDNI { get; set; }
        public string PersonName { get; set; }
        public string PersonPaternalSurname { get; set; }
        public string PersonMaternalSurname { get; set; }
        public int ProveedorId { get; set; }
        public string ProveedorUT { get; set; }
        public int ProveedorStatus { get; set; }
        public DateTime UserCreatedAt { get; set; }
    }

}
