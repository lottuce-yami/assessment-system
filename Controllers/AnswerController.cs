using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssessmentSystem.Data;
using AssessmentSystem.Models;

namespace AssessmentSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnswerController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    // GET: api/Answer
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Answer>>> GetAnswer()
    {
        return await _context.Answer.ToListAsync();
    }

    // GET: api/Answer/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Answer>> GetAnswer(Guid id)
    {
        var answer = await _context.Answer.FindAsync(id);

        if (answer == null)
        {
            return NotFound();
        }

        return answer;
    }

    // PUT: api/Answer/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutAnswer(Guid id, Answer answer)
    {
        if (id != answer.Id)
        {
            return BadRequest();
        }

        _context.Entry(answer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!AnswerExists(id))
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

    // POST: api/Answer
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Answer>> PostAnswer(Answer answer)
    {
        _context.Answer.Add(answer);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetAnswer", new { id = answer.Id }, answer);
    }

    // DELETE: api/Answer/5
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
