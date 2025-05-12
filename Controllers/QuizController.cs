using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssessmentSystem.Data;
using AssessmentSystem.Models;
using AssessmentSystem.Services.Mappers;
using Microsoft.AspNetCore.Authorization;

namespace AssessmentSystem.Controllers;

[Route("api/[controller]")]
[ApiController]
public class QuizController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    // GET: api/Quiz
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Quiz>>> GetQuiz()
    {
        return await _context.Quiz.ToListAsync();
    }

    // GET: api/Quiz/5
    [HttpGet("{id}")]
    public async Task<ActionResult<QuizDto>> GetQuiz(long id)
    {
        var quiz = await _context.Quiz
            .Where(q => q.Id == id)
            .Include(q => q.Questions)
            .Include(q => q.Results)
            .FirstOrDefaultAsync();

        if (quiz == null)
        {
            return NotFound();
        }

        return quiz.ToDto();
    }

    // PUT: api/Quiz/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> PutQuiz(long id, Quiz quiz)
    {
        if (id != quiz.Id)
        {
            return BadRequest();
        }

        _context.Entry(quiz).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!QuizExists(id))
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

    // POST: api/Quiz
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<Quiz>> PostQuiz(QuizInputDto quizDto)
    {
        var quiz = quizDto.ToEntity();
        _context.Quiz.Add(quiz);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetQuiz", new { id = quiz.Id }, quiz.ToDto());
    }

    // DELETE: api/Quiz/5
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuiz(long id)
    {
        var quiz = await _context.Quiz.FindAsync(id);
        if (quiz == null)
        {
            return NotFound();
        }

        _context.Quiz.Remove(quiz);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool QuizExists(long id)
    {
        return _context.Quiz.Any(e => e.Id == id);
    }
}
