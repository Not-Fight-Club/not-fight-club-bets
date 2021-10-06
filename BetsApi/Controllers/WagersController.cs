using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BetsApi_Data.Models;

namespace BetsApi
{
    [Route("api/[controller]")]
    [ApiController]
    public class WagersController : ControllerBase
    {
        private readonly WageDbContext _context;

        public WagersController(WageDbContext context)
        {
            _context = context;
        }

        // GET: api/Wagers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wager>>> GetWagers()
        {
            return await _context.Wagers.ToListAsync();
        }

        // GET: api/Wagers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wager>> GetWager(int id)
        {
            var wager = await _context.Wagers.FindAsync(id);

            if (wager == null)
            {
                return NotFound();
            }

            return wager;
        }

        // PUT: api/Wagers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutWager(int id, Wager wager)
        {
            if (id != wager.WagerId)
            {
                return BadRequest();
            }

            _context.Entry(wager).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!WagerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Wagers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Wager>> PostWager(Wager wager)
        {
            _context.Wagers.Add(wager);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetWager", new { id = wager.WagerId }, wager);
        }

        // DELETE: api/Wagers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWager(int id)
        {
            var wager = await _context.Wagers.FindAsync(id);
            if (wager == null)
            {
                return NotFound();
            }

            _context.Wagers.Remove(wager);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool WagerExists(int id)
        {
            return _context.Wagers.Any(e => e.WagerId == id);
        }
    }
}
