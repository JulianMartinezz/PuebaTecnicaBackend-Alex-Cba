namespace HR_Medical_Records.DTOs.MedicalRecordDTOs
{
    public class SimpleMedicalRecordDTO
    {
        public int MedicalRecordId { get; set; }
        public string? Diagnosis { get; set; }
        public string? Disability { get; set; }
        public decimal? DisabilityPercentage { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
