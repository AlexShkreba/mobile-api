using MobileAppAPI.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MobileAppAPI.Tests.ApiTests
{
    class CategoriesControllerTests : TestBase
    {
        [SetUp]
        public async Task Setup()
        {
            var reqUser = JsonContent.Create(new Tasks_By_Categories
            {
                
            });
            await Client.PostAsync("/api/Users/post", reqUser);
        }

        #region Create user
       
        #endregion

        #region Delete user
        [Test]
        public async Task DeleteUser()
        {
           
            //Assert.AreEqual();
        }
        [Test]
        public async Task DeleteUserNull()
        {
            //Assert.AreEqual();
        }
        #endregion
    }
}
