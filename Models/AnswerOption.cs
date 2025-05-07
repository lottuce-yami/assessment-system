namespace AssessmentSystem.Models;

public class AnswerOption
{
    public Guid Id { get; set; }
    public required string Text { get; set; }
    public bool IsCorrect { get; set; }
    
    public Guid QuestionId { get; set; }
    public required Question Question { get; set; }
}