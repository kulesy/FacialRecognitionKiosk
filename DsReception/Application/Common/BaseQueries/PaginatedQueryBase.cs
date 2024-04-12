namespace DsReceptionAPI.Application.Common.BaseQueries
{
    public abstract record PaginatedQueryBase
    {
        public int PageSize { get; init; } = 100;

        public int PageNumber { get; init; } = 1;

        public string Term { get; init; }
    }
}
