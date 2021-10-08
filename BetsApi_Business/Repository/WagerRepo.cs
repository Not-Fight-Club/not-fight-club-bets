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

        public async Task<Wager> ViewToEF(ViewWager view) {
           
            Wager wm = new Wager();

            var allwagersquery = (from o in _context.Wagers
                                  where o.UserId == view.UserId && o.FightId == view.FightId && o.FighterId == view.FighterId && o.Amount == view.Amount
                                  select new { o }
           ).ToListAsync();
            foreach (var p in await allwagersquery)
            {
                wm.WagerId = p.o.WagerId;
                wm.UserId = p.o.UserId;
                wm.Amount = p.o.Amount;
                wm.FighterId = p.o.FighterId;
                wm.FightId = p.o.FightId;

            }
            return wm;
        }

        public async Task<List<ViewWager>> WagerListAsnyc() {

            ViewWager vws = new ViewWager();
            List<ViewWager> vwsl = new List<ViewWager>();
            var allwagersquery = (from o in _context.Wagers
                                  select new { o }
           ).ToListAsync();
            foreach (var p in await allwagersquery)
            {
                vws = new ViewWager();
                vws.UserId = p.o.UserId;
                vws.Amount = p.o.Amount;
                vws.FighterId = p.o.FighterId;
                vws.FightId = p.o.FightId;
                vwsl.Add(vws);
            }

            return vwsl;
 
        }

        public async Task<List<ViewWager>> SpecificWagerListAsnyc(int curFightId) {

            ViewWager vws = new ViewWager();
            List<ViewWager> vwsl = new List<ViewWager>();
            var allwagersquery = (from o in _context.Wagers
                              where  o.FightId == curFightId
                              select new { o}
           ).ToListAsync();
            foreach (var p in await allwagersquery )
            {
                vws = new ViewWager();
                vws.UserId = p.o.UserId;
                vws.Amount = p.o.Amount;
                vws.FighterId = p.o.FighterId;
                vws.FightId = p.o.FightId;
                vwsl.Add(vws);


            }

            return vwsl;
        }


        public async Task<List<ViewUser>> ReturnUsersToPayoutsAsnyc(int curFightId, int winningFighterId)
        {
            //List of Winners
            var winnerBets = await (from o in _context.Wagers
                              where o.FighterId == winningFighterId && o.FightId == curFightId
                              select new { o.UserId, o.Amount }
                  ).ToListAsync();
            //List of Losers
            var loserBets = await(from o in _context.Wagers
                             where o.FighterId != winningFighterId && o.FightId == curFightId
                             select new { o.UserId, o.Amount }
      ).ToListAsync();
            //Tptal Won and Lost by Winners and Losers
            double totalWinningBets = winnerBets.Select( c =>  c.Amount).Sum();
            double totalLosingBets =  loserBets.Select( c =>  c.Amount).Sum();
            
            List<ViewUser> userToBePaid = new List<ViewUser>();
            int payout;
            double fractionOfWinnings;
            //Adds a Winning ViewUser to userToBePaid List
            foreach (var a in winnerBets)
            {
                ViewUser curUser = new ViewUser();
                curUser.UserId = a.UserId;

                payout = 0;
                payout += a.Amount;
                fractionOfWinnings = (double)a.Amount / (double)totalWinningBets;
                payout += (int)(totalLosingBets * fractionOfWinnings);


                curUser.TotalCurrency = payout;
                userToBePaid.Add(curUser);
            }
             
            return userToBePaid;
        }



        public async Task<ViewWager> putWagerAsnyc(ViewWager vw)
        {

            int c1 = await _context.Database.ExecuteSqlRawAsync("UPDATE Wager SET Amount= {0} WHERE UserId= {1} AND FightId= {2} AND FighterId= {3} ", vw.Amount, vw.UserId, vw.FightId, vw.FighterId);// default is NULL

            if (c1 != 1) return null;

            Wager w1 = await _context.Wagers.FromSqlRaw<Wager>("SELECT * FROM Wager WHERE FightId = {0} AND FighterId = {1} AND UserId = {2}", vw.FightId, vw.FighterId, vw.UserId).FirstOrDefaultAsync();// default is NULL     
            if (w1.Amount == vw.Amount)
                return EFToView(w1);
            else
                return null;
        }


        public async Task<ViewWager> PostWagerAsync(ViewWager vw)
        {
            int i = await _context.Database.ExecuteSqlRawAsync("INSERT INTO Wager values ({0},{1},{2},{3})", vw.UserId, vw.FightId, vw.Amount,vw.FighterId);// default is null
            
            if (i != 1) return null;
            Wager newWager =  _context.Wagers.FromSqlRaw<Wager>("SELECT * FROM Wager WHERE UserId={0} AND FightId={1} AND FighterId={2}", vw.UserId, vw.FightId, vw.FighterId).FirstOrDefault();
            
            return EFToView(newWager);
        }
    }//EOC
}//EON
