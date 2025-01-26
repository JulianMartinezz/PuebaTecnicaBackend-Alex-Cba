using HR_Medical_Records.DTOs.SortingDTOs;

namespace HR_Medical_Records.DTOs.MedicalRecordDTOs
{
    /// <summary>
    /// Represents the filter request for querying medical records, including various filters for status, 
    /// date range, and medical record type. Inherits from the <see cref="Basefilter"/> class.
    /// </summary>
    public class MedicalRecordFilterRequest : Basefilter
    {
        public int? StatusId { get; set; }
        public DateOnly? StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
        public int? MedicalRecordTypeId { get; set; }
        public int? FieldFilter { get; set; }
    }
}
