namespace HR_Medical_Records.DTOs.PaginationDTO
{
    /// <summary>
    /// Represents a paginated response containing a total count of items and a list of the current items.
    /// </summary>
    /// <typeparam name="T">The type of items being paginated.</typeparam>
    public class PaginationDTO<T>
    {
        public int TotalRegisters { get; set; }
        public List<T> Items { get; set; }
    }
}
