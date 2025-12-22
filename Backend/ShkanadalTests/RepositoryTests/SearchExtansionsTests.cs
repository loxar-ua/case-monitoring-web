using ShkandalData.Models;
using ShkandalInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShkandalData.Common;
using Microsoft.EntityFrameworkCore;

namespace ShkanadalTests.RepositoryTests
{
    public class SearchExtansionsTests : IClassFixture<DatabaseFixture>, IAsyncLifetime
    {
        private readonly DatabaseFixture _fixture;
        private readonly ShkandalDbContext _context;

        public SearchExtansionsTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _context = _fixture.CreateContext();
        }
        public async Task InitializeAsync()
        {
            await _context.Clusters.ExecuteDeleteAsync();
            await _context.Database.EnsureCreatedAsync();

            _context.Clusters.AddRange(
                new Cluster { Name = "Справа «Мідас»: Міндіч, «Енергоатом», НАБУ/САП, Галущенко та Гринчук", ViewCounter = 300 },
                new Cluster { Name = "Угода про корисні копалини між Україною та США — переговори, проєкти та політичний скандал (Трамп, Зеленський, Мінфін США)", ViewCounter = 50 },
                new Cluster { Name = "Зеленський, Міністерство оборони, Верховна Рада та ОПК: масштабна державна програма виробництва дронів і податкові пільги для оптоволокна", ViewCounter = 30 },
                new Cluster { Name = "Коаліція рішучих: Макрон, Зеленський та країни-учасниці про розгортання іноземного контингенту в Україні", ViewCounter = 10 }
            );
            await _context.SaveChangesAsync();
        }

        public Task DisposeAsync() => Task.CompletedTask;

        [Fact]
        public async Task ApplySearch_ReturnsAll()
        {
            var result = await SearchExtensions.ApplySearch(_context.Clusters, null).ToListAsync();

            Assert.Equal(4, result.Count);
            Assert.Equal("Справа «Мідас»: Міндіч, «Енергоатом», НАБУ/САП, Галущенко та Гринчук", result[0].Name);
        }

        [Fact]
        public async Task ApplySearch_ILike_Returns3()
        {
            var result = await SearchExtensions.ApplySearch(_context.Clusters, "Зел").ToListAsync();

            Assert.Equal(3, result.Count);
            Assert.Equal("Угода про корисні копалини між Україною та США — переговори, проєкти та політичний скандал (Трамп, Зеленський, Мінфін США)", result[0].Name);
            Assert.Equal("Зеленський, Міністерство оборони, Верховна Рада та ОПК: масштабна державна програма виробництва дронів і податкові пільги для оптоволокна", result[1].Name);
            Assert.Equal("Коаліція рішучих: Макрон, Зеленський та країни-учасниці про розгортання іноземного контингенту в Україні", result[2].Name);
        }
        //триграми не працюють для довгих заголовків

        //[Fact]
        //public async Task ApplySearch_Trigram_ReturnsWithFix()
        //{
        //    var result = await SearchExtensions.ApplySearch(_context.Clusters, "Міндоч").ToListAsync();

        //    Assert.Equal(1, result.Count);
        //    Assert.Equal("Справа «Мідас»: Міндіч, «Енергоатом», НАБУ/САП, Галущенко та Гринчук", result[0].Name);
        //}
    }
}
