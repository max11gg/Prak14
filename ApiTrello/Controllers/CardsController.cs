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
    public class CardsController : ControllerBase
    {
        private readonly TrelloContext _context;

        public CardsController(TrelloContext context)
        {
            _context = context;
        }

        // GET: api/Cards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Card>>> GetCards()
        {
          if (_context.Cards == null)
          {
              return NotFound();
          }
            return await _context.Cards.ToListAsync();
        }

        // GET: api/Cards/ByList/{listId}
        [HttpGet("ByList/{listId}")]
        public async Task<ActionResult<List<Card>>> GetCardsByList(int listId)
        {
            var cards = await _context.Cards.Where(c => c.ListId == listId).ToListAsync();

            if (cards == null || cards.Count == 0)
            {
                return NotFound();
            }

            return cards;
        }


        // GET: api/Cards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Card>> GetCard(int id)
        {
          if (_context.Cards == null)
          {
              return NotFound();
          }
            var card = await _context.Cards.FindAsync(id);

            if (card == null)
            {
                return NotFound();
            }

            return card;
        }

        // PUT: api/Cards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCard(int id, Card card)
        {
            if (id != card.CardId)
            {
                return BadRequest();
            }

            _context.Entry(card).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
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

        // PUT: api/Cards/5/Description
        [HttpPut("{id}/Description")]
        public async Task<IActionResult> UpdateCardDescription(int id, [FromBody] CardDescriptionUpdateModel model)
        {
            if (model == null)
            {
                return BadRequest("Model is null.");
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound($"Card with ID {id} not found.");
            }

            card.CardDescription = model.CardDescription; // Обновляем только описание

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
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

        public class CardDescriptionUpdateModel
        {
            public string? CardDescription { get; set; }
        }

        // PUT: api/Cards/5/Deadline
        [HttpPut("{id}/Deadline")]
        public async Task<IActionResult> UpdateCardDeadline(int id, [FromBody] CardDeadlineUpdateModel model)
        {
            if (model == null)
            {
                return BadRequest("Model is null.");
            }

            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound($"Card with ID {id} not found.");
            }

            // Обновляем только дату дедлайна
            card.Deadline = model.Deadline;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CardExists(id))
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

        public class CardDeadlineUpdateModel
        {
            public DateTime? Deadline { get; set; }
        }


        // POST: api/Cards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Card>> PostCard(Card card)
        {
          if (_context.Cards == null)
          {
              return Problem("Entity set 'TrelloContext.Cards'  is null.");
          }
            _context.Cards.Add(card);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCard", new { id = card.CardId }, card);
        }

        // DELETE: api/Cards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCard(int id)
        {
            if (_context.Cards == null)
            {
                return NotFound();
            }
            var card = await _context.Cards.FindAsync(id);
            if (card == null)
            {
                return NotFound();
            }

            _context.Cards.Remove(card);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CardExists(int id)
        {
            return (_context.Cards?.Any(e => e.CardId == id)).GetValueOrDefault();
        }
    }
}
