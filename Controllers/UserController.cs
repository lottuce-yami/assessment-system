using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AssessmentSystem.Data;
using AssessmentSystem.Models;
using AssessmentSystem.Services.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using AssessmentSystem.Extensions;

namespace AssessmentSystem.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UserController(ApplicationDbContext context, IPasswordHasher<User> passwordHasher) : ControllerBase
{
    private readonly ApplicationDbContext _context = context;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    // GET: api/User
    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<PagedResult<UserDto>>> GetUsers([FromQuery] PaginationParams pagination)
    {
        return await _context.Users
            .Include(u => u.Results)
            .ToPagedResultAsync(pagination, u => u.ToDto());
    }

    // GET: api/User/5
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUser(long id)
    {
        if (User.GetId() != id && !User.IsAdmin())
        {
            return Forbid();
        }

        var user = await _context.Users
            .Where(u => u.Id == id)
            .Include(u => u.Results)
            .FirstOrDefaultAsync();

        if (user == null)
        {
            return NotFound();
        }

        return user.ToDto();
    }

    // PUT: api/User/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(long id, UserEditDto userDto)
    {
        if (id != userDto.Id)
        {
            return BadRequest();
        }

        if (User.GetId() != id && !User.IsAdmin())
        {
            return Forbid();
        }

        var user = userDto.ToEntity();

        _context.Entry(user).State = EntityState.Modified;
        _context.Entry(user).Property(u => u.PasswordHash).IsModified = false;
        _context.Entry(user).Collection(u => u.Results).IsModified = false;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UserExists(id))
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

    // POST: api/User
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<User>> PostUser(UserInputDto userDto)
    {
        var user = userDto.ToEntity();

        user.PasswordHash = _passwordHasher.HashPassword(user, userDto.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetUser", new { id = user.Id }, user.ToDto());
    }

    // DELETE: api/User/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        if (User.GetId() != id && !User.IsAdmin())
        {
            return Forbid();
        }

        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UserExists(long id)
    {
        return _context.Users.Any(e => e.Id == id);
    }
}
