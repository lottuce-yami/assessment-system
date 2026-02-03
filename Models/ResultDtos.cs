namespace AssessmentSystem.Models;

public record ResultInputDto
(
    long QuizId
);

public record ResultDto
(
    Guid Id,
    int Score,
    List<Guid> Answers,
    long UserId,
    long QuizId,
    DateTime StartedAt,
    DateTime? FinishedAt,
    int? CompletionTime,
    List<ResultTopicStatDto> TopicStats
);
