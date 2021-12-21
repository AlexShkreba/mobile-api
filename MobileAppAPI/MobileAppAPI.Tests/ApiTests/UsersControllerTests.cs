using System;
using System.Net;
using NUnit.Framework;
using MobileAppAPI.Controllers;
using MobileAppAPI.Models;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MobileAppAPI.Tests.ApiTests
{
    class UsersControllerTests : TestBase
    {
        [SetUp]
        public async Task Setup()
        {
            var User = new Users
            {
                login = "login",
                password = "password"
            };
            var reqUser = JsonContent.Create(User);
            await Client.PostAsync("/api/Users/post", reqUser);
            Categories allTasks = new Categories
            {
                name = "All Tasks"
            };
            Categories highPriorities = new Categories
            {
                name = "High Priorities"
            };
            Categories inSchedule = new Categories
            {
                name = "In Schedule"
            };;
            await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", User.login, allTasks.name));
            await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", User.login, highPriorities.name));
            await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", User.login, inSchedule.name));
        }

        #region Create user
        
        [Test]
        public async Task CreateUser()
        {
            Users user = new Users()
            {
                login = "log",
                password = "pass"
            };
            var reqUser = JsonContent.Create(user);
            var result = await Client.PostAsync("/api/Users/post", reqUser);

            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            Users response = null;

            Assert.DoesNotThrowAsync(async () =>
            {
                response = JsonConvert.DeserializeObject<Users>(await result.Content.ReadAsStringAsync());
            });
            Assert.AreEqual(response.login, user.login);
            Assert.AreEqual(response.password, user.password);
        }
        [Test]
        public async Task CreateUserAlredyExists()
        {
            Users user = new Users()
            {
                login = "login",
                password = "password"
            };
            var reqUser = JsonContent.Create(user);
            var result = await Client.PostAsync("/api/Users/post", reqUser);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        [Test]
        public async Task CreateUserIsNull()
        {
            var test = JsonContent.Create("");
            var result = await Client.PostAsync("/api/Users/post", JsonContent.Create<Users>(new Users()));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        #endregion

        #region Delete user
        [Test]
        public async Task DeleteUser()
        {
            var result = await Client.DeleteAsync("/api/Users/delete/login");
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }
        [Test]
        public async Task DeleteUserNull()
        {
            var result = await Client.DeleteAsync("/api/Users/delete/user");
            Assert.AreEqual(result.StatusCode, HttpStatusCode.NotFound);
        }
        #endregion
    }
}
