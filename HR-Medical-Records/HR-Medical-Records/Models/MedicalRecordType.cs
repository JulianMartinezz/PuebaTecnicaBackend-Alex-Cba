namespace HR_Medical_Records.Models;

/// <summary>
/// Represents the type of a medical record.
/// Contains properties for the type's ID, name, description, and associated medical records.
/// </summary>
public partial class MedicalRecordType
{
    public int MedicalRecordTypeId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TMedicalRecord> TMedicalRecords { get; set; } = new List<TMedicalRecord>();
}
