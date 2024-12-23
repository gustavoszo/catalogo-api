using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using CatalogoApi.Dtos;

namespace CatalogoApi.Pagination
{
    public class PageList<T> : List<T>
    {

        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 5;
        public int TotalCount { get; set; }

        public bool HasNext;
        public bool HasPrevious;

        public PageList(List<T> data, int pageNumber)
        {
            TotalCount = data.Count();
            TotalPages = (int)Math.Ceiling(data.Count() / (double)PageSize);
            CurrentPage = pageNumber;

            var hasPrevious = pageNumber > 0;
            var hasNext = pageNumber < TotalPages - 1;

            AddRange(data);
        }

        public static PageList<T> ToPagedList(IEnumerable<T> data, int pageNumber)
        { 
            return new PageList<T>(data.ToList(), pageNumber);
        }

        public PageListResponseDto<T> ToPageListResponse()
        {
            return new PageListResponseDto<T>()
            {
                Data = this.ToList(),
                TotalPages = TotalPages,
                CurrentPage = CurrentPage,
                TotalCount = TotalCount,
                PageSize = PageSize,
                HasPrevious = HasPrevious,
                HasNext = HasNext
            };
        }

    }
}
