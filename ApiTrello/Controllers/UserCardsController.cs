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
    public class UserCardsController : ControllerBase
    {
        private readonly TrelloContext _context;

        public UserCardsController(TrelloContext context)
        {
            _context = context;
        }

        // GET: api/UserCards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserCard>>> GetUserCards()
        {
          if (_context.UserCards == null)
          {
              return NotFound();
          }
            return await _context.UserCards.ToListAsync();
        }

        // GET: api/UserCards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserCard>> GetUserCard(int id)
        {
          if (_context.UserCards == null)
          {
              return NotFound();
          }
            var userCard = await _context.UserCards.FindAsync(id);

            if (userCard == null)
            {
                return NotFound();
            }

            return userCard;
        }

        // GET: api/UserCards/ByCard/5
        [HttpGet("ByCard/{cardId}")]
        public async Task<ActionResult<IEnumerable<UserCard>>> GetUserCardsByCard(int cardId)
        {
            if (_context.UserCards == null)
            {
                return NotFound("UserCards set is null.");
            }

            var userCards = await _context.UserCards
                                          .Where(uc => uc.CardId == cardId)
                                          .Include(uc => uc.User) // Добавление этой строки предполагает, что вы хотите включить данные пользователя
                                          .Include(uc => uc.Card) // и данные карточки в результат запроса
                                          .ToListAsync();

            if (userCards == null || userCards.Count == 0)
            {
                return NotFound($"No UserCards found for CardId {cardId}.");
            }

            return userCards;
        }

        // GET: api/UserCards/ByUser/5
        [HttpGet("ByUser/{userId}")]
        public async Task<ActionResult<IEnumerable<UserCard>>> GetUserCardsByUser(int userId)
        {
            if (_context.UserCards == null)
            {
                return NotFound("UserCards set is null.");
            }

            var userCards = await _context.UserCards
                                          .Where(uc => uc.UserId == userId)
                                          .Include(uc => uc.User) // Для включения данных пользователя
                                          .Include(uc => uc.Card) // и данных карточки в результат запроса
                                          .ToListAsync();

            if (userCards == null || userCards.Count == 0)
            {
                return NotFound($"No UserCards found for UserId {userId}.");
            }

            return userCards;
        }


        // PUT: api/UserCards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserCard(int id, UserCard userCard)
        {
            if (id != userCard.UserCardId)
            {
                return BadRequest();
            }

            _context.Entry(userCard).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserCardExists(id))
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

        public class UserCardCreateModel
        {
            public string Email { get; set; } // Изменено на прием email вместо UserId
            public int CardId { get; set; }
        }

        // POST: api/UserCards
        [HttpPost]
        public async Task<ActionResult<UserCard>> PostUserCard([FromBody] UserCardCreateModel model)
        {
            if (_context.UserCards == null || _context.Users == null || _context.Cards == null)
            {
                return Problem("Entity set 'TrelloContext.UserCards', 'TrelloContext.Users', or 'TrelloContext.Cards' is null.");
            }

            // Находим пользователя по email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
            if (user == null)
            {
                return NotFound($"User with email {model.Email} not found.");
            }

            var cardExists = await _context.Cards.AnyAsync(c => c.CardId == model.CardId);
            if (!cardExists)
            {
                return NotFound($"Card with ID {model.CardId} not found.");
            }

            // Создаем новую связь пользователя с карточкой, используя UserId найденного пользователя
            var userCard = new UserCard
            {
                UserId = user.UserId,
                CardId = model.CardId
            };

            _context.UserCards.Add(userCard);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserCard), new { id = userCard.UserCardId }, userCard);
        }



        // DELETE: api/UserCards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserCard(int id)
        {
            if (_context.UserCards == null)
            {
                return NotFound();
            }
            var userCard = await _context.UserCards.FindAsync(id);
            if (userCard == null)
            {
                return NotFound();
            }

            _context.UserCards.Remove(userCard);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserCardExists(int id)
        {
            return (_context.UserCards?.Any(e => e.UserCardId == id)).GetValueOrDefault();
        }
    }
}
