using AssessmentSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace AssessmentSystem.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }

    public DbSet<Quiz> Quiz { get; set; }

    public DbSet<Answer> Answer { get; set; }

    public DbSet<AnswerOption> AnswerOption { get; set; }

    public DbSet<Question> Question { get; set; }

    public DbSet<Result> Result { get; set; }
}