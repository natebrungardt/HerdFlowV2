using HerdFlow.Api.DTOs;
using HerdFlow.Api.Models;
using HerdFlow.Api.Data;

namespace HerdFlow.Api.Services;

public class CowService
{
    private readonly AppDbContext _context;
    private readonly ActivityLogService _activityLogService;
    public CowService(AppDbContext context, ActivityLogService activityLogService)
    {
        _context = context;
        _activityLogService = activityLogService;
    }
    private void ValidateCreateCow(CreateCowDto dto)
    {
        if (dto.LivestockGroup == LivestockGroupType.None)
            throw new ArgumentException("Livestock group is required");
    }

    public List<Cow> GetCows()
    {
        return _context.Cows
            .Where(c => !c.IsRemoved)
            .ToList();
    }
    public Cow? GetCowById(int id)
    {
        return _context.Cows.FirstOrDefault(c => c.Id == id);
    }
    public Cow? UpdateCow(int id, CreateCowDto dto)
    {
        var cow = _context.Cows.Find(id);

        if (cow == null)
            return null;

        ValidateCreateCow(dto);
        var changes = new List<string>();


        if (_context.Cows.Any(c => c.TagNumber == dto.TagNumber && c.Id != id))
            throw new ArgumentException("Tag number already exists");

        if (cow.TagNumber != dto.TagNumber)
            changes.Add($"Tag number changed from {cow.TagNumber} to {dto.TagNumber}");

        if (cow.OwnerName != dto.OwnerName)
            changes.Add($"Owner changed from {cow.OwnerName} to {dto.OwnerName}");

        if (cow.LivestockGroup != dto.LivestockGroup)
            changes.Add($"Livestock group changed from {cow.LivestockGroup} to {dto.LivestockGroup}");

        if (cow.HealthStatus != dto.HealthStatus)
            changes.Add($"Health status changed from {cow.HealthStatus} to {dto.HealthStatus}");

        if (cow.Breed != dto.Breed)
            changes.Add($"Breed changed from {cow.Breed} to {dto.Breed}");

        if (cow.Sex != dto.Sex)
            changes.Add($"Sex changed from {cow.Sex} to {dto.Sex}");

        if (cow.PurchasePrice != dto.PurchasePrice)
            changes.Add($"Purchase price changed from {cow.PurchasePrice} to {dto.PurchasePrice}");

        if (cow.SalePrice != dto.SalePrice)
            changes.Add($"Sale price changed from {cow.SalePrice} to {dto.SalePrice}");

        if (cow.DateOfBirth != dto.DateOfBirth)
            changes.Add($"Date of birth changed from {cow.DateOfBirth:MMM dd, yyyy} to {dto.DateOfBirth:MMM dd, yyyy}");

        if (cow.PurchaseDate != dto.PurchaseDate)
            changes.Add($"Purchase date changed from {cow.PurchaseDate:MMM dd, yyyy} to {dto.PurchaseDate:MMM dd, yyyy}");

        if (cow.SaleDate != dto.SaleDate)
            changes.Add($"Sale date changed from {cow.SaleDate:MMM dd, yyyy} to {dto.SaleDate:MMM dd, yyyy}");

        if (cow.HeatStatus != dto.HeatStatus)
            changes.Add($"Heat status changed from {cow.HeatStatus} to {dto.HeatStatus}");

        if (cow.BreedingStatus != dto.BreedingStatus)
            changes.Add($"Breeding status changed from {cow.BreedingStatus} to {dto.BreedingStatus}");

        // Apply updates

        cow.TagNumber = dto.TagNumber;
        cow.OwnerName = dto.OwnerName;
        cow.LivestockGroup = dto.LivestockGroup;
        cow.Sex = dto.Sex;
        cow.Breed = dto.Breed;
        cow.DateOfBirth = dto.DateOfBirth;
        cow.HealthStatus = dto.HealthStatus;
        cow.HeatStatus = dto.HeatStatus;
        cow.BreedingStatus = dto.BreedingStatus;
        cow.PurchasePrice = dto.PurchasePrice;
        cow.SalePrice = dto.SalePrice;
        cow.PurchaseDate = dto.PurchaseDate;
        cow.SaleDate = dto.SaleDate;


        _context.SaveChanges();
        if (changes.Count == 0)
        {
            _activityLogService.LogAsync(cow.Id, "Cow updated").Wait();
        }
        else
        {
            foreach (var change in changes)
            {
                _activityLogService.LogAsync(cow.Id, change).Wait();
            }
        }

        return cow;
    }
    public Cow CreateCow(CreateCowDto dto)
    {
        ValidateCreateCow(dto);

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
        };
        _context.Cows.Add(cow);
        _context.SaveChanges();
        _activityLogService.LogAsync(cow.Id, "Cow record created").Wait();
        return cow;
    }
    public bool DeleteCow(int id)
    {
        var cow = _context.Cows.FirstOrDefault(c => c.Id == id);

        if (cow == null)
            return false;

        cow.IsRemoved = true;
        _context.SaveChanges();
        _activityLogService.LogAsync(cow.Id, "Cow removed from herd").Wait();

        return true;
    }
    public List<Cow> GetRemovedCows()
    {
        return _context.Cows
            .Where(c => c.IsRemoved)
            .ToList();
    }
    public void RestoreCow(int id)
    {
        var cow = _context.Cows.Find(id);
        if (cow == null) return;

        cow.IsRemoved = false;
        _context.SaveChanges();
        _activityLogService.LogAsync(cow.Id, "Cow restored to herd").Wait();
    }
}
