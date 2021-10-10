using BetsApi_Business.Repository;
using BetsApi_Data;
using BetsApi_Models.EFModels;
using BetsApi_Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BetsApi_Test {
    public class WagerRepoTest {

        public static DbContextOptions<WageDbContext> options { get; set; } = new DbContextOptionsBuilder<WageDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        private WageDbContext _context = new WageDbContext(options);
        private WagerRepo wr;

        public WagerRepoTest() {
            _context.Database.EnsureDeleted();
            _context.Database.EnsureCreated();
            Wager w1 = new Wager() {
                UserId = new Guid("f814ad2f-a55a-4272-8af1-1bb9190c1983"),
                FightId = 10,
                Amount = 100,
                FighterId = 3
            };
            Wager w2 = new Wager() {
                UserId = new Guid("f814ad2f-a55a-4272-8af1-1bb9190c1984"),
                FightId = 10,
                Amount = 300,
                FighterId = 3
            };
            Wager w3 = new Wager() {
                UserId = new Guid("f814ad2f-a55a-4272-8af1-1bb9190c1985"),
                FightId = 10,
                Amount = 250,
                FighterId = 4
            };
            Wager w4 = new Wager() {
                UserId = new Guid("f814ad2f-a55a-4272-8af1-1bb9190c1984"),
                FightId = 7,
                Amount = 250,
                FighterId = 5
            };
            Wager w5 = new Wager() {
                UserId = new Guid("f814ad2f-a55a-4272-8af1-1bb9190c1985"),
                FightId = 7,
                Amount = 250,
                FighterId = 6
            };
            _context.Wagers.Add(w1);
            _context.Wagers.Add(w2);
            _context.Wagers.Add(w3);
            _context.Wagers.Add(w4);
            _context.Wagers.Add(w5);
            _context.SaveChanges();
            wr = new WagerRepo(_context);
        }
        //What methods from the WagerRepo do we want to test?
        //1. EFToView(Wager ef)
        [Fact]
        public void TestEFWagerToViewWager() {

            Wager sut = new Wager() {
                UserId = new Guid("f814ad2f-a55a-4272-8af1-1bb9190c1983"),
                FightId = 10,
                Amount = 100,
                FighterId = 3
            };

            ViewWager result = wr.EFToView(sut);

            Assert.True(result is ViewWager);
            Assert.Equal(sut.UserId, result.UserId);
            Assert.Equal(sut.FightId, result.FightId);
            Assert.Equal(sut.FighterId, result.FighterId);
            Assert.Equal(sut.Amount, result.Amount);
        }
        //2. ViewToEF(ViewWager view)
        [Fact]
        public async void TestViewWagerToEFWager() {
            ViewWager vw = new ViewWager() {
                UserId = new Guid("f814ad2f-a55a-4272-8af1-1bb9190c1983"),
                FightId = 10,
                Amount = 100,
                FighterId = 3
            };

            Wager result = await wr.ViewToEF(vw);

            Assert.True(result is Wager);
            Assert.Equal(vw.UserId, result.UserId);
            Assert.Equal(vw.FightId, result.FightId);
            Assert.Equal(vw.FighterId, result.FighterId);
            Assert.Equal(vw.Amount, result.Amount);
        }
        //3. WagerListAsync()
        [Fact]
        public async void TestWagerListAsync() {
            List<ViewWager> results = await wr.WagerListAsnyc();

            Assert.True(results is List<ViewWager>);
            Assert.Equal(5, results.Count);
        }
        //4. SpecificWagerListAsync(int curFightId)
        [Theory]
        [InlineData(10)]
        public async void TestSpecificWagerListAsnyc(int fight) {
            List<ViewWager> results1 = await wr.SpecificWagerListAsnyc(fight);

            Assert.True(results1 is List<ViewWager>);
            Assert.Equal(3, results1.Count);
        }
        //5. ReturnUsersToPayoutsAsync(int curFightId, int winningFighterId)
        [Theory]
        [InlineData(10,3)]
        public async void TestReturnUsersToPayoutsAsync(int fightId, int fighterId) {
            var winningUsers = await wr.ReturnUsersToPayoutsAsnyc(fightId, fighterId);

            Assert.Equal(162, winningUsers[0].TotalCurrency);
            Assert.Equal(487, winningUsers[1].TotalCurrency);
        }

        //6. PostWagerAsync(ViewWager vw)
        [Fact]
        public async void TestPostWagerAsync()
        {
            ViewWager w = new ViewWager()
            {
                UserId = new Guid("f814ad2f-a55a-4272-8af1-1bb9190c2021"),
                FightId = 21,
                Amount = 121,
                FighterId = 11
            };
            ViewWager wager = await wr.PostWagerAsync(w);

            Assert.Equal(wager.UserId, w.UserId);
            Assert.Equal(wager.FighterId, w.FighterId);
            Assert.Equal(wager.FightId, w.FightId);
            Assert.Equal(wager.Amount, w.Amount);
        }
        //7. PutWagerAsync(ViewWager vw)
    }
}
