using AssessmentSystem.Models;

namespace AssessmentSystem.Services.Mappers;

public static class QuizMapper
{
    public static Quiz ToEntity(this QuizInputDto dto)
    {
        var quiz = new Quiz
        {
            Title = dto.Title,
            Questions = [.. dto.Questions.Select(q => q.ToEntity())]
        };

        quiz.CalculateMaxScore();

        return quiz;
    }
    
    public static Quiz ToEntity(this QuizEditDto dto)
    {
        var quiz = new Quiz
        {
            Id = dto.Id,
            Title = dto.Title
        };

        return quiz;
    }

    public static QuizDto ToDto(this Quiz quiz) => new QuizDto(
        quiz.Id,
        quiz.Title,
        quiz.MaxScore,
        [.. quiz.Questions.Select(q => q.Id)],
        [.. quiz.Results.Select(r => r.Id)]
    );
}