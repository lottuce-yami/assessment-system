using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssessmentSystem.Data;
using AssessmentSystem.Models;

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
    [HttpPost]
    public async Task<ActionResult<Result>> PostResult(Result result)
    {
        _context.Result.Add(result);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetResult", new { id = result.Id }, result);
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
