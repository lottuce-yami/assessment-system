using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssessmentSystem.Data;
using AssessmentSystem.Models;
using Microsoft.AspNetCore.Authorization;
using AssessmentSystem.Extensions;
using AssessmentSystem.Services.Mappers;

namespace AssessmentSystem.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AnswerOptionController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    // GET: api/AnswerOption
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<PagedResult<AnswerOption>>> GetAnswerOption([FromQuery] PaginationParams pagination)
    {
        return await _context.AnswerOption
            .ToPagedResultAsync(pagination);
    }

    // GET: api/AnswerOption/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AnswerOptionDto>> GetAnswerOption(Guid id)
    {
        var answerOption = await _context.AnswerOption
            .Where(ao => ao.Id == id)
            .Include(ao => ao.Answers)
            .FirstOrDefaultAsync();

        if (answerOption == null)
        {
            return NotFound();
        }

        if (User.IsAdmin())
        {
            return answerOption.ToAdminDto();
        }
        else
        {
            return answerOption.ToDto();
        }
    }

    // PUT: api/AnswerOption/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAnswerOption(Guid id, AnswerOption answerOption)
    {
        if (id != answerOption.Id)
        {
            return BadRequest();
        }

        _context.Entry(answerOption).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AnswerOptionExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/AnswerOption
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<AnswerOptionDto>> PostAnswerOption(AnswerOptionInputAloneDto answerOptionDto)
    {
        var answerOption = answerOptionDto.ToEntity();
        _context.AnswerOption.Add(answerOption);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAnswerOption", new { id = answerOption.Id }, answerOption.ToDto());
    }

    // DELETE: api/AnswerOption/5
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnswerOption(Guid id)
    {
        var answerOption = await _context.AnswerOption.FindAsync(id);
        if (answerOption == null)
        {
            return NotFound();
        }

        _context.AnswerOption.Remove(answerOption);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AnswerOptionExists(Guid id)
    {
        return _context.AnswerOption.Any(e => e.Id == id);
    }
}
