namespace HR_Medical_Records.DTOs.MedicalRecordDTOs
{
    public class SoftDeleteMedicalRecord
    {
        public int MedicalRecordId { get; set; }
        public string DeletionReason { get; set; }

    }
}
