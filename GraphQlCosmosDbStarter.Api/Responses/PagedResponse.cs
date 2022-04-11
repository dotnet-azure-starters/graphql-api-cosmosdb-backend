namespace GraphQlCosmosDbStarter.Api.Responses
{
    public class PagedResponse<T> : BaseResponse, IPagedResponse<T>
    {
        public IEnumerable<T> Items { get; set; }

        public bool HasNextPage { get; set; }

        public bool HasPreviousPage { get; set; }

        public long TotalCount { get; set; }

        public long ItemCount { get; set; }
    }

}
