namespace AssessmentSystem.Models;

public record AnswerOptionInputDto
(
    string Text,
    bool IsCorrect
);

public record AnswerOptionInputAloneDto
(
    string Text,
    bool IsCorrect,
    Guid QuestionId
);

public record AnswerOptionEditDto
(
    Guid Id,
    string Text,
    bool IsCorrect
);

public record AnswerOptionDto
(
    Guid Id,
    string Text,
    Guid QuestionId
);

public record AnswerOptionAdminDto
(
    Guid Id,
    string Text,
    bool IsCorrect,
    Guid QuestionId,
    List<Guid> AnswersId
) : AnswerOptionDto(Id, Text, QuestionId);

public record AnswerOptionTreeAdminDto
(
    Guid Id,
    string Text,
    bool IsCorrect,
    List<Guid> AnswerIds
);
