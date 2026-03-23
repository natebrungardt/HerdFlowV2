using HerdFlow.Api.DTOs;
using HerdFlow.Api.Models;
using HerdFlow.Api.Data;

namespace HerdFlow.Api.Services;

public class CowService
{
    private readonly AppDbContext _context;

    public CowService(AppDbContext context)
    {
        _context = context;
    }
    public Cow CreateCow(CreateCowDto dto)
    {
        var cow = new Cow
        {
            TagNumber = dto.TagNumber,
            OwnerName = dto.OwnerName,
            LivestockGroup = dto.LivestockGroup,
            Sex = dto.Sex,
            Breed = dto.Breed,
            DateOfBirth = dto.DateOfBirth,
            HealthStatus = dto.HealthStatus,
            HeatStatus = dto.HeatStatus,
            BreedingStatus = dto.BreedingStatus,
            PurchasePrice = dto.PurchasePrice,
            SalePrice = dto.SalePrice,
            PurchaseDate = dto.PurchaseDate,
            SaleDate = dto.SaleDate,
            Notes = dto.Notes,
            HarvestDate = dto.HarvestDate
        };
        _context.Cows.Add(cow);
        _context.SaveChanges();
        return cow;
    }

    public List<Cow> GetCows()
    {
        return _context.Cows.ToList();
    }
}
