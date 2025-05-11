namespace AssessmentSystem.Models;

public record QuestionInputDto
(
    string Text,
    List<string> Topics,
    int Difficulty,
    List<AnswerOptionInputDto> AnswerOptions
);