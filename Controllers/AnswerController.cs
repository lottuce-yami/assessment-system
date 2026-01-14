using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssessmentSystem.Data;
using AssessmentSystem.Models;
using AssessmentSystem.Services.Mappers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AssessmentSystem.Extensions;

namespace AssessmentSystem.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class AnswerController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    // GET: api/Answer
    [HttpGet]
    public async Task<ActionResult<PagedResult<AnswerDto>>> GetAnswer([FromQuery] PaginationParams pagination)
    {
        if (User.IsAdmin())
        {
            return await _context.Answer
                .Include(a => a.SelectedOptions)
                .Include(a => a.Question)
                .Include(a => a.Result)
                .ToPagedResultAsync(pagination, a => a.ToDto());
        }

        return await _context.Answer
            .Where(a => a.Result.UserId == User.GetId())
            .Include(a => a.SelectedOptions)
            .Include(a => a.Question)
            .Include(a => a.Result)
            .ToPagedResultAsync(pagination, a => a.ToDto());
    }

    // GET: api/Answer/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AnswerDto>> GetAnswer(Guid id)
    {
        var answer = await _context.Answer
            .Where(a => a.Id == id)
            .Include(a => a.SelectedOptions)
            .Include(a => a.Question)
            .Include(a => a.Result)
            .FirstOrDefaultAsync();
        
        if (answer == null)
        {
            return NotFound();
        }

        if (User.GetId() != answer.Result.UserId && !User.IsAdmin())
        {
            return Forbid();
        }

        return answer.ToDto();
    }

    // POST: api/Answer
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Answer>> PostAnswer(AnswerInputDto dto)
    {
        var currentTime = DateTime.UtcNow;

        var options = await _context.AnswerOption
            .AsNoTracking()
            .Where(ao => ao.QuestionId == dto.QuestionId)
            .Select(ao => new { ao.Id, ao.IsCorrect })
            .ToListAsync();

        var validOptions = options.Select(ao => ao.Id).ToList();

        if (dto.SelectedOptions.Any(so => !validOptions.Contains(so)))
        {
            return BadRequest();
        }

        var answer = dto.ToEntity();
        answer.AnsweredAt = currentTime;
        foreach (var selectedOption in answer.SelectedOptions)
        {
            _context.Attach(selectedOption);
        }

        _context.Answer.Add(answer);
        await _context.SaveChangesAsync();

        var correctOptions = options.Where(ao => ao.IsCorrect).Select(ao => ao.Id).ToHashSet();

        if (correctOptions.SetEquals(dto.SelectedOptions))
        {
            var difficulty = await _context.Question
                .Where(q => q.Id == dto.QuestionId)
                .Select(q => q.Difficulty)
                .FirstOrDefaultAsync();

            if (difficulty > 0)
            {
                await _context.Result
                    .Where(r => r.Id == dto.ResultId)
                    .ExecuteUpdateAsync(calls => calls.SetProperty(r => r.Score, r => r.Score + difficulty));
            }
        }

        await _context.Result
            .Where(r => r.Id == dto.ResultId)
            .ExecuteUpdateAsync(calls => calls.SetProperty(r => r.FinishedAt, currentTime));

        return CreatedAtAction("GetAnswer", new { id = answer.Id }, answer.ToDto());
    }

    // DELETE: api/Answer/5
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAnswer(Guid id)
    {
        var answer = await _context.Answer.FindAsync(id);
        if (answer == null)
        {
            return NotFound();
        }

        _context.Answer.Remove(answer);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool AnswerExists(Guid id)
    {
        return _context.Answer.Any(e => e.Id == id);
    }
}
