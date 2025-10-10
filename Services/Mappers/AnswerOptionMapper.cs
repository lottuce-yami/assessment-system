using AssessmentSystem.Models;

namespace AssessmentSystem.Services.Mappers;

public static class AnswerOptionMapper
{
    public static AnswerOption ToEntity(this AnswerOptionInputDto dto) => new AnswerOption
    {
        Text = dto.Text,
        IsCorrect = dto.IsCorrect
    };

    public static AnswerOptionDto ToDto(this AnswerOption answerOption) => new AnswerOptionDto
    (
        answerOption.Id,
        answerOption.Text,
        answerOption.QuestionId
    );

    public static AnswerOptionAdminDto ToAdminDto(this AnswerOption answerOption) => new AnswerOptionAdminDto
    (
        answerOption.Id,
        answerOption.Text,
        answerOption.IsCorrect,
        answerOption.QuestionId,
        [.. answerOption.Answers.Select(a => a.Id)]
    );
}