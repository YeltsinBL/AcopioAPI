namespace AcopioAPIs.Repositories
{
    public interface IStorageService
    {
        Task<string> UploadImageAsync(string nombreCarpeta, IFormFile imagen);
    }
}
