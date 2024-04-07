using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ApiTrello.Models;

namespace ApiTrello.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavouritesController : ControllerBase
    {
        private readonly TrelloContext _context;

        public FavouritesController(TrelloContext context)
        {
            _context = context;
        }

        // GET: api/Favourites
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Favourite>>> GetFavourite()
        {
          if (_context.Favourites == null)
          {
              return NotFound();
          }
            return await _context.Favourites.ToListAsync();
        }

        // GET: api/Favourites/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Favourite>> GetFavourite(int id)
        {
          if (_context.Favourites == null)
          {
              return NotFound();
          }
            var favourite = await _context.Favourites.FindAsync(id);

            if (favourite == null)
            {
                return NotFound();
            }

            return favourite;
        }
        // GET: api/Favourites/User/5
        [HttpGet("User/{userId}")]
        public async Task<ActionResult<IEnumerable<Favourite>>> GetFavouritesByUserId(int userId)
        {
            if (_context.Favourites == null)
            {
                return NotFound();
            }

            var favourites = await _context.Favourites
                                            .Where(f => f.UserId == userId)
                                            .ToListAsync();

            if (favourites == null || !favourites.Any())
            {
                return NotFound();
            }

            return favourites;
        }

        // PUT: api/Favourites/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFavourite(int id, Favourite favourite)
        {
            if (id != favourite.FavouriteId)
            {
                return BadRequest();
            }

            _context.Entry(favourite).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FavouriteExists(id))
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

        // POST: api/Favourites
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Favourite>> PostFavourite(Favourite favourite)
        {
          if (_context.Favourites == null)
          {
              return Problem("Entity set 'TrelloContext.Favourite'  is null.");
          }
            _context.Favourites.Add(favourite);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFavourite", new { id = favourite.FavouriteId }, favourite);
        }

        // DELETE: api/Favourites/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFavourite(int id)
        {
            if (_context.Favourites == null)
            {
                return NotFound();
            }
            var favourite = await _context.Favourites.FindAsync(id);
            if (favourite == null)
            {
                return NotFound();
            }

            _context.Favourites.Remove(favourite);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FavouriteExists(int id)
        {
            return (_context.Favourites?.Any(e => e.FavouriteId == id)).GetValueOrDefault();
        }
    }
}
