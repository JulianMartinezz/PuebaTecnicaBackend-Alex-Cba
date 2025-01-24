using System.Text.Json.Serialization;

namespace HR_Medical_Records.DTOs.SortingDTOs
{
    public class SortingDTO
    {
        public SORTBY SortBy { get; set; }
        public string Field { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SORTBY { ASC, DESC }
}
