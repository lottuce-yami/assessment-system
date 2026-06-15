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
public class ResultController(ApplicationDbContext context) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;

    // GET: api/Result
    [HttpGet]
    public async Task<ActionResult<PagedResult<ResultDto>>> GetResult([FromQuery] PaginationParams pagination)
    {
        if (User.IsAdmin())
        {
            return await _context.Result
                .Include(r => r.Answers)
                .Include(r => r.User)
                .ToPagedResultAsync(pagination, r => r.ToDto());
        }
        
        return await _context.Result
            .Where(r => r.UserId == User.GetId())
            .Include(r => r.Answers)
            .Include(r => r.User)
            .OrderByDescending(r => r.FinishedAt)
            .ToPagedResultAsync(pagination, r => r.ToDto());
    }

    // GET: api/Result/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ResultDto>> GetResult(Guid id)
    {
        var result = await _context.Result
            .Where(r => r.Id == id)
            .Include(r => r.Answers)
            .Include(r => r.TopicStats)
            .Include(r => r.User)
            .FirstOrDefaultAsync();

        if (result == null)
        {
            return NotFound();
        }

        if (User.GetId() != result.UserId && !User.IsAdmin())
        {
            return Forbid();
        }

        return result.ToDto();
    }

    // POST: api/Result
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<ResultDto>> PostResult(ResultInputDto dto)
    {
        var currentTime = DateTime.UtcNow;
        
        var result = dto.ToEntity();
        result.UserId = (long)User.GetId()!;
        result.StartedAt = currentTime;

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

        if (User.GetId() != result.UserId || !User.IsAdmin())
        {
            return Forbid();
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
