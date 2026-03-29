using System.ComponentModel.DataAnnotations;

namespace HerdFlow.Api.Models;

public class Note
{
    public Guid Id { get; set; }

    [Required]
    public string UserId { get; set; } = null!;

    public Guid CowId { get; set; }

    [Required]
    [MaxLength(1000)]
    public string Content { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
