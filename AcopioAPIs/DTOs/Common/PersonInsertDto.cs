namespace AcopioAPIs.DTOs.Common
{
    public class PersonInsertDto
    {
        public string? PersonDNI { get; set; }
        public required string PersonName { get; set; }
        public required string PersonPaternalSurname { get; set; }
        public required string PersonMaternalSurname { get; set; }
    }
}
