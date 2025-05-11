using AssessmentSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace AssessmentSystem.Services.Mappers;

public static class UserMapper
{
    public static User ToEntity(this UserInputDto dto) => new User {
        Username = dto.Username,
        Age = dto.Age,
        Interests = dto.Interests
    };

    public static UserDto ToDto(this User user) => new UserDto (
        user.Id,
        user.Username,
        user.Age,
        user.Interests,
        [.. user.Results.Select(r => r.Id)]
    );
}