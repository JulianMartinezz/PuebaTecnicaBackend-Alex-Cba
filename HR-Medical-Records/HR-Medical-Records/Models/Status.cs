namespace HR_Medical_Records.Models;

/// <summary>
/// Represents the status of a medical record.
/// Contains properties for the status's ID, name, description, and associated medical records.
/// </summary>
public partial class Status
{
    public int StatusId { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<TMedicalRecord> TMedicalRecords { get; set; } = new List<TMedicalRecord>();
}
