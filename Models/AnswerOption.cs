namespace AssessmentSystem.Models;

public class AnswerOption
{
    public Guid Id { get; set; }
    public required string Text { get; set; }
    public bool IsCorrect { get; set; }
    
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public List<Answer> Answers { get; set; } = null!;
}