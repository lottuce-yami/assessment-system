namespace AssessmentSystem.Models;

public class Quiz
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public int MaxScore { get; set; }

    public List<Question> Questions { get; set; } = [];

    public List<Result> Results { get; set; } = [];
}