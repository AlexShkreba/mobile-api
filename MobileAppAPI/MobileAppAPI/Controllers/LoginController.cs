using System.Linq;
using Microsoft.AspNetCore.Mvc;
using MobileAppAPI.Models;

namespace MobileAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Context _context;

        public LoginController(Context context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Login(Users user)
        {
            IActionResult response = Unauthorized();
            var person = Authenticate(user);
            if (person != null)
            {
                response = Ok(person);
            }
            return response;
        }
        
        private Users Authenticate(Users user)
        {
            Users person = _context.users.FirstOrDefault(x => x.login == user.login && x.password == user.password);
            return person;
        }
    }
}