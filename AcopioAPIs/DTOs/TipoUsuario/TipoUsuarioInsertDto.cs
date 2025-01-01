using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.DTOs.TipoUsuario
{
    public class TipoUsuarioInsertDto:InsertDto
    {
        public required string TipoUsuarioNombre { get; set; }
    }
}
