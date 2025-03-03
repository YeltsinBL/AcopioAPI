using AcopioAPIs.Models;
using Microsoft.IdentityModel.Tokens;

namespace AcopioAPIs.Repositories
{
    public class ImagenRepository : IImagen
    {
        private readonly DbacopioContext _context;
        private readonly IStorageService _storageService;

        public ImagenRepository(DbacopioContext context, IStorageService storageService)
        {
            _context = context;
            _storageService = storageService;
        }

        public async Task SaveImagen(
            int referenciaId, string tipoReferencia, DateTime fecha, string usuario,
            List<IFormFile> imagenes, List<string> descripciones)
        {
            for (int i = 0; i < imagenes.Count; i++)
            {
                var imagen = imagenes[i];
                var descripcion = descripciones.ElementAtOrDefault(i) ?? "Sin descripción";

                if (imagen.Length > 0)
                {
                    var uploadResult = await _storageService.UploadImageAsync(tipoReferencia, imagen);
                    if (uploadResult.IsNullOrEmpty()) throw new Exception("Error al subir imagen a Cloudinary");
                    _context.Add(new Imagen
                    {
                        ReferenciaId = referenciaId,
                        TipoReferencia = tipoReferencia,
                        ImagenUrl = uploadResult,
                        ImagenComentario = descripcion,
                        ImagenStatus = true,
                        UserCreatedAt = fecha,
                        UserCreatedName = usuario,
                    });

                    await _context.SaveChangesAsync();
                }
            }
            return;
        }
    }
}
