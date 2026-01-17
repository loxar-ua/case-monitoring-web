using ShkandalData.Models;

namespace ShkandalData.Common
{
    public static class ClusterExtensions
    {
        public static IQueryable<Cluster> ApplyCategoryFilter(this IQueryable<Cluster> query, int? categoryId)
        {
            if (categoryId == null || categoryId == 0)
                return query;

            return query.Where(c => c.Categories.Any(ca => ca.Id == categoryId));
        }

        public static IQueryable<Cluster> ApplySorting(this IQueryable<Cluster> query, string? sortBy)
        {
            return sortBy?.ToLower() switch
            {
                "popular" => query.OrderByDescending(c => c.ViewCounter)
                                 .ThenByDescending(c => c.LastUpdatedAt),

                "unpopular" => query.OrderBy(c => c.ViewCounter)
                                .ThenByDescending(c => c.LastUpdatedAt),

                "newest" => query.OrderByDescending(c => c.LastUpdatedAt),

                "oldest" => query.OrderBy(c => c.LastUpdatedAt),

                _ => query.OrderByDescending(c => c.ViewCounter)
            };
        }
    }
}