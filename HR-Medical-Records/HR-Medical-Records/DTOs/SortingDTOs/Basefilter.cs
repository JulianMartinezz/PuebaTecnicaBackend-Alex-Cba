namespace HR_Medical_Records.DTOs.SortingDTOs
{
    public class Basefilter
    {
        public int? Limit { get; set; }
        public int? Skip { get; set; }
        public SORTBY? SortBy { get; set; }
        public string? ColumnFilter { get; set; }
        public List<SortingDTO>? Sorting { get; set; }
    }
}
