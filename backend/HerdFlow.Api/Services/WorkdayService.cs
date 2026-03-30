using HerdFlow.Api.Data;
using HerdFlow.Api.Exceptions;
using HerdFlow.Api.Models;
using Microsoft.EntityFrameworkCore;
using HerdFlow.Api.DTOs;
using Microsoft.AspNetCore.Http;
using Npgsql;
using System.Security.Claims;

namespace HerdFlow.Api.Services;

public class WorkdayService
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public WorkdayService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // CREATE
    public async Task<Workday> CreateWorkday(CreateWorkdayDto dto)
    {
        var distinctCowIds = dto.CowIds
            .Distinct()
            .ToList();

        var cows = distinctCowIds.Count == 0
            ? new List<Cow>()
            : await _context.Cows
                .Where(c => distinctCowIds.Contains(c.Id) && !c.IsRemoved)
                .ToListAsync();

        if (cows.Count != distinctCowIds.Count)
        {
            throw new ValidationException("One or more selected cows could not be added to the workday.");
        }

        var workday = new Workday
        {
            UserId = GetCurrentUserId(),
            Title = dto.Title.Trim(),
            Date = NormalizeWorkdayDate(dto.Date),
            Summary = dto.Summary,
            WorkdayCows = cows.Select(cow => new WorkdayCow
            {
                CowId = cow.Id
            }).ToList()
        };

        _context.Workdays.Add(workday);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (WasDuplicatePrimaryKeyInsert(ex))
        {
            var existingWorkday = await _context.Workdays
                .Include(w => w.WorkdayCows)
                    .ThenInclude(wc => wc.Cow)
                .Include(w => w.WorkdayNotes)
                .FirstOrDefaultAsync(w => w.Id == workday.Id);

            if (existingWorkday is not null)
            {
                return existingWorkday;
            }

            throw;
        }

        return workday;
    }

    // READ - Active Workdays
    public async Task<List<Workday>> GetActiveWorkdays()
    {
        return await _context.Workdays
            .Where(w => !w.IsArchived)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    // READ - Archived Workdays
    public async Task<List<Workday>> GetArchivedWorkdays()
    {
        return await _context.Workdays
            .Where(w => w.IsArchived)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
    }

    // READ - By Id (with cows + notes)
    public async Task<Workday> GetWorkdayById(Guid id)
    {
        var workday = await _context.Workdays
            .Include(w => w.WorkdayCows)
                .ThenInclude(wc => wc.Cow)
            .Include(w => w.WorkdayNotes)
            .FirstOrDefaultAsync(w => w.Id == id);

        return workday ?? throw new NotFoundException("Workday not found.");
    }

    public async Task AddCowsToWorkday(Guid id, List<Guid> cowIds)
    {
        var workday = await _context.Workdays
            .Include(w => w.WorkdayCows)
            .FirstOrDefaultAsync(w => w.Id == id);

        if (workday == null)
        {
            throw new NotFoundException("Workday not found.");
        }

        var distinctCowIds = cowIds
            .Distinct()
            .ToList();

        if (distinctCowIds.Count == 0)
        {
            return;
        }

        var existingCowIds = workday.WorkdayCows
            .Select(wc => wc.CowId)
            .ToHashSet();

        var newCowIds = distinctCowIds
            .Where(cowId => !existingCowIds.Contains(cowId))
            .ToList();

        if (newCowIds.Count == 0)
        {
            return;
        }

        var cows = await _context.Cows
            .Where(c => newCowIds.Contains(c.Id) && !c.IsRemoved)
            .ToListAsync();

        if (cows.Count != newCowIds.Count)
        {
            throw new ValidationException("One or more selected cows could not be added to the workday.");
        }

        foreach (var cowId in newCowIds)
        {
            workday.WorkdayCows.Add(new WorkdayCow
            {
                CowId = cowId
            });
        }

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex) when (WasDuplicateWorkdayCowInsert(ex))
        {
            var refreshedWorkday = await _context.Workdays
                .Include(w => w.WorkdayCows)
                .FirstOrDefaultAsync(w => w.Id == id);

            if (refreshedWorkday is not null &&
                distinctCowIds.All(cowId => refreshedWorkday.WorkdayCows.Any(wc => wc.CowId == cowId)))
            {
                return;
            }

            throw;
        }
    }

    public async Task RemoveCowFromWorkday(Guid id, Guid cowId)
    {
        var workdayCow = await _context.WorkdayCows
            .FirstOrDefaultAsync(wc => wc.WorkdayId == id && wc.CowId == cowId);

        if (workdayCow == null)
        {
            throw new NotFoundException("Cow assignment not found for this workday.");
        }

        _context.WorkdayCows.Remove(workdayCow);
        await _context.SaveChangesAsync();
    }

    // UPDATE - Archive Workday
    public async Task ArchiveWorkday(Guid id)
    {
        var workday = await FindWorkdayAsync(id);

        workday.IsArchived = true;
        await _context.SaveChangesAsync();
    }

    // UPDATE - Restore Workday
    public async Task RestoreWorkday(Guid id)
    {
        var workday = await FindWorkdayAsync(id);

        workday.IsArchived = false;
        await _context.SaveChangesAsync();
    }

    // DELETE (optional hard delete)
    public async Task DeleteWorkday(Guid id)
    {
        var workday = await FindWorkdayAsync(id);

        _context.Workdays.Remove(workday);
        await _context.SaveChangesAsync();
    }

    private async Task<Workday> FindWorkdayAsync(Guid id)
    {
        var workday = await _context.Workdays.FindAsync(id);
        return workday ?? throw new NotFoundException("Workday not found.");
    }

    public async Task<Workday> UpdateWorkday(Guid id, UpdateWorkdayDto dto)
    {
        var workday = await FindWorkdayAsync(id);

        workday.Title = dto.Title.Trim();
        workday.Summary = dto.Summary;
        workday.Date = NormalizeWorkdayDate(dto.Date);

        await _context.SaveChangesAsync();

        return workday;
    }

    private static DateOnly NormalizeWorkdayDate(DateOnly? value)
    {
        return value ?? DateOnly.FromDateTime(DateTime.UtcNow);
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

    private static bool WasDuplicatePrimaryKeyInsert(DbUpdateException exception)
    {
        return exception.InnerException is PostgresException postgresException
            && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
            && postgresException.ConstraintName == "PK_Workdays";
    }

    private static bool WasDuplicateWorkdayCowInsert(DbUpdateException exception)
    {
        return exception.InnerException is PostgresException postgresException
            && postgresException.SqlState == PostgresErrorCodes.UniqueViolation
            && postgresException.ConstraintName == "IX_WorkdayCows_WorkdayId_CowId";
    }
}
