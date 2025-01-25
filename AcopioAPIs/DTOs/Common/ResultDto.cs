namespace AcopioAPIs.DTOs.Common
{
    public class ResultDto<T>
    {
        public bool Result { get; set; }
        public string? ErrorMessage { get; set; }
        public T? Data { get; set; }
    }
}
