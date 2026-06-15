using AssessmentSystem.Models;

namespace AssessmentSystem.Services.Mappers;

public static class ResultMapper
{
    public static Result ToEntity(this ResultInputDto dto) => new Result {
        QuizId = dto.QuizId
    };

    public static ResultDto ToDto(this Result result) => new ResultDto (
        result.Id,
        result.Score,
        [.. result.Answers.Select(a => a.Id)],
        result.UserId,
        result.QuizId,
        DateTime.SpecifyKind(result.StartedAt, DateTimeKind.Utc),
        result.FinishedAt.HasValue 
            ? DateTime.SpecifyKind(result.FinishedAt.Value, DateTimeKind.Utc) 
            : null,
        result.FinishedAt.HasValue 
            ? (int)(result.FinishedAt.Value - result.StartedAt).TotalSeconds 
            : null,
        result.TopicStats.Select(ts => ts.ToDto())
            .OrderByDescending(t => t.Percentage).ToList(),
        result.User.Username
    );
}