using AssessmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AssessmentSystem.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}