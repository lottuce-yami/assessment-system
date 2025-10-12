using AssessmentSystem.Models;

namespace AssessmentSystem.Services.Mappers;

public static class QuestionMapper
{
    public static Question ToEntity(this QuestionInputDto dto) =>
        MapCommonFields(dto.Text, dto.Topics, dto.Difficulty, dto.AnswerOptions);

    public static Question ToEntity(this QuestionInputAloneDto dto)
    {
        var question = MapCommonFields(dto.Text, dto.Topics, dto.Difficulty, dto.AnswerOptions);
        question.QuizId = dto.QuizId;
        return question;
    }

    public static Question ToEntity(this QuestionEditDto dto)
    {
        var question = MapCommonFields(dto.Text, dto.Topics, dto.Difficulty, dto.AnswerOptions);
        question.Id = dto.Id;
        question.QuizId = dto.QuizId;
        return question;
    }

    public static QuestionDto ToDto(this Question question) => new QuestionDto
    (
        question.Id,
        question.Text,
        question.Topics,
        question.Difficulty,
        [.. question.AnswerOptions.Select(ao => ao.Id)],
        question.QuizId
    );

    private static Question MapCommonFields(
        string text,
        List<string> topics,
        int difficulty,
        List<AnswerOptionInputDto> answerOptions
        )
    {
        return new Question
        {
            Text = text,
            Topics = topics,
            Difficulty = difficulty,
            AnswerOptions = [.. answerOptions.Select(ao => ao.ToEntity())]
        };
    }
}