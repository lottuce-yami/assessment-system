namespace AssessmentSystem.Models;

public class Quiz
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public int MaxScore { get; set; }

    public required List<Question> Questions { get; set; }

    public required List<Result> Results { get; set; }
}