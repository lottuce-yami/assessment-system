namespace AssessmentSystem.Models;

public class User
{
    public long Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
    public int Age { get; set; }
    public required List<string> Interests { get; set; }
    
    public required List<Result> Results { get; set; }
}
