using ShkandalData.Models;
using ShkandalInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xunit;
using ShkandalData.Common;

namespace ShkanadalTests.RepositoryTests
{
    public class ArticleSearchTests : IClassFixture<DatabaseFixture>, IAsyncLifetime
    {
        private readonly DatabaseFixture _fixture;
        private readonly ShkandalDbContext _context;

        public ArticleSearchTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _context = _fixture.CreateContext();
        }

        public async Task InitializeAsync()
        {
            await _context.Articles.ExecuteDeleteAsync();

            var defaultMedia = new Media { Name = "bihus.info", SitemapIndexURL = "https://bihus.info/sitemap_index.xml", IsActive = true };

            _context.Articles.AddRange(
                new Article
                {
                    Title = "Шість годин болю: Історія загибелі 11-річного Артема з Миколаєва, який всю ніч пробув під завалами знищеного росіянами будинку - Bihus.Info",
                    PublishedAt = DateTime.UtcNow.AddHours(-1),
                    IsChecked = true,
                    IsRelevant = true,
                    Media = defaultMedia
                },
                new Article
                {
                    Title = "Пропаганда війни та геноциду: як і за що російських представників влади та пропагандистів можуть притягнути до відповідальності - Bihus.Info",
                    PublishedAt = DateTime.UtcNow.AddHours(-5),
                    IsChecked = false,
                    IsRelevant = true,
                    Media = defaultMedia
                },
                new Article
                {
                    Title = "“Місто під час окупації повернулося у середньовіччя”: історія працівниці онкодиспансеру, яка п’ять місяців прожила у захопленому Херсоні - Bihus.Info",
                    PublishedAt = DateTime.UtcNow,
                    IsChecked = true,
                    IsRelevant = true,
                    Media = defaultMedia
                },
                new Article
                {
                    Title = "Поліція перевіряє в.о. директора ДП \"Рівнеторф\", який змушував підлеглих платити штраф замість себе, – юристи Bihus.Info - Bihus.Info",
                    PublishedAt = DateTime.UtcNow.AddDays(-1),
                    IsChecked = true,
                    IsRelevant = true,
                    Media = defaultMedia
                }
            );
            await _context.SaveChangesAsync();
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task ApplySearch_NullSearchTerm_ReturnsAllSortedByDate()
        {
            string? searchTerm = null;

            var result = await SearchExtensions.ApplySearch(_context.Articles, searchTerm).ToListAsync();

            Assert.Equal(4, result.Count);
            Assert.Equal("“Місто під час окупації повернулося у середньовіччя”: історія працівниці онкодиспансеру, яка п’ять місяців прожила у захопленому Херсоні - Bihus.Info", result[0].Title);
            Assert.Equal("Шість годин болю: Історія загибелі 11-річного Артема з Миколаєва, який всю ніч пробув під завалами знищеного росіянами будинку - Bihus.Info", result[1].Title);
            Assert.Equal("Пропаганда війни та геноциду: як і за що російських представників влади та пропагандистів можуть притягнути до відповідальності - Bihus.Info", result[2].Title);
            Assert.Equal("Поліція перевіряє в.о. директора ДП \"Рівнеторф\", який змушував підлеглих платити штраф замість себе, – юристи Bihus.Info - Bihus.Info", result[3].Title);
        }

        [Fact]
        public async Task ApplySearch_ShortPrefix_ReturnsMatchesStartingWithTerm()
        {
            var searchTerm = "Пол";

            var result = await SearchExtensions.ApplySearch(_context.Articles, searchTerm).ToListAsync();

            Assert.Single(result);
            Assert.Equal("Поліція перевіряє в.о. директора ДП \"Рівнеторф\", який змушував підлеглих платити штраф замість себе, – юристи Bihus.Info - Bihus.Info", result[0].Title);
        }

        [Fact]
        public async Task ApplySearch_TypoInTitle_ReturnsCorrectArticle()
        {
            var searchTerm = "директор Рівноторф";

            var result = await SearchExtensions.ApplySearch(_context.Articles, searchTerm).ToListAsync();

            Assert.Single(result);
            Assert.Equal("Поліція перевіряє в.о. директора ДП \"Рівнеторф\", який змушував підлеглих платити штраф замість себе, – юристи Bihus.Info - Bihus.Info", result[0].Title);
        }

        [Fact]
        public async Task ApplySearch_IrrelevantTerm_ReturnsEmpty()
        {
            var searchTerm = "Криптовалюта";

            var result = await SearchExtensions.ApplySearch(_context.Articles, searchTerm).ToListAsync();

            Assert.Empty(result);
        }
    }
}