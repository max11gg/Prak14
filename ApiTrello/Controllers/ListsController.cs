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
    public class ListsController : ControllerBase
    {
        private readonly TrelloContext _context;

        public ListsController(TrelloContext context)
        {
            _context = context;
        }

        // GET: api/Lists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<List>>> GetLists()
        {
          if (_context.Lists == null)
          {
              return NotFound();
          }
            return await _context.Lists.ToListAsync();
        }

        // GET: api/Lists/ByBoard/{boardId}
        [HttpGet("ByBoard/{boardId}")]
        public ActionResult<List<List>> GetListsByBoard(int boardId)
        {
            var lists = _context.Lists.Where(l => l.BoardId == boardId).ToList();

            if (lists == null || lists.Count == 0)
            {
                return NotFound();
            }

            return lists;
        }


        // GET: api/Lists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<List>> GetList(int id)
        {
          if (_context.Lists == null)
          {
              return NotFound();
          }
            var list = await _context.Lists.FindAsync(id);

            if (list == null)
            {
                return NotFound();
            }

            return list;
        }

        // PUT: api/Lists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutList(int id, List list)
        {
            if (id != list.ListId)
            {
                return BadRequest();
            }

            _context.Entry(list).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ListExists(id))
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

        // POST: api/Lists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<List>> PostList(List list)
        {
          if (_context.Lists == null)
          {
              return Problem("Entity set 'TrelloContext.Lists'  is null.");
          }
            _context.Lists.Add(list);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetList", new { id = list.ListId }, list);
        }

        // DELETE: api/Lists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteList(int id)
        {
            if (_context.Lists == null)
            {
                return NotFound();
            }
            var list = await _context.Lists.FindAsync(id);
            if (list == null)
            {
                return NotFound();
            }

            _context.Lists.Remove(list);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ListExists(int id)
        {
            return (_context.Lists?.Any(e => e.ListId == id)).GetValueOrDefault();
        }
    }
}
