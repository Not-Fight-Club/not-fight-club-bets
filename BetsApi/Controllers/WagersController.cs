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
        //private readonly WageDbContext _context;
        private readonly IWagerRepo _wagerRepo;

        public WagersController(IWagerRepo wr)
        {
            _wagerRepo = wr;
        }

        // GET: api/Wagers
        [HttpGet]
        public async Task<ActionResult<List<ViewWager>>> GetWagers()
        {
            return await _wagerRepo.WagerListAsnyc();
        }

        // GET: api/Wagers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wager>> GetWager(int id)
        {
            /*var wager = await _context.Wagers.FindAsync(id);

            if (wager == null)
            {
                return NotFound();
            }

            return wager;*/
            return null;
        }

        // PUT: api/Wagers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

        // POST: api/Wagers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Wager>> PostWager(Wager wager)
        {
            /*
            _context.Wagers.Add(wager);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWager", new { id = wager.WagerId }, wager);
            */
            return null;
        }

        // DELETE: api/Wagers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWager(int id)
        {
            /*
            var wager = await _context.Wagers.FindAsync(id);
            if (wager == null)
            {
                return NotFound();
            }

            _context.Wagers.Remove(wager);
            await _context.SaveChangesAsync();

            return NoContent();
            */
            return null;
        }

        private bool WagerExists(int id)
        {
            /*
            return _context.Wagers.Any(e => e.WagerId == id);
            */
            return false;
        }
        [HttpGet("{fightid}/{fighterid}")]
        public async Task<List<ViewUser>> GetPayouts(int fightid, int fighterid) {
            return await _wagerRepo.ReturnUsersToPayoutsAsnyc(fightid,fighterid);
        }

        /// <summary>
        /// This method post wager info to the Wagers table
        /// </summary>
        /// <param name="vw"></param>
        /// <returns></returns>
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
