namespace AcopioAPIs.DTOs.Cliente
{
    public class ClienteDto
    {
        public int ClienteId { get; set; }
        public string? ClienteDni { get; set; }
        public required string ClienteNombre { get; set; }
        public string? ClienteApellidoPaterno { get; set; }
        public string? ClienteApellidoMaterno { get; set; }
        public bool ClienteStatus { get; set; }
    }
}
