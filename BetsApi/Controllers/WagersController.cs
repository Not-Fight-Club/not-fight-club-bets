using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetsApi_Data;
using BetsApi_Models.ViewModels;
using BetsApi_Business.Interfaces;
using BetsApi_Models.EFModels;

namespace BetsApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class WagersController : ControllerBase
    {
        private readonly IWagerRepo _wagerRepo;

        public WagersController(IWagerRepo wr)
        {
            _wagerRepo = wr;
        }
        /// <summary>
        /// Purpose: Get all records from the Wager Table
        /// </summary>
        /// <returns>List of ViewWager</returns>
        [HttpGet]
        public async Task<ActionResult<List<ViewWager>>> GetWagers()
        {
            return await _wagerRepo.WagerListAsnyc();
        }
        /// <summary>
        /// Purpose: Updates a record in the Wager Table
        /// </summary>
        /// <param name="viewwager"></param>
        /// <returns>ViewWager</returns>
        [HttpPut]
        public async Task<ActionResult<ViewWager>> PutWager(ViewWager viewwager)
        {
            if (!ModelState.IsValid) return BadRequest();

            ViewWager c1 = await _wagerRepo.putWagerAsnyc(viewwager);
            if (c1 == null)
            {
                return NotFound();
            }

            return c1;
        }
        /// <summary>
        /// Purpose: Get a list of users to be paid for a concluded fight
        /// </summary>
        /// <param name="fightid"></param>
        /// <param name="fighterid"></param>
        /// <returns>List of ViewUser</returns>
        [HttpGet("{fightid}/{fighterid}")]
        public async Task<List<ViewUser>> GetPayouts(int fightid, int fighterid) {
            return await _wagerRepo.ReturnUsersToPayoutsAsnyc(fightid,fighterid);
        }
        /// <summary>
        /// This method post wager info to the Wagers table
        /// </summary>
        /// <param name="vw"></param>
        /// <returns>ViewWager</returns>
        [HttpPost("postbet")]
        public async Task<ActionResult<ViewWager>> Create(ViewWager vw)
        {
            if (!ModelState.IsValid) return BadRequest();

            ViewWager vw1 = await _wagerRepo.PostWagerAsync(vw);
            if (vw1 == null)
            {
                return NotFound();
            }
            return vw1;
        }
    }
}
