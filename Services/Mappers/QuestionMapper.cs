using AssessmentSystem.Models;

namespace AssessmentSystem.Services.Mappers;

public static class QuestionMapper
{
    public static Question ToEntity(this QuestionInputDto dto) => new Question
    {
        Text = dto.Text,
        Topics = dto.Topics,
        Difficulty = dto.Difficulty,
        AnswerOptions = [.. dto.AnswerOptions.Select(ao => ao.ToEntity())]
    };

    public static QuestionDto ToDto(this Question question) => new QuestionDto
    (
        question.Id,
        question.Text,
        question.Topics,
        question.Difficulty,
        [.. question.AnswerOptions.Select(ao => ao.Id)],
        question.QuizId
    );
}