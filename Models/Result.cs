namespace AssessmentSystem.Models;

public class Result
{
    public Guid Id { get; set; }
    public int Score { get; set; }

    public required List<Answer> Answers { get; set; }

    public long UserId { get; set; }
    public required User User { get; set; }
    
    public long QuizId { get; set; }
    public required Quiz Quiz { get; set; }
}