namespace AssessmentSystem.Models;

public record QuizInputDto
(
    string Title,
    List<QuestionInputDto> Questions,
    string Language = "uk"
);

public record QuizEditDto
(
    long Id,
    string Title,
    List<QuestionEditDto> Questions,
    string Language
);

public record QuizDto
(
    long Id,
    string Title,
    int MaxScore,
    List<Guid> Questions,
    List<Guid> Results,
    List<string> MainTopics,
    string Language
);

public record QuizTreeDto
(
    long Id,
    string Title,
    int MaxScore,
    List<QuestionTreeDto> Questions,
    List<Guid> Results,
    List<string> MainTopics,
    string Language
);
