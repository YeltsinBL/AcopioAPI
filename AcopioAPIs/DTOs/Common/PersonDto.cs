namespace AcopioAPIs.DTOs.Common
{
    public class PersonDto
    {
        public int PersonId { get; set; }
        public string? PersonDNI { get; set; }
        public required string PersonName { get; set; }
        public required string PersonPaternalSurname { get; set; }
        public required string PersonMaternalSurname { get; set; }
        public bool PersonStatus { get; set; }
    }
}
