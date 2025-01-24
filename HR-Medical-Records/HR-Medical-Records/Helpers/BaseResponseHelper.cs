using HR_Medical_Records.DTOs.BaseResponse;

namespace HR_Medical_Records.Helpers
{
    public static class BaseResponseHelper
    {
        public static BaseResponse<T> CreateSuccessful<T>(T data, int? totalRows = null)
        {
            string entityName = typeof(T).Name;
            string message = $"{entityName} created successfully";

            return new BaseResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Code = 200,
                TotalRows = totalRows ?? 0
            };
        }

        public static BaseResponse<T> GetSuccessful<T>(T data, int? totalRows = null)
        {
            string entityName = typeof(T).Name;
            string message = $"{entityName} retrieved successfully";

            return new BaseResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Code = 200,
                TotalRows = totalRows ?? 0
            };
        }

        public static BaseResponse<T> CreateError<T>(string message, int errorCode = 400, T data = default)
        {
            return new BaseResponse<T>
            {
                Success = false,
                Message = message,
                Data = data,
                Code = errorCode,
                TotalRows = 0
            };
        }

    }
}
