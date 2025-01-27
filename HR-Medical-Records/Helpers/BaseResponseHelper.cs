using HR_Medical_Records.DTOs.BaseResponse;

namespace HR_Medical_Records.Helpers
{
    /// <summary>
    /// Helper class to create standardized success responses for various CRUD operations, including Create, Update, Get, and SoftDelete.
    /// </summary>
    public static class BaseResponseHelper
    {
        /// <summary>
        /// The number of rows affected by the operation.
        /// </summary>
        public static int? AffectedRows { get; set; }

        /// <summary>
        /// Creates a successful response for a created entity.
        /// </summary>
        /// <typeparam name="T">The type of the entity being created.</typeparam>
        /// <param name="data">The data to be included in the response.</param>
        /// <param name="totalRows">The total number of rows, optional.</param>
        /// <returns>A <see cref="BaseResponse{T}"/> indicating the success of the creation operation.</returns>
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

        /// <summary>
        /// Creates a successful response for an updated entity.
        /// </summary>
        /// <typeparam name="T">The type of the entity being updated.</typeparam>
        /// <param name="data">The data to be included in the response.</param>
        /// <param name="totalRows">The total number of rows, optional.</param>
        /// <returns>A <see cref="BaseResponse{T}"/> indicating the success of the update operation.</returns>
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

        /// <summary>
        /// Creates a successful response for a retrieved entity.
        /// </summary>
        /// <typeparam name="T">The type of the entity being retrieved.</typeparam>
        /// <param name="data">The data to be included in the response.</param>
        /// <param name="totalRows">The total number of rows, optional.</param>
        /// <returns>A <see cref="BaseResponse{T}"/> indicating the success of the retrieval operation.</returns>
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

        /// <summary>
        /// Creates a successful response for a soft-deleted entity.
        /// </summary>
        /// <typeparam name="T">The type of the entity being soft-deleted.</typeparam>
        /// <param name="data">The data to be included in the response.</param>
        /// <param name="totalRows">The total number of rows, optional.</param>
        /// <returns>A <see cref="BaseResponse{T}"/> indicating the success of the soft-delete operation.</returns>
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
