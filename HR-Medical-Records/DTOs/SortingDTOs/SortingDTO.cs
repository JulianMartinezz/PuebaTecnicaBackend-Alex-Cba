using System.Text.Json.Serialization;

/// <summary>
/// Represents the sorting criteria for a particular field in a query, including the sort direction (ascending or descending).
/// </summary>
namespace HR_Medical_Records.DTOs.SortingDTOs
{
    public class SortingDTO
    {
        public SORTBY SortBy { get; set; }
        public string Field { get; set; }
    }

    /// <summary>
    /// Enum representing sorting directions.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SORTBY { ASC, DESC }
}
