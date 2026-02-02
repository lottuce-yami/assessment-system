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
    List<QuestionEditDto> Questions
);

public record QuizDto
(
    long Id,
    string Title,
    int MaxScore,
    List<Guid> Questions,
    List<Guid> Results
);

public record QuizTreeDto
(
    long Id,
    string Title,
    int MaxScore,
    List<QuestionTreeDto> Questions,
    List<Guid> Results
);
