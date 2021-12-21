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
    class LoginControllerTests : TestBase
    {
        [SetUp]
        public async Task Setup()
        {
            var reqUser = JsonContent.Create(new Users
            {
                login = "login",
                password = "password"
            });
            await Client.PostAsync("/api/Users/post", reqUser);
        }

        #region Login
        [Test]
        public async Task LoginDone()
        {
            Users user = new Users()
            {
                login = "login",
                password = "password"
            };
            var reqUser = JsonContent.Create(user);
            var result = await Client.PostAsync("/api/login/", reqUser);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            Users response = null;
            Assert.DoesNotThrowAsync(async () =>
            {
                response = JsonConvert.DeserializeObject<Users>(await result.Content.ReadAsStringAsync());
            });
            Assert.AreEqual(response.login, user.login);
            Assert.AreEqual(response.password, user.password);
            Assert.NotNull(response);
        }
        [Test]
        public async Task LoginFail()
        {
            Users user = new Users()
            {
                login = "log",
                password = "pass"
            };
            var reqUser = JsonContent.Create(user);
            var result = await Client.PostAsync("/api/login/", reqUser);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.Unauthorized);
        }
        #endregion

    }
}
