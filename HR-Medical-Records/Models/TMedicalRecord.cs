﻿namespace HR_Medical_Records.Models;

/// <summary>
/// Represents a medical record with details about the patient's health, medical history, and status.
/// Includes information such as diagnosis, disability, dates, and links to related status and medical record types.
/// </summary>
public partial class TMedicalRecord
{
    public int MedicalRecordId { get; set; }

    public string? Audiometry { get; set; }

    public string? PositionChange { get; set; }

    public string? MotherData { get; set; }

    public string? Diagnosis { get; set; }

    public string? OtherFamilyData { get; set; }

    public string? FatherData { get; set; }

    public string? ExecuteMicros { get; set; }

    public string? ExecuteExtra { get; set; }

    public string? VoiceEvaluation { get; set; }

    public DateOnly? DeletionDate { get; set; }

    public DateOnly? CreationDate { get; set; }

    public DateOnly? ModificationDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public DateOnly? StartDate { get; set; }

    public int? StatusId { get; set; }

    public int? MedicalRecordTypeId { get; set; }

    public string? Disability { get; set; }

    public string? MedicalBoard { get; set; }

    public string? DeletionReason { get; set; }

    public string? Observations { get; set; }

    public decimal? DisabilityPercentage { get; set; }

    public string? DeletedBy { get; set; }

    public string? CreatedBy { get; set; }

    public string? ModifiedBy { get; set; }

    public string? AreaChange { get; set; }

    public int? FileId { get; set; }

    public virtual MedicalRecordType? MedicalRecordType { get; set; }

    public virtual Status? Status { get; set; }
}
