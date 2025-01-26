namespace HR_Medical_Records.DTOs.MedicalRecordDTOs
{
    /// <summary>
    /// Represents a simplified version of a medical record with essential information such as diagnosis, 
    /// disability status, and relevant dates.
    /// </summary>
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
