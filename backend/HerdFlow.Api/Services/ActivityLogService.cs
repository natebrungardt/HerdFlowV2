using HerdFlow.Api.Data;
using HerdFlow.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HerdFlow.Api.Services;

public class ActivityLogService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ActivityLogService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogAsync(Guid cowId, string description)
    {
        var entry = new ActivityLogEntry
        {
            CowId = cowId,
            Description = description,
            UserId = GetCurrentUserId(),
        };

        _context.ActivityLogEntries.Add(entry);
        await _context.SaveChangesAsync();
    }

    public async Task<List<ActivityLogEntry>> GetByCowIdAsync(Guid cowId)
    {
        return await _context.ActivityLogEntries
            .Where(a => a.CowId == cowId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
    }

    private string GetCurrentUserId()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userId = user?.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? user?.FindFirstValue("sub");

        if (string.IsNullOrWhiteSpace(userId))
        {
            throw new InvalidOperationException("Authenticated user ID is missing.");
        }

        return userId;
    }
}
