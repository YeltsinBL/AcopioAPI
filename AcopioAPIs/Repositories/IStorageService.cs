namespace AcopioAPIs.Repositories
{
    public interface IStorageService
    {
        Task<string> UploadImageAsync(string nombreCarpeta, Stream fileStream, string fileName);
    }
}
