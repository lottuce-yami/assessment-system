using System.Security.Claims;

namespace AssessmentSystem.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static long? GetId(this ClaimsPrincipal user)
    {
        if (long.TryParse(user.FindFirstValue(ClaimTypes.NameIdentifier), out var id))
            return id;
        return null;
    }

    public static bool IsAdmin(this ClaimsPrincipal user)
    {
        if (user.FindFirstValue(ClaimTypes.Role) == "Admin")
            return true;
        else
            return false;
    }
}