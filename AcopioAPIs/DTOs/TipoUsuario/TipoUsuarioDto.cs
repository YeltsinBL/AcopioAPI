namespace AcopioAPIs.DTOs.TipoUsuario
{
    public class TipoUsuarioDto
    {
        public int TipoUsuarioId { get; set; }
        public required string TipoUsuarioNombre { get; set; }
        public bool TipoUsuarioEstado { get; set; }

    }
}
