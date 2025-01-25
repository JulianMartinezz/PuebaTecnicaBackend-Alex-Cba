using HR_Medical_Records.DTOs.BaseResponse;

namespace HR_Medical_Records.Helpers
{
    public static class BaseResponseHelper
    {
        public static int? AffectedRows { get; set; }

        public static BaseResponse<T> CreateSuccessful<T>(T data, int? totalRows = null)
        {
            string entityName = typeof(T).Name;
            string message = $"{entityName} created successfully";

            return new BaseResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Code = 201,
                TotalRows = totalRows.HasValue ? totalRows.Value : AffectedRows.HasValue ? AffectedRows.Value : 0,
            };
        }

        public static BaseResponse<T> UpdateSuccessful<T>(T data, int? totalRows = null)
        {
            string entityName = typeof(T).Name;
            string message = $"{entityName} has been successfully updated";

            return new BaseResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Code = 200,
                TotalRows = totalRows.HasValue ? totalRows.Value : AffectedRows.HasValue ? AffectedRows.Value : 0,
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
                TotalRows = totalRows.HasValue ? totalRows.Value : AffectedRows.HasValue ? AffectedRows.Value : 0,
            };
        }

        public static BaseResponse<T> SoftDeleteSuccessful<T>(T data, int? totalRows = null)
        {
            string entityName = typeof(T).Name;
            string message = $"{entityName} has been successfully eliminated";

            return new BaseResponse<T>
            {
                Success = true,
                Message = message,
                Data = data,
                Code = 200,
                TotalRows = totalRows.HasValue ? totalRows.Value : AffectedRows.HasValue ? AffectedRows.Value : 0,
            };
        }

    }
}
