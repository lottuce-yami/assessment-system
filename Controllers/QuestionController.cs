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
public class QuestionController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    // GET: api/Question
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<PagedResult<QuestionDto>>> GetQuestion([FromQuery] PaginationParams pagination)
    {
        return await _context.Question
            .Include(q => q.AnswerOptions)
            .ToPagedResultAsync(pagination, q => q.ToDto());
    }

    // GET: api/Question/5
    [HttpGet("{id}")]
    public async Task<ActionResult<QuestionDto>> GetQuestion(Guid id)
    {
        var question = await _context.Question
            .Where(q => q.Id == id)
            .Include(q => q.AnswerOptions)
            .FirstOrDefaultAsync();

        if (question == null)
        {
            return NotFound();
        }

        return question.ToDto();
    }

    // PUT: api/Question/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    // [Authorize(Roles = "Admin")]
    // [HttpPut("{id}")]
    // public async Task<IActionResult> PutQuestion(Guid id, QuestionEditDto questionDto)
    // {
    //     if (id != questionDto.Id) return BadRequest();

    //     var existingQuestion = await _context.Question
    //         .Include(q => q.Quiz)
    //         .ThenInclude(qz => qz.Questions)
    //         .FirstOrDefaultAsync(q => q.Id == id);

    //     if (existingQuestion == null) return NotFound();

    //     var incomingData = questionDto.ToEntity();

    //     _context.Entry(existingQuestion).CurrentValues.SetValues(incomingData);
    //     existingQuestion.Topics = incomingData.Topics;

    //     existingQuestion.Quiz.UpdateMetadata();

    //     try
    //     {
    //         await _context.SaveChangesAsync();
    //     }
    //     catch (DbUpdateConcurrencyException)
    //     {
    //         if (!QuestionExists(id)) return NotFound();
    //         throw;
    //     }

    //     return NoContent();
    // }

    // POST: api/Question
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<QuestionDto>> PostQuestion(QuestionInputAloneDto questionDto)
    {
        var quiz = await _context.Quiz
            .Include(q => q.Questions)
            .FirstOrDefaultAsync(q => q.Id == questionDto.QuizId);

        if (quiz == null) return NotFound("Quiz not found");

        var question = questionDto.ToEntity();
        quiz.Questions.Add(question);

        quiz.UpdateMetadata();

        await _context.SaveChangesAsync();

        return CreatedAtAction("GetQuestion", new { id = question.Id }, question.ToDto());
    }

    // DELETE: api/Question/5
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteQuestion(Guid id)
    {
        var question = await _context.Question
            .Include(q => q.Quiz)
            .ThenInclude(qz => qz.Questions)
            .FirstOrDefaultAsync(q => q.Id == id);

        if (question == null) return NotFound();

        var quiz = question.Quiz;

        _context.Question.Remove(question);

        quiz.Questions.Remove(question); 
        quiz.UpdateMetadata();

        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool QuestionExists(Guid id)
    {
        return _context.Question.Any(e => e.Id == id);
    }
}
