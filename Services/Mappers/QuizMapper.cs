using AssessmentSystem.Models;

namespace AssessmentSystem.Services.Mappers;

public static class QuizMapper
{
    public static Quiz ToEntity(this QuizInputDto dto) => new Quiz
    {
        Title = dto.Title,
        MaxScore = dto.Questions.Aggregate(0, (total, next) => total + next.Difficulty),
        Questions = [.. dto.Questions.Select(q => q.ToEntity())]
    };

    public static QuizDto ToDto(this Quiz quiz) => new QuizDto(
        quiz.Id,
        quiz.Title,
        quiz.MaxScore,
        [.. quiz.Questions.Select(q => q.Id)],
        [.. quiz.Results.Select(r => r.Id)]
    );
}