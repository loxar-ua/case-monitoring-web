using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShkandalData.Common.Constants;

namespace ShkandalData.Common
{
    public class PagedList<T> 
    {
        private PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            CurrentPage = pageNumber;
            TotalCount = count;
            PageSize = pageSize;
        }
        public List<T> Items { get; set; }

        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
        public bool IsPreviousPageExists => CurrentPage > 1;
        public bool IsNextPageExists => CurrentPage < TotalPages;

        public static async  Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
        {
            if(pageNumber < 1) 
            {
                pageNumber = 1;
            }

            if (pageSize < 1)
            {
                pageSize = DefaultPageSize;
            }
            else if (pageSize > MaxPageSize)
            {
                pageSize = MaxPageSize;
            }

            var count = await source
                .CountAsync();

            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
        
        public PagedList<U> Select<U>(Func<T, U> selector)
        {
            var mappedItems = Items.Select(selector).ToList();
            return new PagedList<U>(mappedItems, TotalCount, CurrentPage, PageSize);
        }
    }
}
