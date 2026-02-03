namespace AssessmentSystem.Models;

public class ResultTopicStat
{
    public Guid Id { get; set; }

    public required string Topic { get; set; }

    public int CorrectCount { get; set; }

    public int TotalCount { get; set; }
    
    public int Percentage => TotalCount == 0 ? 0 : (int)((double)CorrectCount / TotalCount * 100);

    public Guid ResultId { get; set; }
    public Result Result { get; set; } = null!;
}
