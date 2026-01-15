namespace AssessmentSystem.Models;

public class Quiz
{
    public long Id { get; set; }
    public required string Title { get; set; }
    public int MaxScore { get; set; }

    public List<Question> Questions { get; set; } = [];

    public List<Result> Results { get; set; } = [];

    public List<string> MainTopics { get; set; } = [];

    public void UpdateMetadata()
    {
        MaxScore = Questions.Aggregate(0, (total, next) => total + next.Difficulty);

        MainTopics = Questions
            .SelectMany(q => q.Topics)
            .GroupBy(t => t)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .Select(g => g.Key)
            .ToList();
    }
}