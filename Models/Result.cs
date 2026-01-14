namespace AssessmentSystem.Models;

public class Result
{
    public Guid Id { get; set; }
    public int Score { get; set; }

    public DateTimeOffset StartedAt { get; set; }

    public DateTimeOffset? FinishedAt { get; set; }

    public List<Answer> Answers { get; set; } = [];

    public long UserId { get; set; }
    public User User { get; set; } = null!;
    
    public long QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
}