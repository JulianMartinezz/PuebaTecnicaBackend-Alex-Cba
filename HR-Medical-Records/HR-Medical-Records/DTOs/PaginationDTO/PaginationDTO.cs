namespace HR_Medical_Records.DTOs.PaginationDTO
{
    public class PaginationDTO<T>
    {
        public int TotalRegisters { get; set; }
        public List<T> Items { get; set; }
    }
}
