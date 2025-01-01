using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.TipoUsuario
{
    public class TipoUsuarioUpdateDto: UpdateDto
    {
        public int TipoUsuarioId { get; set; }
        public required string TipoUsuarioNombre { get; set; }
    }
}
