using System.ComponentModel.DataAnnotations;

namespace AssessmentSystem.Models;

public record UserInputDto
(
    [StringLength(maximumLength: 16, MinimumLength = 3)]
    [RegularExpression("^[a-z][-a-z0-9_]*")]
        string Username,
    string Password,
    int Age,
    List<string> Interests
);

public record UserDto
(
    long Id,
    string Username,
    int Age,
    List<string> Interests,
    List<Guid> Results
);

public record UserLoginDto
(
    string Username,
    string Password
);