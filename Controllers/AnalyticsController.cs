using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssessmentSystem.Data;
using Microsoft.AspNetCore.Authorization;

namespace AssessmentSystem.Controllers;

[Authorize(Roles = "Admin")]
[Route("api/[controller]")]
[ApiController]
public class AnalyticsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public AnalyticsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: api/Analytics/global-knowledge
    [HttpGet("global-knowledge")]
    public async Task<IActionResult> GetGlobalKnowledgeProfile()
    {
        var stats = await _context.ResultTopicStats
            .GroupBy(x => x.Topic)
            .Select(g => new 
            {
                Topic = g.Key,
                AverageAccuracy = g.Sum(x => x.TotalCount) == 0 ? 0 : 
                    (int)((double)g.Sum(x => x.CorrectCount) / g.Sum(x => x.TotalCount) * 100),
                
                TotalQuestionsAnswered = g.Sum(x => x.TotalCount)
            })
            .OrderByDescending(x => x.AverageAccuracy)
            .ToListAsync();

        return Ok(stats);
    }

    // GET: api/Analytics/activity-trend
    [HttpGet("activity-trend")]
    public async Task<IActionResult> GetActivityTrend()
    {
        var today = DateTime.UtcNow.Date;
        var sevenDaysAgo = today.AddDays(-6); 

        var rawData = await _context.Result
            .Where(r => r.FinishedAt.HasValue && r.FinishedAt.Value.Date >= sevenDaysAgo)
            .GroupBy(r => r.FinishedAt.Value.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToListAsync();

        var values = new List<int>();
        var labels = new List<DateTime>();

        for (var day = sevenDaysAgo; day <= today; day = day.AddDays(1))
        {
            var entry = rawData.FirstOrDefault(d => d.Date == day);
            values.Add(entry?.Count ?? 0);
            labels.Add(DateTime.SpecifyKind(day, DateTimeKind.Utc));
        }

        return Ok(new { Values = values, Labels = labels });
    }
}
