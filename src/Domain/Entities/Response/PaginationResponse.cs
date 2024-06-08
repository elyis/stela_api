namespace stela_api.src.Domain.Entities.Response
{
    public class PaginationResponse<T>
    {
        public int Count { get; set; }
        public int Offset { get; set; }
        public long Total { get; set; }

        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    }
}