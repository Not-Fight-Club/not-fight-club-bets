using BetsApi_Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetsApi_Business.Interfaces {
    public interface IWagerRepo {
        Task<List<ViewWager>> WagerListAsnyc();
        Task<List<ViewWager>> SpecificWagerListAsnyc(int curFightId);
        Task<List<ViewUser>> ReturnUsersToPayoutsAsnyc(int curFightId, int winningFighterId);
        Task<ViewWager> PostWagerAsync(ViewWager vw);
        Task<ViewWager> putWagerAsnyc(ViewWager vw);

        Task<ViewWager> putWagerOnCustomer(ViewWager vw);
    }
}
