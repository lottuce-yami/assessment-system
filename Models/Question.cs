namespace AssessmentSystem.Models;

public class Question
{
    public Guid Id { get;set; }
    public required string Text { get; set; }
    public required List<string> Topics { get; set; }
    public int Difficulty { get; set; }

    public required List<AnswerOption> AnswerOptions { get; set; }

    public long QuizId { get; set; }
    public required Quiz Quiz { get; set; }
}