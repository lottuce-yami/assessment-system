using AssessmentSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace AssessmentSystem.Extensions;

public static class PaginationExtensions
{
    public static async Task<PagedResult<T>> ToPagedResultAsync<T> (
        this IQueryable<T> query,
        PaginationParams pagination
    )
    {
        pagination.Normalize();

        var total = await query.CountAsync();
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        return new PagedResult<T>(items, total, pagination.PageNumber, pagination.PageSize);
    }

    public static PagedResult<T> ToPagedResult<T> (
        this IEnumerable<T> enumerable,
        PaginationParams pagination
    )
    {
        pagination.Normalize();
        
        var total = enumerable.Count();
        var items = enumerable
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToList();

        return new PagedResult<T>(items, total, pagination.PageNumber, pagination.PageSize);
    }
}
