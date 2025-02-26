using AcopioAPIs.DTOs.Common;

namespace AcopioAPIs.Utils
{
    public static class ResponseHelper
    {
        public static ResultDto<T> ReturnData<T>(T? data, string message, bool result=true)
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
