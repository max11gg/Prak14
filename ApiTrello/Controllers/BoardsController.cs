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
    public class BoardsController : ControllerBase
    {
        private readonly TrelloContext _context;

        public BoardsController(TrelloContext context)
        {
            _context = context;
        }

        // GET: api/Boards
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Board>>> GetBoards()
        {
          if (_context.Boards == null)
          {
              return NotFound();
          }
            return await _context.Boards.ToListAsync();
        }

        // GET: api/Boards/ByUser/5
        [HttpGet("ByUser/{userId}")]
        public ActionResult<IEnumerable<Board>> GetBoardsByUser(int userId)
        {
            var boards = _context.Boards.Where(b => b.UserId == userId).ToList();

            if (boards == null || boards.Count == 0)
            {
                return NotFound();
            }

            return boards;
        }


        // GET: api/Boards/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Board>> GetBoard(int id)
        {
          if (_context.Boards == null)
          {
              return NotFound();
          }
            var board = await _context.Boards.FindAsync(id);

            if (board == null)
            {
                return NotFound();
            }

            return board;
        }

        // PUT: api/Boards/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBoard(int id, Board board)
        {
            if (id != board.BoardId)
            {
                return BadRequest();
            }

            _context.Entry(board).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BoardExists(id))
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

        // POST: api/Boards
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Board>> PostBoard(Board board)
        {
          if (_context.Boards == null)
          {
              return Problem("Entity set 'TrelloContext.Boards'  is null.");
          }
            _context.Boards.Add(board);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBoard", new { id = board.BoardId }, board);
        }

        // POST: api/Boards
        [HttpPost("add")]
        public async Task<ActionResult<Board>> PostBoards([FromBody] BoardCreateModel model)
        {
            if (_context.Boards == null || _context.Users == null)
            {
                return Problem("Entity set 'TrelloContext.Boards' or 'TrelloContext.Users' is null.");
            }

            // Находим пользователя по email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == model.EmailUser);
            if (user == null)
            {
                // Пользователь не найден
                return NotFound($"User with email {model.EmailUser} not found.");
            }

            // Создаем новую доску с привязкой к UserId
            var board = new Board
            {
                BoardName = model.BoardName,
                UserId = user.UserId
            };

            _context.Boards.Add(board);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBoard), new { id = board.BoardId }, board);
        }
        public class BoardCreateModel
        {
            public string BoardName { get; set; }
            public string EmailUser { get; set; }
        }


        // DELETE: api/Boards/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBoard(int id)
        {
            if (_context.Boards == null)
            {
                return NotFound();
            }
            var board = await _context.Boards.FindAsync(id);
            if (board == null)
            {
                return NotFound();
            }

            _context.Boards.Remove(board);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool BoardExists(int id)
        {
            return (_context.Boards?.Any(e => e.BoardId == id)).GetValueOrDefault();
        }
    }
}
