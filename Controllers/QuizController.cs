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
            .AsNoTracking()
            .AsQueryable();

        // TODO sqlite can't lowercase cyrillic properly, implement a workaround
        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query
                .Where(q => q.Title.Contains(search.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(topic))
        {
            query = query
                .Where(q => q.MainTopics.Any(t => t == topic));
        }

        query = query
            .OrderBy(q => q.Title);

        return await query
            .ToPagedResultAsync(pagination, q => q.ToDto());
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

    // GET: api/Quiz/topics
    [AllowAnonymous]
    [HttpGet("topics")]
    public async Task<ActionResult<List<string>>> GetAvailableTopics()
    {
        var allTopicLists = await _context.Quiz
            .Select(q => q.MainTopics)
            .ToListAsync();

        var topics = allTopicLists
            .SelectMany(t => t)
            .Distinct()
            .OrderBy(t => t)
            .ToList();

        return topics;
    }

    [HttpGet("recommendations/general")]
    public async Task<ActionResult<List<QuizDto>>> GetGeneralRecommendations()
    {
        var user = await _context.Users.FindAsync(User.GetId());

        int targetDiff = GetRecommendedDifficulty(user!.Age);

        var candidates = await _context.Quiz
            .Where(q => q.Questions.Any(x => Math.Abs(x.Difficulty - targetDiff) <= 1))
            .Include(q => q.Questions) 
            .Include(q => q.Results)
            .ToListAsync();

        var randomQuizzes = candidates
            .OrderBy(q => Guid.NewGuid())
            .Take(5)
            .Select(q => q.ToDto())
            .ToList();

        return randomQuizzes;
    }

    // GET: api/Quiz/recommendations/personalized
    [HttpGet("recommendations/personalized")]
    public async Task<ActionResult<List<QuizDto>>> GetPersonalizedRecommendations()
    {
        var userId = User.GetId();

        var userResults = await _context.Result
            .Where(r => r.UserId == userId)
            .Include(r => r.Quiz)
            .ToListAsync();

        var passedQuizzes = userResults
            .Where(r => r.Quiz.MaxScore > 0 && ((double)r.Score / r.Quiz.MaxScore) > 0.6)
            .ToList();

        var strongTopics = passedQuizzes
            .SelectMany(r => r.Quiz.MainTopics)
            .Distinct()
            .ToList();

        if (!strongTopics.Any())
        {
            return new List<QuizDto>();
        }

        var takenQuizIds = userResults.Select(r => r.QuizId).ToHashSet();

        var allQuizzes = await _context.Quiz
            .Include(q => q.Questions)
            .Include(q => q.Results)
            .ToListAsync();

        var recommended = allQuizzes
            .Where(q => !takenQuizIds.Contains(q.Id))
            .Where(q => q.MainTopics.Intersect(strongTopics).Any())
            .OrderByDescending(q => q.Questions.Sum(x => x.Difficulty))
            .Take(5)
            .Select(q => q.ToDto())
            .ToList();

        return recommended;
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
