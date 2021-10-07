using BetsApi_Business.Interfaces;
using BetsApi_Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BetsApi_Models;
using BetsApi_Models.ViewModels;
using BetsApi_Models.EFModels;
using Microsoft.EntityFrameworkCore;

namespace BetsApi_Business.Repository {
    public class WagerRepo : IModelMapper<Wager, ViewWager>, IWagerRepo {

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

        public Wager ViewToEfModel(ViewWager vw)
        {
            Wager w = new Wager();
            w.UserId = vw.UserId;
            w.FightId = vw.FightId;
            w.FighterId = vw.FighterId;
            w.Amount = vw.Amount;
            return w;
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
            //Get the winning bets
            List<ViewWager> winningBets = await GetWinningWages(curFightId, winningFighterId);
            //Create a list of Users to be paid
            List<ViewUser> userToBePaid = new List<ViewUser>();
            foreach(ViewWager curWage in winningBets) {
                int curPayout = GetSinglePayout(totalLosingBets,curWage.Amount,totalWinningBets);
                ViewUser curUser = new ViewUser();
                curUser.UserId = curWage.UserId;
                curUser.TotalCurrency = curPayout;
                userToBePaid.Add(curUser);
            }


            return userToBePaid;
        }

        //Helper Functions
        //Get money bet on losing fighter
        public async Task<int> GetLosingFighterBets(int curFightId, int winningFighterId) {
            int stuff = 0;
            List<Wager> wstuff = await _context.Wagers.FromSqlRaw<Wager>("SELECT * FROM Wager WHERE FightId = {0} AND FighterId != {1}", curFightId, winningFighterId).ToListAsync();
            foreach(Wager w in wstuff) {
                stuff += w.Amount;
            }
            return stuff;
        }
        //Get money bet on winning fighter
        public async Task<int> GetWinningFighterBets(int curFightId, int winningFighterId) {
            int stuff = 0;
            List<Wager> wstuff = await _context.Wagers.FromSqlRaw<Wager>("SELECT * FROM Wager WHERE FightId = {0} AND FighterId = {1}", curFightId, winningFighterId).ToListAsync();
            foreach (Wager w in wstuff) {
                stuff += w.Amount;
            }
            return stuff;
        }
        //Get the amount of money to be paid to a user
        public int GetSinglePayout(int prizePool, int userBet, int totalWinnerBets) {
            int payout = 0;
            payout += userBet;
            double fractionOfWinnings = (double)userBet / (double)totalWinnerBets;
            payout += (int)(prizePool * fractionOfWinnings);
            return payout;
        }
        public async Task<List<ViewWager>> GetWinningWages(int curFightId, int winningFighterId) {
            List<Wager> wagers = await _context.Wagers.FromSqlRaw<Wager>("SELECT * FROM Wager WHERE FightId = {0} AND FighterId = {1}", curFightId, winningFighterId).ToListAsync();
            List<ViewWager> vws = new List<ViewWager>();
            foreach(Wager w in wagers) {
                vws.Add(EFToView(w));
            }
            return vws;
        }

        public async Task<ViewWager> PostWagerAsync(ViewWager vw)
        //public async Task<string> PostWagerAsync(ViewWager vw)
        {
            Wager w = ViewToEfModel(vw);
            int i = await _context.Database.ExecuteSqlRawAsync("INSERT INTO Wager values ({0},{1},{2},{3})", w.UserId, w.FightId, w.Amount, w.FighterId);// default is null
            
            if (i != 1) return null;
            Wager newWager =  _context.Wagers.FromSqlRaw<Wager>("SELECT * FROM Wager WHERE UserId={0} AND FightId={1} AND FighterId={2}", w.UserId, w.FightId, w.FighterId).FirstOrDefault(); ;
            
            return EFToView(newWager);
        }

    }
}
