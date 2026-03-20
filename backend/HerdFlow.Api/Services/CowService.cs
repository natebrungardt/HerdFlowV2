using HerdFlow.Api.DTOs;
using HerdFlow.Api.Models;

namespace HerdFlow.Api.Services;

public class CowService
{

    public Cow CreateCow(CreateCowDto dto)
    {
        var cow = new Cow
        {
            Id = 1,
            TagNumber = dto.TagNumber,
            Breed = dto.Breed,
            HealthStatus = dto.HealthStatus,
            HeatStatus = dto.HeatStatus,
            BreedingStatus = dto.BreedingStatus,
            OwnerName = dto.OwnerName,
            DateOfBirth = dto.DateOfBirth,
            PurchasePrice = dto.PurchasePrice,
            SalePrice = dto.SalePrice
        };

        return cow;
    }
}
