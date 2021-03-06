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

        /// <summary>
        /// Assigning an inMemoryDatabase called TestDb with the WageDbContext as a template
        /// </summary>
        public static DbContextOptions<WageDbContext> options { get; set; } = new DbContextOptionsBuilder<WageDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;
        
        private WageDbContext _context = new WageDbContext(options);
        private WagerRepo wr;

        /// <summary>
        /// Purpose: This constructor seeds the inMemoryDatabase with records to be tested throughout this file.
        /// </summary>
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
                UserId = new Guid("f814ad2f-a55a-4272-8af1-1bb9190c1987"),
                FightId = 7,
                Amount = 250,
                FighterId = 5
            };
            Wager w5 = new Wager() {
                UserId = new Guid("f814ad2f-a55a-4272-8af1-1bb9190c1994"),
                FightId = 11,
                Amount = 250,
                FighterId = 6
            };
            Wager w6 = new Wager()
            {
                UserId = new Guid("f815ad2f-a25a-4272-8af1-1bb9190c1991"),
                FightId = 8,
                Amount = 250,
                FighterId = 12
            };

            _context.Wagers.Add(w1);
            _context.Wagers.Add(w2);
            _context.Wagers.Add(w3);
            _context.Wagers.Add(w4);
            _context.Wagers.Add(w5);
            _context.Wagers.Add(w6);
            _context.SaveChanges();
            wr = new WagerRepo(_context);
        }
        /// <summary>
        /// Method being tested: EFToView(Wager ef)
        /// </summary>
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
        /// <summary>
        /// Method being tested: ViewToEF(ViewWager view)
        /// </summary>
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
        /// <summary>
        /// Method being tested: WagerListAsync()
        /// </summary>
        [Fact]
        public async void TestWagerListAsync()
        {
            List<ViewWager> results = await wr.WagerListAsnyc();

            Assert.True(results is List<ViewWager>);
            Assert.Equal(6, results.Count);
        }
        /// <summary>
        /// Method being tested: SpecificWagerListAsync(int curFightId)
        /// </summary>
        /// <param name="fight"></param>
        [Theory]
        [InlineData(10)]
        public async void TestSpecificWagerListAsnyc(int fight) {
            List<ViewWager> results1 = await wr.SpecificWagerListAsnyc(fight);

            Assert.True(results1 is List<ViewWager>);
            Assert.Equal(3, results1.Count);
        }
        /// <summary>
        /// Method being tested: ReturnUsersToPayoutsAsync(int curFightId, int winningFighterId)
        /// </summary>
        /// <param name="fightId"></param>
        /// <param name="fighterId"></param>
        [Theory]
        [InlineData(10,3)]
        public async void TestReturnUsersToPayoutsAsync(int fightId, int fighterId) {
            var winningUsers = await wr.ReturnUsersToPayoutsAsnyc(fightId, fighterId);

            Assert.Equal(162, winningUsers[0].TotalCurrency);
            Assert.Equal(487, winningUsers[1].TotalCurrency);
        }
        /// <summary>
        /// Method being tested: PostWagerAsync(ViewWager vw)
        /// </summary>
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
        /// <summary>
        /// Method being tested: PutWagerAsync(ViewWager vw)
        /// </summary>
        /// <param name="fightId"></param>
        /// <param name="fighterId"></param>
        [Theory]
        [InlineData(8, 12)]
        public async void TestPutWagerAsync(int fightId, int fighterId)
        {
            Wager w = await _context.Wagers.Where(e => e.FightId == fightId && e.FighterId == fighterId).FirstOrDefaultAsync();

            ViewWager vw = new ViewWager();
                vw.UserId = w.UserId;
                vw.FightId = w.FightId;
                vw.FighterId = w.FighterId;
                vw.Amount = 10;

                ViewWager wager = await wr.putWagerAsnyc(vw);

                Assert.Equal(wager.UserId, w.UserId);
                Assert.Equal(wager.FighterId, w.FighterId);
                Assert.Equal(wager.FightId, w.FightId);
                Assert.Equal(wager.Amount, w.Amount);
                Assert.Equal(10, wager.Amount);          
        }
    }
}
