using Microsoft.EntityFrameworkCore;
using ShkandalData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShkandalData.Common
{
    public static class SearchExtensions
    {
        public static IQueryable<Cluster> ApplySearch(IQueryable<Cluster> query, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return query.OrderByDescending(c => c.ViewCounter);
            }

            if (searchTerm.Length < Constants.TrigramThresholdLength)
            {
                return query
                    .Where(c => EF.Functions.ILike(c.Name, $"%{searchTerm}%"))
                    .OrderByDescending(c => c.ViewCounter);
            }
            return query
                .Where(c => EF.Functions.TrigramsWordSimilarity(searchTerm, c.Name) > Constants.TrigramAccuracyThreshold)
                .OrderByDescending(c => EF.Functions.TrigramsWordSimilarity(searchTerm, c.Name))
                .ThenByDescending(c => c.ViewCounter);
        }

        public static IQueryable<Article> ApplySearch(IQueryable<Article> query, string? searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return query.OrderByDescending(a => a.PublishedAt);
            }

            if (searchTerm.Length < Constants.TrigramThresholdLength)
            {
                return query
                    .Where(a => EF.Functions.ILike(a.Title, $"%{searchTerm}%"))
                    .OrderByDescending(a => a.PublishedAt);
            }

            return query
                .Where(a => EF.Functions.TrigramsWordSimilarity(searchTerm, a.Title) > Constants.TrigramAccuracyThreshold)
                .OrderByDescending(a => EF.Functions.TrigramsWordSimilarity(searchTerm, a.Title))
                .ThenByDescending(a => a.PublishedAt);
        }
    }
}
