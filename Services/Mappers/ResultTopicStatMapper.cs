using AssessmentSystem.Models;

namespace AssessmentSystem.Services.Mappers;

public static class ResultTopicStatMapper
{
    public static ResultTopicStatDto ToDto(this ResultTopicStat topicStat) => new ResultTopicStatDto
    (
        topicStat.Id,
        topicStat.Topic,
        topicStat.CorrectCount,
        topicStat.TotalCount,
        topicStat.Percentage
    );
}
