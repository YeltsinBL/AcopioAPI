using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.Utils
{
    public static class ResponseHelper
    {
        public static ResultDto<T> ReturnData<T>(T? data, bool result, string message)
        {
            return new ResultDto<T>
            {
                Result = result,
                ErrorMessage = message,
                Data = data
            };
        }
    }
}
