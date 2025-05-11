namespace AssessmentSystem.Models;

public class Question
{
    public Guid Id { get;set; }
    public required string Text { get; set; }
    public List<string> Topics { get; set; } = [];
    public required int Difficulty { get; set; }

    public List<AnswerOption> AnswerOptions { get; set; } = [];

    public long QuizId { get; set; }
    public Quiz Quiz { get; set; } = null!;
}