// Updated PaginationService.cs
using AssetManagement.Application.Paginations;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace AssetManagement.Application.Services
{
    public static class PaginationService
    {
        public static async Task<PagedList<T>> ToPagedList<T>(IQueryable<T> query, int pageNumber, int pageSize)
        {
            int count;
            List<T> items;

            try
            {
                // Try using EF Core async methods
                count = await query.CountAsync();
                items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
            }
            catch (InvalidOperationException)
            {
                // Fallback to synchronous operations for in-memory collections
                count = query.Count();
                items = query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            }

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
