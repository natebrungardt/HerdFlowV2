using Microsoft.AspNetCore.Mvc;
using HerdFlow.Api.DTOs;
using HerdFlow.Api.Services;

namespace HerdFlow.Api.Controllers;

[ApiController]
[Route("api/cow")]
public class CowController : ControllerBase
{
    private readonly CowService _cowService;

    public CowController()
    {
        _cowService = new CowService();
    }

    [HttpGet]
    public IActionResult GetCows()
    {
        var cows = new[]
        {
            new { Id = 1, Name = "Bessie" },
            new { Id = 2, Name = "Mooana" }
        };

        return Ok(cows);
    }
    [HttpPost]
    public IActionResult CreateCow([FromBody] CreateCowDto dto)
    {
        var cow = _cowService.CreateCow(dto);
        return Ok(cow);
    }
}
