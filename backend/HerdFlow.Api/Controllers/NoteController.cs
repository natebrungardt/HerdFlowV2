using HerdFlow.Api.DTOs;
using HerdFlow.Api.Models;
using Microsoft.AspNetCore.Mvc;
using HerdFlow.Api.Data;

namespace HerdFlow.Api.Controllers;

[ApiController]
[Route("api/cows/{cowId:int}/notes")]
public class NoteController : ControllerBase
{
    private readonly AppDbContext _context;

    public NoteController(AppDbContext context)
    {
        _context = context;
    }

    // GET notes for a cow
    [HttpGet]
    public IActionResult GetNotes(int cowId)
    {
        var notes = _context.Notes
            .Where(n => n.CowId == cowId)
            .OrderByDescending(n => n.CreatedAt)
            .ToList();

        return Ok(notes);
    }

    // CREATE note
    [HttpPost]
    public IActionResult CreateNote(int cowId, [FromBody] CreateNoteDto dto)
    {
        var note = new Note
        {
            CowId = cowId,
            Content = dto.Content
        };

        _context.Notes.Add(note);
        _context.SaveChanges();

        return Ok(note);
    }
}
