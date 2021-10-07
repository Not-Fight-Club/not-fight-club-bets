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
        Task<int> GetLosingFighterBets(int curFightId, int winningFighterId);
        Task<int> GetWinningFighterBets(int curFightId, int winningFighterId);
        int GetSinglePayout(int prizePool, int userBet, int totalWinnerBets);
        Task<List<ViewWager>> GetWinningWages(int curFightId, int winningFighterId);
        Task<ViewWager> PostWagerAsync(ViewWager vw);
    }
}
