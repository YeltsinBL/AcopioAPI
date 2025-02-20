using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.Cliente
{
    public class ClienteUpdateDto: UpdateDto
    {
        public int ClienteId { get; set; }
        public required string ClienteDni{ get; set; }
        public required string ClienteNombre { get; set; }
        public required string ClienteApellidoPaterno { get; set; }
        public required string ClienteApellidoMaterno { get; set; }
    }
}
