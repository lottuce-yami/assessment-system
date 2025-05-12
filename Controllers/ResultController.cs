using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssessmentSystem.Data;
using AssessmentSystem.Models;
using AssessmentSystem.Services.Mappers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AssessmentSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResultController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    // GET: api/Result
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Result>>> GetResult()
    {
        return await _context.Result.ToListAsync();
    }

    // GET: api/Result/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Result>> GetResult(Guid id)
    {
        var result = await _context.Result.FindAsync(id);

        if (result == null)
        {
            return NotFound();
        }

        return result;
    }

    // PUT: api/Result/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutResult(Guid id, Result result)
    {
        if (id != result.Id)
        {
            return BadRequest();
        }

        _context.Entry(result).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!ResultExists(id))
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

    // POST: api/Result
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Result>> PostResult(ResultInputDto dto)
    {
        var result = dto.ToEntity();
        result.UserId = long.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        // If no answers, create empty result
        
        result.Answers = [.. result.Answers.Select(a => _context.Answer.Find(a.Id))];
        // Check if answers are for the same quiz
        // var quizId = result.QuizId;
        // foreach (var answer in result.Answers)
        // {
        //     if (answer.Question.QuizId != quizId) {
        //         return BadRequest("Answers are not for the correct quiz");
        //     }
        // }
        result.Score = result.Answers.Aggregate(0, (total, next) => total + next.Question.Difficulty);

        _context.Result.Add(result);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetResult", new { id = result.Id }, result.ToDto());
    }

    // DELETE: api/Result/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResult(Guid id)
    {
        var result = await _context.Result.FindAsync(id);
        if (result == null)
        {
            return NotFound();
        }

        _context.Result.Remove(result);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool ResultExists(Guid id)
    {
        return _context.Result.Any(e => e.Id == id);
    }
}
