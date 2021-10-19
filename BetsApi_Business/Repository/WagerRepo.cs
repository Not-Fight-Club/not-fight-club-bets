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
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace BetsApi_Business.Repository {
    public class WagerRepo : IModelMapper<Wager, ViewWager>, IWagerRepo {

        private IHttpClientFactory _clientFactory;
        private ILogger<WagerRepo> _logger;
        private IConfiguration _config;
        private readonly WageDbContext _context;
        public WagerRepo(IHttpClientFactory clientFactory, ILogger<WagerRepo> logger, IConfiguration config, WageDbContext context) {
            _context = context;
            _clientFactory = clientFactory;
            _logger = logger;
            _config = config;
        }

        public WagerRepo(WageDbContext context)
        {
            _context = context;
        }
        /// <summary>
        /// Purpose: Return a converted Entity Framework Model to a View Model.
        /// </summary>
        /// <param name="ef"></param>
        /// <returns>ViewWager</returns>
        public ViewWager EFToView(Wager ef) {
            ViewWager vw = new ViewWager();
            vw.UserId = ef.UserId;
            vw.FightId = ef.FightId;
            vw.FighterId = ef.FighterId;
            vw.Amount = ef.Amount;
            return vw;
        }
        /// <summary>
        /// Purpose: Return a View Model to an Entity Framework Model by searching the Database.
        /// </summary>
        /// <param name="view"></param>
        /// <returns>Wager</returns>
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
        /// <summary>
        /// Purpose: Return all records from the Wager Table as a list of View Models.
        /// </summary>
        /// <returns>List of ViewWager</returns>
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
        /// <summary>
        /// Purpose: Return all wager records for a specific fight
        /// </summary>
        /// <param name="curFightId"></param>
        /// <returns>List of ViewWager</returns>
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
        /// <summary>
        /// Purpose: Calculates the winning bets for each user that bet on the correct fighter
        /// </summary>
        /// <param name="curFightId"></param>
        /// <param name="winningFighterId"></param>
        /// <returns>List of ViewUser</returns>
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
            //Total Won and Lost by Winners and Losers
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
        /// <summary>
        /// Purpose: Updates a wager record
        /// </summary>
        /// <param name="vw"></param>
        /// <returns>ViewWager</returns>
        public async Task<ViewWager> putWagerAsnyc(ViewWager vw)
        {        
            var updatedWager = await _context.Wagers.Where(e => e.UserId == vw.UserId && e.FightId == vw.FightId && e.FighterId == vw.FighterId).FirstOrDefaultAsync();
            updatedWager.Amount = vw.Amount; 
            _context.Update(updatedWager);
         
           await _context.SaveChangesAsync();
           return vw;          
        }

        public async Task<ViewWager> putWagerOnCustomer(ViewWager vw)
        {
            string baseUrl = _config["apiUrl:users"];
            ViewUser vu = new ViewUser() { UserId = vw.UserId, TotalCurrency = -(vw.Amount)};

            string endpointURI = $"{baseUrl}/UpdateTotal";
            var jsonBody = JsonConvert.SerializeObject(vu);

            var request = new HttpRequestMessage(HttpMethod.Put, endpointURI);
            request.Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();

            _logger.LogInformation($"base address for client api: {endpointURI}");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError($"Failed request to {endpointURI}: {response}");
                return null;
            }
            else
            {
                return vw;
            }
        }

        /// <summary>
        /// Purpose: Places a wager record into the Wager Table
        /// </summary>
        /// <param name="vw"></param>
        /// <returns>ViewWager</returns>
        public async Task<ViewWager> PostWagerAsync(ViewWager vw)
        {
            var wagerToPost = new Wager() {
                UserId = vw.UserId,
                FightId = vw.FightId,
                FighterId = vw.FighterId,
                Amount = vw.Amount
            };
            
            _context.Add(wagerToPost);
            await _context.SaveChangesAsync();

            var newWager = await _context.Wagers.Where(e => e.UserId == wagerToPost.UserId && e.FightId == wagerToPost.FightId && e.FighterId == wagerToPost.FighterId && e.Amount == wagerToPost.Amount).FirstOrDefaultAsync();

            return EFToView(newWager);
        }
    }//EOC
}//EON
