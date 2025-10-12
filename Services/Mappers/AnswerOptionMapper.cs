using AssessmentSystem.Models;

namespace AssessmentSystem.Services.Mappers;

public static class AnswerOptionMapper
{
    public static AnswerOption ToEntity(this AnswerOptionInputDto dto) =>
        MapCommonFields(dto.Text, dto.IsCorrect);

    public static AnswerOption ToEntity(this AnswerOptionInputAloneDto dto)
    {
        var answerOption = MapCommonFields(dto.Text, dto.IsCorrect);
        answerOption.QuestionId = dto.QuestionId;
        return answerOption;
    }

    public static AnswerOption ToEntity(this AnswerOptionEditDto dto)
    {
        var answerOption = MapCommonFields(dto.Text, dto.IsCorrect);
        answerOption.Id = dto.Id;
        answerOption.QuestionId = dto.QuestionId;
        return answerOption;
    }

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

    private static AnswerOption MapCommonFields(
        string text,
        bool isCorrect
        )
    {
        return new AnswerOption
        {
            Text = text,
            IsCorrect = isCorrect
        };
    }
}