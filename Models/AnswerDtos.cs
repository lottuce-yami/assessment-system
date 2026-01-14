namespace AssessmentSystem.Models;

public record AnswerInputDto
(
    Guid ResultId,
    Guid QuestionId,
    List<Guid> SelectedOptions
);

public record AnswerDto
(
    Guid Id,
    DateTime AnsweredAt,
    List<Guid> SelectedOptions,
    Guid QuestionId,
    Guid ResultId
);