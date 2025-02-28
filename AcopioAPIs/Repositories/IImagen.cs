using AcopioAPIs.Models;

namespace AcopioAPIs.Repositories
{
    public interface IImagen
    {
        Task SaveImagen(
            int referenciaId, string tipoReferencia, DateTime fecha, string usuario,
            List<IFormFile> imagenes, List<string> descripciones);
    }
}
