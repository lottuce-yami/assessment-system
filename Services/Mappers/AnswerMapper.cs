using AssessmentSystem.Models;

namespace AssessmentSystem.Services.Mappers;

public static class AnswerMapper
{
    public static Answer ToEntity(this AnswerInputDto dto) => new Answer {
        ResultId = dto.ResultId,
        SelectedOptions = [.. dto.SelectedOptions.Select(id => new AnswerOption { Id = id })]
    };

    public static AnswerDto ToDto(this Answer answer) => new AnswerDto(
        answer.Id,
        answer.AnsweredAt,
        [.. answer.SelectedOptions.Select(o => o.Id)],
        answer.QuestionId,
        answer.ResultId
    );
}