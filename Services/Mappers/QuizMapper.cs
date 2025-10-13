using AssessmentSystem.Models;

namespace AssessmentSystem.Services.Mappers;

public static class QuizMapper
{
    public static Quiz ToEntity(this QuizInputDto dto)
    {
        var quiz = MapCommonFields(dto.Title, dto.Questions);

        quiz.CalculateMaxScore();

        return quiz;
    }
    
    public static Quiz ToEntity(this QuizEditDto dto)
    {
        var quiz = MapCommonFields(dto.Title, dto.Questions);
        quiz.Id = dto.Id;

        quiz.CalculateMaxScore();

        return quiz;
    }

    public static QuizDto ToDto(this Quiz quiz) => new QuizDto(
        quiz.Id,
        quiz.Title,
        quiz.MaxScore,
        [.. quiz.Questions.Select(q => q.Id)],
        [.. quiz.Results.Select(r => r.Id)]
    );

    private static Quiz MapCommonFields(string title, List<QuestionInputDto> questions)
    {
        return new Quiz
        {
            Title = title,
            Questions = [.. questions.Select(q => q.ToEntity())]
        };
    }
}