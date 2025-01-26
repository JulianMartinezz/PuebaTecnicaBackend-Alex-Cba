namespace HR_Medical_Records.DTOs.MedicalRecordDTOs
{
    /// <summary>
    /// Represents a request to soft-delete a medical record, storing the record ID and the reason for deletion.
    /// </summary>
    public class SoftDeleteMedicalRecord
    {
        public int MedicalRecordId { get; set; }
        public string DeletionReason { get; set; }

    }
}
