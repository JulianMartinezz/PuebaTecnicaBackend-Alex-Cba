namespace HR_Medical_Records.Models;

public partial class Applicationuser
{
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? Statusid { get; set; }

    public virtual Status? Status { get; set; }
}
