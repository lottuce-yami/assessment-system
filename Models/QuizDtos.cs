namespace AssessmentSystem.Models;

public record QuizInputDto
(
    string Title,
    List<QuestionInputDto> Questions
);

public record QuizEditDto
(
    long Id,
    string Title,
    List<QuestionInputDto> Questions
);

public record QuizDto
(
    long Id,
    string Title,
    int MaxScore,
    List<Guid> Questions,
    List<Guid> Results
);