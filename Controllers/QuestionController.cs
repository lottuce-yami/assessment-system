using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssessmentSystem.Data;
using AssessmentSystem.Models;

namespace AssessmentSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuestionController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    // GET: api/Question
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Question>>> GetQuestion()
    {
        return await _context.Question.ToListAsync();
    }

    // GET: api/Question/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Question>> GetQuestion(Guid id)
    {
        var question = await _context.Question.FindAsync(id);

        if (question == null)
        {
            return NotFound();
        }

        return question;
    }

    // PUT: api/Question/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutQuestion(Guid id, Question question)
    {
        if (id != question.Id)
        {
            return BadRequest();
        }

        _context.Entry(question).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!QuestionExists(id))
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

    // POST: api/Question
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Question>> PostQuestion(Question question)
    {
        _context.Question.Add(question);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetQuestion", new { id = question.Id }, question);
    }

    // DELETE: api/Question/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(Guid id)
    {
        var question = await _context.Question.FindAsync(id);
        if (question == null)
        {
            return NotFound();
        }

        _context.Question.Remove(question);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool QuestionExists(Guid id)
    {
        return _context.Question.Any(e => e.Id == id);
    }
}
