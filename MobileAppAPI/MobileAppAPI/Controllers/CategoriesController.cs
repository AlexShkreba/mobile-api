using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MobileAppAPI.Models;

namespace MobileAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly Context _context;

        public CategoriesController(Context context)
        {
            _context = context;
        }

        [HttpGet("get_type/{login}/{type}")]
        public async Task<ActionResult> get_type(string login, string type)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.login == login);
            if (user == null)
                return BadRequest();

            var tasks_by_categories_list = await _context.tasks_by_categories.Where(x => x.id_user == user.id).ToListAsync();
            var result = new ObservableCollection<Categories>();

            foreach (var i in tasks_by_categories_list)
            {
                var tmp = await _context.categories.FirstOrDefaultAsync(x => x.id == i.id_category);
                if (tmp.type == type && !result.Contains(tmp))
                {
                    result.Add(tmp);
                }
            }
            return Ok(result);
        }

        [HttpGet("get_name/{name}")]
        public async Task<ActionResult> get_name(string name)
        {
            var category = await _context.categories.FirstOrDefaultAsync(x => x.name == name);
            if (category == null)
            {
                return BadRequest();
            }
            return Ok(new { id = category.id });
        }

        [HttpPut("put")]
        public async Task<ActionResult> put(Categories category)
        {
            if (category.name == null || !_context.categories.Any(x => x.id == category.id))
            {
                return BadRequest();
            }
            _context.Update(category);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("create/{login}/{category}")]
        public async Task<ActionResult> create(string login, string category)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.login == login);
            if (user == null || _context.categories.Any(x => x.name == category))
            {
                return BadRequest();
            }
            else
            {
                if (category == "All Tasks" || category == "High Priorities" || category == "In Schedule")
                {
                    _context.categories.Add(new Categories { name = category, type = "Standard" });
                }
                else
                {
                    _context.categories.Add(new Categories { name = category, type = "Users" });
                }
                await _context.SaveChangesAsync();
                var tmp = await _context.categories.FirstOrDefaultAsync(x => x.name == category);
                _context.tasks_by_categories.Add(new Tasks_By_Categories { id_user = user.id, id_category = tmp.id, id_task = null });
                await _context.SaveChangesAsync();
                return Ok(tmp);
            }
            
        }

        [HttpDelete("delete/{login}/{category}")]
        public async Task<ActionResult> delete(string login, string category)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.login == login);
            var categories = await _context.categories.FirstOrDefaultAsync(x => x.name == category);
            if (user == null || categories == null)
            {
                return BadRequest();
            }
            else
            {
                var tmp = _context.tasks_by_categories.Where(x => x.id_user == user.id && x.id_category == categories.id);
                foreach (var i in tmp)
                {
                    _context.tasks_by_categories.Remove(i);
                }
                await _context.SaveChangesAsync();
                _context.categories.Remove(categories);
                await _context.SaveChangesAsync();
                return Ok();
            }
        }
    }
}