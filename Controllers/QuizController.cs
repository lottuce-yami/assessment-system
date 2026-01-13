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
public class QuizController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    // GET: api/Quiz
    [AllowAnonymous]
    [HttpGet]
    public async Task<ActionResult<PagedResult<QuizDto>>> GetQuiz(
        [FromQuery] PaginationParams pagination,
        [FromQuery] string? search = null,
        [FromQuery] string? topic = null)
    {
        var query = _context.Quiz
            .Include(q => q.Questions)
            .Include(q => q.Results)
            .AsQueryable();

        // TODO sqlite can't lowercase cyrillic properly, implement a workaround
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(q => q.Title.Contains(search.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(topic))
        {
            query = query.Where(q => q.Questions.Any(qu => qu.Topics.Any(t => t == topic.ToLower())));
        }

        if (User.Identity!.IsAuthenticated)
        {
            var user = await _context.Users.FindAsync(User.GetId());

            var quizzes = await query.ToListAsync();

            var relevantQuizzes = quizzes
                .Select(quiz => new {
                    Quiz = quiz,
                    Relevance = CalculateRelevance(quiz, user!)
                })
                .OrderByDescending(q => q.Relevance)
                .ToPagedResult(pagination, q => q.Quiz.ToDto());

            return relevantQuizzes;
        }
        else
        {
            return await query
                .ToPagedResultAsync(pagination, q => q.ToDto());
        }
    }

    private static int CalculateRelevance(Quiz quiz, User user)
    {
        int relevance = 0;

        foreach (var question in quiz.Questions)
        {
            int topicMatches = question.Topics.Count(t => user.Interests.Contains(t));
            relevance += topicMatches * 2;

            var expectedDifficulty = GetRecommendedDifficulty(user.Age);
            var diffDelta = Math.Abs(expectedDifficulty - question.Difficulty);
            relevance += 5 - diffDelta;
        }

        return relevance;
    }

    private static int GetRecommendedDifficulty(int age)
    {
        return age switch
        {
            <= 13 => 1,
            <= 16 => 2,
            <= 18 => 3,
            <= 22 => 4,
            _ => 5
        };
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
    public async Task<IActionResult> PutQuiz(long id, QuizEditDto quizDto)
    {
        if (id != quizDto.Id)
        {
            return BadRequest();
        }

        var quiz = quizDto.ToEntity();

        _context.Entry(quiz).State = EntityState.Modified;
        _context.Entry(quiz).Property(q => q.MaxScore).IsModified = false;
        _context.Entry(quiz).Collection(q => q.Questions).IsModified = false;
        _context.Entry(quiz).Collection(q => q.Results).IsModified = false;

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
