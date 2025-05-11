namespace AssessmentSystem.Models;

public record ResultInputDto
(
    long QuizId,
    List<Guid> AnswersId
);