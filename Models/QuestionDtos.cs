namespace AssessmentSystem.Models;

public record QuestionInputDto
(
    string Text,
    List<string> Topics,
    int Difficulty,
    List<AnswerOptionInputDto> AnswerOptions
);

public record QuestionInputAloneDto(
    string Text,
    List<string> Topics,
    int Difficulty,
    List<AnswerOptionInputDto> AnswerOptions,
    long QuizId
);

public record QuestionEditDto
(
    Guid? Id,
    string Text,
    List<string> Topics,
    int Difficulty,
    List<AnswerOptionInputDto> AnswerOptions
);

public record QuestionDto
(
    Guid Id,
    string Text,
    List<string> Topics,
    int Difficulty,
    List<Guid> AnswerOptionsId,
    long QuizId
);