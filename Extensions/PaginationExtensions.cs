using System.Linq.Expressions;
using AssessmentSystem.Data;
using Microsoft.EntityFrameworkCore;

namespace AssessmentSystem.Extensions;

public static class PaginationExtensions
{
    public static async Task<PagedResult<TResult>> ToPagedResultAsync<T, TResult> (
        this IQueryable<T> query,
        PaginationParams pagination,
        Expression<Func<T, TResult>> selector
    )
    {
        pagination.Normalize();

        var total = await query.CountAsync();
        var items = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(selector)
            .ToListAsync();

        return new PagedResult<TResult>(items, total, pagination.PageNumber, pagination.PageSize);
    }

    public static PagedResult<TResult> ToPagedResult<T, TResult> (
        this IEnumerable<T> enumerable,
        PaginationParams pagination,
        Func<T, TResult> selector
    )
    {
        pagination.Normalize();
        
        var total = enumerable.Count();
        var items = enumerable
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .Select(selector)
            .ToList();

        return new PagedResult<TResult>(items, total, pagination.PageNumber, pagination.PageSize);
    }
}
