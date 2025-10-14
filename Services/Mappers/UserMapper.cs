using AssessmentSystem.Models;
using Microsoft.AspNetCore.Identity;

namespace AssessmentSystem.Services.Mappers;

public static class UserMapper
{
    public static User ToEntity(this UserInputDto dto) =>
        MapCommonFields(dto.Username, dto.Age, dto.Interests);
    
    public static User ToEntity(this UserEditDto dto)
    {
        var user = MapCommonFields(dto.Username, dto.Age, dto.Interests);
        user.Id = dto.Id;
        return user;
    }

    public static UserDto ToDto(this User user) => new UserDto(
        user.Id,
        user.Username,
        user.Age,
        user.Interests,
        [.. user.Results.Select(r => r.Id)]
    );
    
    private static User MapCommonFields(string username, int age, List<string> interests)
    {
        return new User
        {
            Username = username,
            Age = age,
            Interests = interests
        };
    }
}