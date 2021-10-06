using BetsApi_Business.Interfaces;
using BetsApi_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetsApi_Models;
using BetsApi_Models.EFModels;
using BetsApi_Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BetsApi_Business.Repository {
    class WagerRepo : IModelMapper<Wager, ViewWager>, IWagerRepo {

        private readonly WageDbContext _context;
        public WagerRepo(WageDbContext context) {
            _context = context;
        }
        public ViewWager EFToView(Wager ef) {
            ViewWager vw = new ViewWager();
            vw.UserId = ef.UserId;
            vw.FightId = ef.FightId;
            vw.FighterId = ef.FighterId;
            vw.Amount = ef.Amount;
            return vw;
        }

        public async Task<Wager> ViewToEF(ViewWager view) {
            Wager w = await _context.Wagers.FromSqlRaw<Wager>("SELECT * FROM Wager WHERE UserId = {0} AND FightId = {1} AND FighterId = {2} AND Amount = {3}", view.UserId, view.FightId, view.FighterId, view.Amount).FirstOrDefaultAsync();
            return w;
        }

        public async Task<List<ViewWager>> WagerListAsnyc() {
            List<Wager> allWagers = await _context.Wagers.FromSqlRaw<Wager>("SELECT * FROM Wager").ToListAsync();
            List<ViewWager> vws = new List<ViewWager>();
            foreach(Wager w in allWagers) {
                vws.Add(EFToView(w));
            }
            return vws;
        }

        public async Task<List<ViewWager>> SpecificWagerListAsnyc(int curFightId) {
            List<Wager> allWagers = await _context.Wagers.FromSqlRaw<Wager>("SELECT * FROM Wager WHERE FightId = {0}", curFightId).ToListAsync();
            List<ViewWager> vws = new List<ViewWager>();
            foreach (Wager w in allWagers) {
                vws.Add(EFToView(w));
            }
            return vws;
        }

        public async Task<List<ViewUser>> ReturnUsersToPayoutsAsnyc(int curFightId, int winningFighterId) {
            //All records for the fight
            List<ViewWager> allBets = await SpecificWagerListAsnyc(curFightId);
            //Get the total amount of bets placed on the winner
            int totalWinningBets = await GetWinningFighterBets(curFightId, winningFighterId);
            //Get the total amount of bets placed on the loser
            int totalLosingBets = await GetLosingFighterBets(curFightId, winningFighterId);

            
            List<ViewUser> userToBePaid = new List<ViewUser>();

            return userToBePaid;
        }

        //Helper Functions
        //Get money bet on losing fighter
        public async Task<int> GetLosingFighterBets(int curFightId, int winningFighterId) {
            int stuff = 0;
            //Some select statement from josh
            return stuff;
        }
        //Get money bet on winning fighter
        public async Task<int> GetWinningFighterBets(int curFightId, int winningFighterId) {
            int stuff = 0;
            //Some select statement from josh
            return stuff;
        }
    }
}
