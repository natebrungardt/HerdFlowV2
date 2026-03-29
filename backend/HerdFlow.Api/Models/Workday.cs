using System.ComponentModel.DataAnnotations;

namespace HerdFlow.Api.Models;

public class Workday
{
    public Guid Id { get; set; }
    [Required]
    public string UserId { get; set; } = null!;
    [Required]
    [MaxLength(120)]
    public string Title { get; set; } = string.Empty; // "Workday 1"

    public DateOnly Date { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow);
    public string? Summary { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsArchived { get; set; } = false;
    // Relationships
    public List<WorkdayCow> WorkdayCows { get; set; } = new();
    public List<WorkdayNote> WorkdayNotes { get; set; } = new();

}
