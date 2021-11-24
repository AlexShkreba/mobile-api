using Microsoft.EntityFrameworkCore;
using MobileAppAPI.Models;

namespace MobileAppAPI.Tests.Unit
{
    public abstract class DbTestsBase
    {
        protected readonly Context DbContext;
        protected DbTestsBase()
        {
            DbContext = new Context(
                new DbContextOptionsBuilder<Context>()
                    .UseInMemoryDatabase("MobileAppApi")
                    .Options
            );
        }
    }
}
