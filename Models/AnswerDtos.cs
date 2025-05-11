namespace AssessmentSystem.Models;

public record AnswerInputDto
(
    Guid ResultId,
    List<Guid> SelectedOptions
);