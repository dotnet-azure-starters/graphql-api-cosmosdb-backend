namespace GraphQlCosmosDbStarter.Api.Responses
{

    public interface IPagedResponse<T>
    {
        // TODO: transfer to a separate file
        IEnumerable<T> Items { get; set; }

        bool HasNextPage { get; set; }

        bool HasPreviousPage { get; set; }

        long TotalCount { get; set; }

        long ItemCount { get; set; }

    }
}
