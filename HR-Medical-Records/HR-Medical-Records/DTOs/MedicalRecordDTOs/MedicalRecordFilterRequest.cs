using HR_Medical_Records.DTOs.SortingDTOs;

namespace HR_Medical_Records.DTOs.MedicalRecordDTOs
{
    public class MedicalRecordFilterRequest : Basefilter
    {
        public int? StatusId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? MedicalRecordTypeId { get; set; }
        public int? FieldFilter { get; set; }
    }
}
