namespace HR_Medical_Records.DTOs.MedicalRecordDTOs
{
    public class MedicalRecordDTO
    {
        public int MedicalRecordId { get; set; }
        public string? Audiometry { get; set; }
        public string? Diagnosis { get; set; }
        public string? Disability { get; set; }
        public decimal? DisabilityPercentage { get; set; }
        public string? Observations { get; set; }
        public string? VoiceEvaluation { get; set; }
        public string? MedicalBoard { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? StatusDescription { get; set; }
        public string? MedicalRecordTypeDescription { get; set; }

        // Metadata for deleted or modified records
        public bool IsDeleted => DeletionDate.HasValue;
        public DateOnly? DeletionDate { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletionReason { get; set; }
        public DateOnly? ModificationDate { get; set; }
        public string? ModifiedBy { get; set; }

        // Computed property to summarize key aspects
        public string Summary => $"{MedicalRecordTypeDescription} - {StatusDescription} ({DisabilityPercentage?.ToString() ?? "0"}% Disability)";
    }
}
