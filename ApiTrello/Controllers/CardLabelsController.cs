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
    public class CardLabelsController : ControllerBase
    {
        private readonly TrelloContext _context;

        public CardLabelsController(TrelloContext context)
        {
            _context = context;
        }

        // GET: api/CardLabels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CardLabel>>> GetCardLabels()
        {
          if (_context.CardLabels == null)
          {
              return NotFound();
          }
            return await _context.CardLabels.ToListAsync();
        }
        // GET: api/CardLabels/ByCard/5
        [HttpGet("ByCard/{cardId}")]
        public async Task<ActionResult<IEnumerable<CardLabel>>> GetLabelsByCardId(int cardId)
        {
            if (_context.CardLabels == null)
            {
                return NotFound();
            }

            var cardLabels = await _context.CardLabels
                                           .Where(cl => cl.CardId == cardId)
                                           .ToListAsync();

            if (cardLabels == null || cardLabels.Count == 0)
            {
                return NotFound();
            }

            return cardLabels;
        }


        // GET: api/CardLabels/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CardLabel>> GetCardLabel(int id)
        {
          if (_context.CardLabels == null)
          {
              return NotFound();
          }
            var cardLabel = await _context.CardLabels.FindAsync(id);

            if (cardLabel == null)
            {
                return NotFound();
            }

            return cardLabel;
        }

        // PUT: api/CardLabels/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCardLabel(int id, CardLabel cardLabel)
        {
            if (id != cardLabel.CardLabelId)
            {
                return BadRequest();
            }

            _context.Entry(cardLabel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardLabelExists(id))
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

        // POST: api/CardLabels
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CardLabel>> PostCardLabel(CardLabel cardLabel)
        {
          if (_context.CardLabels == null)
          {
              return Problem("Entity set 'TrelloContext.CardLabels'  is null.");
          }
            _context.CardLabels.Add(cardLabel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCardLabel", new { id = cardLabel.CardLabelId }, cardLabel);
        }

        // DELETE: api/CardLabels/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCardLabel(int id)
        {
            if (_context.CardLabels == null)
            {
                return NotFound();
            }
            var cardLabel = await _context.CardLabels.FindAsync(id);
            if (cardLabel == null)
            {
                return NotFound();
            }

            _context.CardLabels.Remove(cardLabel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CardLabelExists(int id)
        {
            return (_context.CardLabels?.Any(e => e.CardLabelId == id)).GetValueOrDefault();
        }
    }
}
