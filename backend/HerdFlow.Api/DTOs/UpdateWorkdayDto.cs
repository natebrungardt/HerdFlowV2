using System.ComponentModel.DataAnnotations;

public class UpdateWorkdayDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public DateTime Date { get; set; }
}
