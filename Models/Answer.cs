namespace AssessmentSystem.Models;

public class Answer
{
    public Guid Id { get; set; }
    public DateTimeOffset AnsweredAt { get; set; }

    public List<AnswerOption> SelectedOptions { get; set; } = [];

    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public Guid ResultId { get; set; }
    public Result Result { get; set; } = null!;
}