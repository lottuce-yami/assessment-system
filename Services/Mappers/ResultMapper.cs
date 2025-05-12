using AssessmentSystem.Models;

namespace AssessmentSystem.Services.Mappers;

public static class ResultMapper
{
    public static Result ToEntity(this ResultInputDto dto) => new Result {
        QuizId = dto.QuizId,
        Answers = [.. dto.AnswersId.Select(id => new Answer { Id = id })]
    };

    public static ResultDto ToDto(this Result result) => new ResultDto (
        result.Id,
        result.Score,
        [.. result.Answers.Select(a => a.Id)],
        result.UserId,
        result.QuizId
    );
}