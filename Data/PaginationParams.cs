namespace AssessmentSystem.Data;

public class PaginationParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public const int MaxPageSize = 100;

    public void Normalize()
    {
        if (PageSize > MaxPageSize)
            PageSize = MaxPageSize;

        if (PageNumber < 1)
            PageNumber = 1;
    }
}
