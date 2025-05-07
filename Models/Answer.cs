namespace AssessmentSystem.Models;

public class Answer
{
    public Guid Id { get; set; }
    public DateTimeOffset AnsweredAt { get; set; }

    public required List<AnswerOption> SelectedOptions { get; set; }

    public Guid QuestionId { get; set; }
    public required Question Question { get; set; }

    public Guid ResultId { get; set; }
    public required Result Result { get; set; }
}