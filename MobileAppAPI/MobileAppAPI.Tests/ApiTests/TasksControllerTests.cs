using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MobileAppAPI.Tests.ApiTests
{
    class TasksControllerTests : TestBase
    {
        [SetUp]
        public async Task Setup()
        {

        }

        #region Create task
        public async Task CreateTaskDone()
        {

            //Assert.AreEqual();
        }
        public async Task CreateTaskFailByUserIsNull()
        {

            //Assert.AreEqual();
        }
        public async Task CreateTaskFailByCategoryIsNull()
        {

            //Assert.AreEqual();
        }
        public async Task CreateTaskFailByTaskIsNull()
        {

            //Assert.AreEqual();
        }
        #endregion

        #region Delete task
        [Test]
        public async Task DeleteTaskDone()
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
