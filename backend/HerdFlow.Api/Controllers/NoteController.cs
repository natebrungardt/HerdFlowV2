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
        var cowExists = _context.Cows.Any(c => c.Id == cowId);
        if (!cowExists)
            return NotFound(new { message = "Cow not found" });

        if (string.IsNullOrWhiteSpace(dto.Content))
            return BadRequest(new { message = "Note content is required" });

        var note = new Note
        {
            CowId = cowId,
            Content = dto.Content
        };

        _context.Notes.Add(note);
        _context.SaveChanges();

        return Ok(note);
    }
    [HttpDelete("{noteId:int}")]
    public IActionResult DeleteNote(int cowId, int noteId)
    {
        var note = _context.Notes
            .FirstOrDefault(n => n.Id == noteId && n.CowId == cowId);

        if (note == null)
            return NotFound();

        _context.Notes.Remove(note);
        _context.SaveChanges();

        return NoContent();
    }
    [HttpPut("{noteId:int}")]
    public IActionResult UpdateNote(int cowId, int noteId, [FromBody] CreateNoteDto dto)
    {
        var note = _context.Notes
            .FirstOrDefault(n => n.Id == noteId && n.CowId == cowId);

        if (note == null)
            return NotFound();

        if (string.IsNullOrWhiteSpace(dto.Content))
            return BadRequest(new { message = "Note content is required" });

        note.Content = dto.Content;
        _context.SaveChanges();

        return Ok(note);
    }
}
