using MobileAppAPI.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MobileAppAPI.Tests.ApiTests
{
    class CategoriesControllerTests : TestBase
    {
        internal class ReqCategory
        {
            public int Id { get; set; }
        }
        [SetUp]
        public async Task Setup()
        {
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
            };
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };
            await Client.PostAsync("/api/Users/post", JsonContent.Create(reqUser));
            await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, allTasks.name));
            await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, highPriorities.name));
            await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, inSchedule.name));

        }

        #region Create Category
        [Test]
        public async Task CreateCategoryDone()
        {
            var reqCategory = new Categories
            {
                name = "Dos"
            };
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };
            var result = await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, reqCategory.name));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }

        [Test]
        public async Task CreateCategoryFailByUserNotFound()
        {
            var reqUser = new Users
            {
                login = "login1",
                password = "password"
            };
            var reqCategory = new Categories
            {
                name = "Do"
            };
            var result = await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, reqCategory.name));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task CreateCategoryFailByCategoryAlreadyExist()
        {
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };
            var reqCategory = new Categories
            {
                name = "All Tasks"
            };
            var result = await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, reqCategory.name));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        #endregion

        #region Update Category
        [Test]
        public async Task UpdateCategoryDone()
        {
            var category = new Categories
            {
                name = "Dos not update"
            };
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };
            var result = await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, category.name));
            ReqCategory reqCategory = null;
            Assert.DoesNotThrowAsync(async () =>
            {
                reqCategory = JsonConvert.DeserializeObject<ReqCategory>(await result.Content.ReadAsStringAsync());
            });
            category.id = reqCategory.Id;
            category.name = "Dos update";
            result = await Client.PutAsync(string.Format("/api/categories/put"), JsonContent.Create(category));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }
        [Test]
        public async Task UpdateCategoryFailByCategoryIsNull()
        {
            var result = await Client.PutAsync(string.Format("/api/categories/put"), JsonContent.Create(""));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        [Test]
        public async Task UpdateCategoryFailByCategoryIdNotFound()
        {
            Categories categories = new Categories
            {
                id = -1,
            };
            var result = await Client.PutAsync(string.Format("/api/categories/put"), JsonContent.Create(categories));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        #endregion

        #region Delete Category
        [Test]
        public async Task DeleteCategory()
        {
            var reqCategory = new Categories
            {
                name = "Dos delete"
            };
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };
            var result = await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, reqCategory.name));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            result = await Client.DeleteAsync(string.Format("/api/categories/delete/{0}/{1}", "login", reqCategory.name));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }
        [Test]
        public async Task DeleteCategoryFailByUserIsNull()
        {
            var result = await Client.DeleteAsync(string.Format("/api/categories/delete/{0}/{1}", "login1", "q"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        [Test]
        public async Task DeleteCategoryFailByCategoryrIsNull()
        {
            var result = await Client.DeleteAsync(string.Format("/api/categories/delete/{0}/{1}", "login", "q"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        #endregion

        #region Get Category
        [Test]
        public async Task GetCategoryByNameDone()
        {
            var result = await Client.GetAsync(string.Format("/api/categories/get_name/{0}", "All Tasks"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }
        [Test]
        public async Task GetCategoryByNameFailByCategoryNotFound()
        {
            var result = await Client.GetAsync(string.Format("/api/categories/get_name/{0}", "All"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetTypeDone()
        {
            ObservableCollection<Categories> categories = new ObservableCollection<Categories>()
            {
                new Categories()
                {
                    name = "All Tasks"
                },
                new Categories()
                {
                    name = "High Priorities"
                },
                new Categories()
                {
                    name = "In Schedule"
                }
            };
            var result = await Client.GetAsync(string.Format("/api/categories/get_type/{0}/{1}", "login", "Standard"));
            ObservableCollection<Categories> reqCategories = null;
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
            Assert.DoesNotThrowAsync(async () =>
                {
                    reqCategories = JsonConvert.DeserializeObject<ObservableCollection<Categories>>(await result.Content.ReadAsStringAsync());
            });
            Assert.AreEqual(reqCategories.Count, categories.Count);
            for(int i = 0;i< reqCategories.Count; i++)
            {
                Assert.AreEqual(reqCategories[i].name, categories[i].name);
            }
        }
        [Test]
        public async Task GetTypeFailByUserIsNull()
        {            
            var result = await Client.GetAsync(string.Format("/api/categories/get_type/{0}/{1}", "login1", "Standard"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        #endregion
    }
}
