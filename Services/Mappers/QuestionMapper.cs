using AssessmentSystem.Models;

namespace AssessmentSystem.Services.Mappers;

public static class QuestionMapper
{
    public static Question ToEntity(this QuestionInputDto dto)
    {
        var question = MapCommonFields(dto.Text, dto.Topics, dto.Difficulty);
        question.AnswerOptions = [.. dto.AnswerOptions.Select(ao => ao.ToEntity())];

        return question;
    }
        
    public static Question ToEntity(this QuestionInputAloneDto dto)
    {
        var question = MapCommonFields(dto.Text, dto.Topics, dto.Difficulty);
        question.AnswerOptions = [.. dto.AnswerOptions.Select(ao => ao.ToEntity())];
        question.QuizId = dto.QuizId;

        return question;
    }

    public static Question ToEntity(this QuestionEditDto dto)
    {
        var question = MapCommonFields(dto.Text, dto.Topics, dto.Difficulty);
        question.Id = dto.Id;
        
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

    private static Question MapCommonFields(string text, List<string> topics, int difficulty)
    {
        return new Question
        {
            Text = text,
            Topics = topics,
            Difficulty = difficulty
        };
    }
}