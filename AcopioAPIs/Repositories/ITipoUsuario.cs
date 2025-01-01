using AcopioAPIs.DTOs.TipoUsuario;

namespace AcopioAPIs.Repositories
{
    public interface ITipoUsuario
    {
        Task<List<TipoUsuarioDto>> ListTipos(string? nombre, bool? estado);
        Task<TipoUsuarioDto> GetTipo(int id);
        Task<TipoUsuarioDto> InsertTipo(TipoUsuarioInsertDto tipoUsuario);
        Task<TipoUsuarioDto> UpdateTipo(TipoUsuarioUpdateDto tipoUsuario);
        Task<bool> DeleteTipo(TipoUsuarioDeleteDto tipoUsuario);
    }
}
