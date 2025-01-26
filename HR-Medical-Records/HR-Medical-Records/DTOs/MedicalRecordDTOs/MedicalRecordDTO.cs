namespace HR_Medical_Records.DTOs.MedicalRecordDTOs
{
    /// <summary>
    /// Represents the data transfer object (DTO) for a medical record, containing relevant information 
    /// including diagnosis, status, and metadata such as creation, modification, and deletion details.
    /// </summary>
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
        public string? FatherData { get; set; }
        public string? MotherData { get; set; }
        public string? OtherFamilyData { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public DateOnly? CreationDate { get; set; }
        public string? CreatedBy { get; set; }
        public int? StatusId { get; set; }
        public string? Status { get; set; }
        public string? StatusDescription { get; set; }
        public int? MedicalRecordTypeId { get; set; }
        public string? MedicalRecordType { get; set; }
        public string? MedicalRecordTypeDescription { get; set; }
        public string? ExecuteMicros { get; set; }
        public string? ExecuteExtra { get; set; }
        public string? PositionChange { get; set; }

        // Metadata for deleted or modified records
        public bool IsDeleted => DeletionDate.HasValue;
        public DateOnly? DeletionDate { get; set; }
        public string? DeletedBy { get; set; }
        public string? DeletionReason { get; set; }
        public DateOnly? ModificationDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? AreaChange { get; set; }

        // Computed property to summarize key aspects
        public string Summary => $"{MedicalRecordTypeDescription} - {StatusDescription} ({DisabilityPercentage?.ToString() ?? "0"}% Disability)";
    }
}
