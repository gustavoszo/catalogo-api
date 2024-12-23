namespace CatalogoApi.Dtos
{
    public class PageListResponseDto<T>
    {

        public List<T> Data;
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 5;
        public int TotalCount { get; set; }
        public bool HasNext;
        public bool HasPrevious;

    }
}
