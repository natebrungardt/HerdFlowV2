using HerdFlow.Api.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace HerdFlow.Api.DTOs;

public class CreateCowDto
{
    public string TagNumber { get; set; } = null!;
    public string OwnerName { get; set; } = null!;
    public string LivestockGroup { get; set; } = null!;
    public string Sex { get; set; } = null!;
    public string Breed { get; set; } = null!;
    public DateOnly? DateOfBirth { get; set; }
    [Required]
    [EnumDataType(typeof(HealthStatusType))]
    public HealthStatusType HealthStatus { get; set; }
    public string HeatStatus { get; set; } = null!;
    public string BreedingStatus { get; set; } = null!;

    public decimal? PurchasePrice { get; set; }
    public decimal? SalePrice { get; set; }
    public DateOnly? PurchaseDate { get; set; }
    public DateOnly? SaleDate { get; set; }

    public string? Notes { get; set; }
    public DateOnly? HarvestDate { get; set; }
}
