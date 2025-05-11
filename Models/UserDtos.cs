namespace AssessmentSystem.Models;

public record UserInputDto
(
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