namespace AssessmentSystem.Models;

public record ResultInputDto
(
    long QuizId,
    List<Guid> AnswersId
);

public record ResultDto
(
    Guid Id,
    int Score,
    List<Guid> Answers,
    long UserId,
    long QuizId
);