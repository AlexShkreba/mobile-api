using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileAppAPI.Models;

namespace MobileAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Context _context;

        public UsersController(Context context)
        {
            _context = context;
        }
        
        [HttpPost("post")]
        public async Task<ActionResult> post(Users user)
        {
            if ((user == null) || (_context.users.Any(x => x.login == user.login)))
            {
                return BadRequest();
            }
            _context.users.Add(user);
            await _context.SaveChangesAsync();
            var tmp = await _context.users.FirstOrDefaultAsync(x => x.login == user.login);
            var categories = await _context.categories.Where(x => x.type == "Standard").ToListAsync();
            foreach(var i in categories)
            {
                _context.tasks_by_categories.Add(new Tasks_By_Categories { id_user = tmp.id, id_category = i.id, id_task = null });
                await _context.SaveChangesAsync();
            }
            return Ok(user);
        }

        [HttpDelete("delete")]
        public async Task<ActionResult<Users>> DeleteUsers(string login)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.login == login);
            if (user == null)
            {
                return NotFound();
            }
            foreach (var i in await _context.tasks_by_categories.Where(x => x.id_user == user.id).ToListAsync())
            {
                _context.tasks_by_categories.Remove(i);
            }
            _context.users.Remove(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}