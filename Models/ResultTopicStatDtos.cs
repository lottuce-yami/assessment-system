namespace AssessmentSystem.Models;

public record ResultTopicStatDto
(
    Guid Id,
    string Topic,
    int CorrectCount,
    int TotalCount,
    int Percentage
);
