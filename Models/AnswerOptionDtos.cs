namespace AssessmentSystem.Models;

public record AnswerOptionInputDto
(
    string Text,
    bool IsCorrect
);

public record AnswerOptionDto
(
    Guid Id,
    string Text,
    Guid QuestiondId
);

public record AnswerOptionAdminDto
(
    Guid Id,
    string Text,
    bool IsCorrect,
    Guid QuestionId,
    List<Guid> AnswersId
);