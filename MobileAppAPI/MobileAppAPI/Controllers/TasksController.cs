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
    public class TasksController : ControllerBase
    {
        private readonly Context _context;

        public TasksController(Context context)
        {
            _context = context;
        }

        [HttpGet("get_active_passive/{login}/{category_name}")]
        public async Task<ActionResult> get_active_passive(string login, string category_name)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.login == login);
            var category = await _context.categories.FirstOrDefaultAsync(x => x.name == category_name);
            if (user == null || category == null)
                return BadRequest();

            var tasks_by_categories_list = await _context.tasks_by_categories.Where(x => x.id_user == user.id && x.id_category == category.id).ToListAsync();
            int active = 0, passive = 0;

            foreach (var i in tasks_by_categories_list)
            {
                var tmp = await _context.tasks.FirstOrDefaultAsync(x => x.id == i.id_task);
                if (tmp != null)
                {
                    if (tmp.state == true)
                    {
                        passive++;
                    }
                    else { active++; }
                }
            }
            return Ok(new { active = active, passive = passive });
        }

        [HttpGet("get_tasks_by_category/{login}/{category_name}")]
        public async Task<ActionResult> get_tasks_by_category(string login, string category_name)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.login == login);
            var category = await _context.categories.FirstOrDefaultAsync(x => x.name == category_name);
            if (user == null || category == null)
                return BadRequest();

            var tasks_by_categories_list = await _context.tasks_by_categories.Where(x => x.id_user == user.id && x.id_category == category.id).ToListAsync();
            var tasks = new ObservableCollection<Tasks>();

            foreach (var i in tasks_by_categories_list)
            {
                tasks.Add(await _context.tasks.FirstOrDefaultAsync(x => x.id == i.id_task));
            }
            return Ok(tasks);
        }

        [HttpGet("get_task/{login}/{task_name}")]
        public async Task<ActionResult> get_task(string login, string task_name)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.login == login);
            if (user == null || string.IsNullOrEmpty(task_name))
                return BadRequest();
            var task = await _context.tasks.FirstOrDefaultAsync(x => x.name == task_name);
            return Ok(task);
        }

        [HttpGet("get_by_category/{login}/{task_name}")]
        public async Task<ActionResult> get_by_category(string login, string task_name)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.login == login);
            if (user == null || string.IsNullOrEmpty(task_name))
                return BadRequest();
            var task = await _context.tasks.FirstOrDefaultAsync(x => x.name == task_name);
            if (task == null)
                return BadRequest();
            var temp = await _context.tasks_by_categories.Where(x => x.id_user == user.id && x.id_task == task.id).ToListAsync();
            string result = string.Empty;
            foreach(var i in temp)
            {
                var tmp = await _context.categories.FirstOrDefaultAsync(x => x.id == i.id_category);
                if (tmp.type == "Users")
                {
                    result = tmp.name; break;
                }
            }
            return Ok(new { category = result });
        }

        [HttpPost("create/{login}/{category_name}")]
        public async Task<ActionResult> create(string login, string category_name, Tasks task)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.login == login);
            var category = await _context.categories.FirstOrDefaultAsync(x => x.name == category_name);
            if (task == null || user == null || category == null || _context.tasks.Any(x => x.name == task.name))
            {
                return BadRequest();
            }
            _context.tasks.Add(task);
            await _context.SaveChangesAsync();
            var tmp_task = await _context.tasks.FirstOrDefaultAsync(x => x.name == task.name);
            var all = await _context.categories.FirstOrDefaultAsync(x => x.name == "All Tasks");
            var tmp_tbc_all = await _context.tasks_by_categories.FirstOrDefaultAsync(x => x.id_user == user.id && x.id_category == all.id && x.id_task == null);
            if (tmp_tbc_all == null)
            {
                _context.tasks_by_categories.Add(new Tasks_By_Categories { id_user = user.id, id_category = all.id, id_task = tmp_task.id });
                await _context.SaveChangesAsync();
            }
            else
            {
                tmp_tbc_all.id_task = tmp_task.id;
                _context.Update(tmp_tbc_all);
                await _context.SaveChangesAsync();
            }
            var tmp_tbc = await _context.tasks_by_categories.FirstOrDefaultAsync(x => x.id_user == user.id && x.id_category == category.id && x.id_task == null);
            if (tmp_tbc == null)
            {
                _context.tasks_by_categories.Add(new Tasks_By_Categories { id_user = user.id, id_category = category.id, id_task = tmp_task.id });
                await _context.SaveChangesAsync();
            }
            else
            {
                tmp_tbc.id_task = tmp_task.id;
                _context.Update(tmp_tbc);
                await _context.SaveChangesAsync();
            }
            if (task.priority)
            {
                var pr = await _context.categories.FirstOrDefaultAsync(x => x.name == "High Priorities");
                var tmp_tbc_pr = await _context.tasks_by_categories.FirstOrDefaultAsync(x => x.id_user == user.id && x.id_category == pr.id && x.id_task == null);
                if (tmp_tbc_pr == null)
                {
                    _context.tasks_by_categories.Add(new Tasks_By_Categories { id_user = user.id, id_category = pr.id, id_task = tmp_task.id });
                    await _context.SaveChangesAsync();
                }
                else
                {
                    tmp_tbc_pr.id_task = tmp_task.id;
                    _context.Update(tmp_tbc_pr);
                    await _context.SaveChangesAsync();
                }
            }
            if (task.date_finish != null)
            {
                var sch = await _context.categories.FirstOrDefaultAsync(x => x.name == "In Schedule");
                var tmp_tbc_date = await _context.tasks_by_categories.FirstOrDefaultAsync(x => x.id_user == user.id && x.id_category == sch.id && x.id_task == null);
                if (tmp_tbc_date == null)
                {
                    _context.tasks_by_categories.Add(new Tasks_By_Categories { id_user = user.id, id_category = sch.id, id_task = tmp_task.id });
                    await _context.SaveChangesAsync();
                }
                else
                {
                    tmp_tbc_date.id_task = tmp_task.id;
                    _context.Update(tmp_tbc_date);
                    await _context.SaveChangesAsync();
                }
            }
            await _context.SaveChangesAsync();
            return Ok(tmp_task);
        }

        [HttpDelete("delete/{login}/{task}")]
        public async Task<ActionResult>delete(string login, string task)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(task))
            {
                return BadRequest();
            }
            else
            {
                var user = await _context.users.FirstOrDefaultAsync(x => x.login == login);
                var tasks = await _context.tasks.FirstOrDefaultAsync(x => x.name == task);
                if (user == null || tasks == null)
                {
                    return BadRequest();
                }
                else
                {
                    var tmp = await _context.tasks_by_categories.Where(x => x.id_user == user.id && x.id_task == tasks.id).ToListAsync();
                    int cat_id = 0;
                    foreach (var i in tmp)
                    {
                        cat_id = i.id_category;
                        _context.tasks_by_categories.Remove(i);
                        await _context.SaveChangesAsync();
                        if (!_context.tasks_by_categories.Any(x => x.id_user == user.id && x.id_category == cat_id))
                        {
                            _context.tasks_by_categories.Add(new Tasks_By_Categories { id_user = user.id, id_category = cat_id, id_task = null });
                            await _context.SaveChangesAsync();
                        }
                    }
                    _context.tasks.Remove(tasks);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
            }
        }

        [HttpPut("put/{login}/{category_name}")]
        public async Task<ActionResult> put(string login, string category_name, Tasks task)
        {
            var user = await _context.users.FirstOrDefaultAsync(x => x.login == login);
            var category = await _context.categories.FirstOrDefaultAsync(x => x.name == category_name);
            if (task == null || user == null || category == null || !_context.tasks.Any(x => x.id == task.id))
            {
                return BadRequest();
            }
            var tasks = await _context.tasks_by_categories.Where(x => x.id_user == user.id && x.id_task == task.id).ToListAsync();
            foreach (var i in tasks)
            {
                var tmp = await _context.categories.FirstOrDefaultAsync(x => x.id == i.id_category);
                if (tmp.type == "Users")
                {
                    int ID = i.id_category;
                    i.id_category = category.id;
                    _context.Update(i);
                    await _context.SaveChangesAsync();
                    if (_context.tasks_by_categories.Any(x => x.id_user == user.id && x.id_category == category.id && x.id_task == null))
                    {
                        _context.Remove(await _context.tasks_by_categories.FirstOrDefaultAsync(x => x.id_user == user.id && x.id_category == category.id && x.id_task == null));
                        await _context.SaveChangesAsync();
                    }
                    if (!_context.tasks_by_categories.Any(x => x.id_user == user.id && x.id_category == ID))
                    {
                        _context.tasks_by_categories.Add(new Tasks_By_Categories { id_user = user.id, id_category = ID, id_task = null });
                        await _context.SaveChangesAsync();
                    }
                    break;
                }
            }
            _context.Update(task);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}