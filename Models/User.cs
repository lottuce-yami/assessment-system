namespace AssessmentSystem.Models;

public class User
{
    public long Id { get; set; }
    public required string Username { get; set; }
    public string PasswordHash { get; set; } = null!;
    public required int Age { get; set; }
    public List<string> Interests { get; set; } = [];
    
    public List<Result> Results { get; set; } = [];
}
