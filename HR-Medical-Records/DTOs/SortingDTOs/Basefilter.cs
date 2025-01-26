namespace HR_Medical_Records.DTOs.SortingDTOs
{
    /// <summary>
    /// Base class for filters used in queries. Provides pagination, sorting, and column-based filtering capabilities.
    /// </summary>
    public class Basefilter
    {
        public int? Limit { get; set; } = 10;
        public int? Skip { get; set; } = 0;
        public SORTBY? SortBy { get; set; }
        public string? ColumnFilter { get; set; }
        public List<SortingDTO>? Sorting { get; set; }
    }
}
