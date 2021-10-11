using BetsApi;
using BetsApi_Business.Interfaces;
using BetsApi_Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BetsApi_Test {
    public class WagerControllerTest {
        //What methods from the WagerController do we want to test?
        //1. GetWagers()
        [Fact]
        public async void TestGetFullListOfWagers() {
            var mockRepo = new Mock<IWagerRepo>();
            mockRepo.Setup(repo => repo.WagerListAsnyc())
                .ReturnsAsync(GetTestSessions());
            var controller = new WagersController(mockRepo.Object);

            var result = await controller.GetWagers();

            Assert.True(result is ActionResult<List<ViewWager>>);
            Assert.Equal(2, result.Value.Count);
        }
        //2. GetPayouts(int fightid, int fighterid)
        //3. Create(ViewWager vw)

        private List<ViewWager> GetTestSessions() {
            var sessions = new List<ViewWager>();
            sessions.Add(new ViewWager() {
                UserId = new Guid("f814ad2f-a55a-4272-8af1-1bb9190c1983"),
                FightId = 10,
                Amount = 100,
                FighterId = 3
            });
            sessions.Add(new ViewWager() {
                UserId = new Guid("f814ad2f-a55a-4272-8af1-1bb9190c1984"),
                FightId = 10,
                Amount = 300,
                FighterId = 3
            });
            return sessions;
        }
    }
}
