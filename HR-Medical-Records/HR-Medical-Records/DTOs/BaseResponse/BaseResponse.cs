namespace HR_Medical_Records.DTOs.BaseResponse
{
    /// <summary>
    /// Represents a base response structure for API responses, providing success status, message, data, and additional metadata.
    /// </summary>
    /// <typeparam name="T">The type of the data in the response.</typeparam>
    public class BaseResponse<T>
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public int? Code { get; set; }
        public int? TotalRows { get; set; }
        public string? Exception { get; set; }

    }
}
