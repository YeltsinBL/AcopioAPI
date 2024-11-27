namespace AcopioAPIs.DTOs.Proveedor
{
    public class ProveedorInsertDto
    {
        public string PersonDNI { get; set; }
        public string PersonName { get; set; }
        public string PersonPaternalSurname { get; set; }
        public string PersonMaternalSurname { get; set; }
        public int Person_Type { get; set; }
        public string ProveedorUT { get; set; }
        public bool ProveedorStatus { get; set; }
        public string UserCreatedName { get; set; }
        public DateTime UserCreatedAt { get; set; }
    }

}
