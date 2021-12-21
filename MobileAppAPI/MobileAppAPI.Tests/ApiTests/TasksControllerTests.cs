using MobileAppAPI.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MobileAppAPI.Tests.ApiTests
{
    class TasksControllerTests : TestBase
    {
        internal class Amount
        {
            public int Passive { get; set; }
            public int Active { get; set;}
        }
        internal class ReqCategories
        {
            public string Category { get; set; }
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
            Tasks task = new Tasks()
            {
                name = "task",
                priority = true,
                state = false,
                repetition = false,
                time_notification = null,
                date_finish = DateTime.Now,
            };
            Tasks taskPassive = new Tasks()
            {
                name = "tasssk",
                priority = true,
                state = true,
                repetition = false,
                time_notification = null
            };
            Tasks taskActive = new Tasks()
            {
                name = "tasssk",
                priority = true,
                state = false,
                repetition = false,
                time_notification = null
            };
            var reqTask = JsonContent.Create(task);
            var reqPassiveTask = JsonContent.Create(taskPassive);
            var reqtaskActive = JsonContent.Create(taskActive);
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };
            string categoryDo = "Do";
            string categoryYour = "Your";
            await Client.PostAsync("/api/Users/post", JsonContent.Create(reqUser));
            await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, allTasks.name));
            await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, highPriorities.name));
            await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, inSchedule.name));
            await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, categoryDo));
            await Client.GetAsync(string.Format("/api/categories/create/{0}/{1}", reqUser.login, categoryYour));
            await Client.PostAsync(string.Format("/api/tasks/create/{0}/{1}", reqUser.login, categoryYour), reqTask);
            await Client.PostAsync(string.Format("/api/tasks/create/{0}/{1}", reqUser.login, categoryDo), reqPassiveTask);
            await Client.PostAsync(string.Format("/api/tasks/create/{0}/{1}", reqUser.login, categoryDo), reqtaskActive);
        }

        #region Create task
        [Test]
        public async Task CreateTaskDone()
        {
            Tasks task = new Tasks()
            {
                name = "task dones",
                priority = true,
                state = true,
                repetition = false,
                time_notification = null,
                date_finish = DateTime.Now
            };
            var reqTask = JsonContent.Create(task);
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };

            var result = await Client.PostAsync(string.Format("/api/tasks/create/{0}/{1}", reqUser.login, "Dos"), reqTask);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }
        [Test]
        public async Task CreateTaskFialByTasksIsAlredyExsist()
        {
            Tasks task = new Tasks()
            {
                name = "task",
                priority = true,
                state = false,
                repetition = false,
                time_notification = null,
                date_finish = DateTime.Now
            };
            var reqTask = JsonContent.Create(task);
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };
            var result = await Client.PostAsync(string.Format("/api/tasks/create/{0}/{1}", reqUser.login, "Do"), reqTask);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        [Test]
        public async Task CreateTaskFailByUserIsNull()
        {
            Tasks task = new Tasks()
            {
                name = "task",
                priority = true,
                state = false,
                repetition = false,
                time_notification = null,
                date_finish = DateTime.Now
            };
            var reqTask = JsonContent.Create(task);
            var reqUser = new Users
            {
                login = "login1",
                password = "password"
            };
            var result = await Client.PostAsync(string.Format("/api/tasks/create/{0}/{1}", reqUser.login, "Do"), reqTask);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        [Test]
        public async Task CreateTaskFailByCategoryIsNull()
        {
            Tasks task = new Tasks()
            {
                name = "task",
                priority = true,
                state = false,
                repetition = false,
                time_notification = null,
                date_finish = DateTime.Now
            };
            var reqTask = JsonContent.Create(task);
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };
            var result = await Client.PostAsync(string.Format("/api/tasks/create/{0}/{1}", reqUser.login, "Doqq"), reqTask);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        [Test]
        public async Task CreateTaskFailByTaskIsNull()
        {
            var reqTask = JsonContent.Create("");
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };
            var result = await Client.PostAsync(string.Format("/api/tasks/create/{0}/{1}", reqUser.login, "Do"), reqTask);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        #endregion

        #region Delete task
        [Test]
        public async Task DeleteTaskDone()
        {
            var result = await Client.DeleteAsync("/api/tasks/delete/login/task dones");
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }
        [Test]
        public async Task DeleteUserNull()
        {
            var result = await Client.DeleteAsync("/api/tasks/delete/login1/task");
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        #endregion

        #region Get task/s
        [Test]
        public async Task GetTaskPassive()
        {
            var active_passive = new Amount()
            {
                Active = 0,
                Passive = 1,
            };

            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_active_passive/{0}/{1}", reqUser.login, "Do"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            Amount req_active_passive = null;

            Assert.DoesNotThrowAsync(async () =>
            {
                req_active_passive = JsonConvert.DeserializeObject<Amount>(await result.Content.ReadAsStringAsync());
            });
            Assert.AreEqual(req_active_passive.Active, active_passive.Active);
            Assert.AreEqual(req_active_passive.Passive, active_passive.Passive);
        }
        [Test]
        public async Task GetTaskActive()
        {
            var active_passive = new Amount()
            {
                Active = 1,
                Passive = 0,
            };

            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_active_passive/{0}/{1}", reqUser.login, "Your"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            Amount req_active_passive = null;

            Assert.DoesNotThrowAsync(async () =>
            {
                req_active_passive = JsonConvert.DeserializeObject<Amount>(await result.Content.ReadAsStringAsync());
            });
            Assert.AreEqual(req_active_passive.Active, active_passive.Active);
            Assert.AreEqual(req_active_passive.Passive, active_passive.Passive);
        }

        [Test]
        public async Task GetTaskActiveAndPassiveFail()
        {
            var active_passive = new Amount()
            {
                Active = 0,
                Passive = 1,
            };

            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_active_passive/{0}/{1}", reqUser.login, "All Tasks"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            Amount req_active_passive = null;

            Assert.DoesNotThrowAsync(async () =>
            {
                req_active_passive = JsonConvert.DeserializeObject<Amount>(await result.Content.ReadAsStringAsync());
            });
            Assert.AreNotEqual(req_active_passive.Active, active_passive.Active);
            Assert.AreEqual(req_active_passive.Passive, active_passive.Passive);
        }

        [Test]
        public async Task GetTaskActiveAndPassiveFailByUserIsNull()
        {
            var reqUser = new Users
            {
                login = "login1",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_active_passive/{0}/{1}", reqUser.login, "All Tasks"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetTaskActiveAndPassiveFailByCategoryrIsNull()
        {
            var reqUser = new Users
            {
                login = "login1",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_active_passive/{0}/{1}", reqUser.login, "All Task"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetTaskByCategoryDone()
        {
            Tasks[] tasks = {
                new Tasks
                {
                    name = "task",
                    priority = true,
                    state = false,
                    repetition = false,
                    time_notification = null,
                    date_finish = DateTime.Now
                },
                new Tasks
                {
                    name = "tasssk",
                    priority = true,
                    state = true,
                    repetition = false,
                    time_notification = null
                }
            };
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_tasks_by_category/{0}/{1}", reqUser.login, "All Tasks"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            Tasks[] req_tasks = null;

            Assert.DoesNotThrowAsync(async () =>
            {
                req_tasks = JsonConvert.DeserializeObject<Tasks[]>(await result.Content.ReadAsStringAsync());
            });
            Assert.AreEqual(req_tasks.Length, tasks.Length);
            for (int i = 0; i < tasks.Length; i++)
            {
                Assert.AreEqual(req_tasks[i].name, tasks[i].name);
            }

        }

        [Test]
        public async Task GetTaskByCategoryFailsByUserIsNull()
        {
            var reqUser = new Users
            {
                login = "login1",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_tasks_by_category/{0}/{1}", reqUser.login, "All Tasks"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetTaskByCategoryFailsByCategoryIsNull()
        {
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_tasks_by_category/{0}/{1}", reqUser.login, "All Task"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetTaskDone()
        {
            Tasks task = new Tasks
            {
                name = "task",
                priority = true,
                state = false,
                repetition = false,
                time_notification = null,
            };
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_task/{0}/{1}", reqUser.login, task.name));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            Tasks req_task = null;

            Assert.DoesNotThrowAsync(async () =>
            {
                req_task = JsonConvert.DeserializeObject<Tasks>(await result.Content.ReadAsStringAsync());
            });
            Assert.AreEqual(req_task.name, task.name);
            Assert.AreEqual(req_task.priority, task.priority);
            Assert.AreEqual(req_task.state, task.state);
            Assert.AreEqual(req_task.repetition, task.repetition);
            Assert.AreEqual(req_task.time_notification, task.time_notification);
        }

        [Test]
        public async Task GetTaskFailsByUserIsNull()
        {
            var reqUser = new Users
            {
                login = "login1",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_task/{0}/{1}", reqUser.login, "All Tasks"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetTaskFailsByUserNoContent()
        {
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_task/{0}/{1}", reqUser.login, "doo"));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.NoContent);
        }

        [Test]
        public async Task GetByCategoryDone()
        {
            Tasks task = new Tasks
            {
                name = "task",
                priority = true,
                state = false,
                repetition = false,
                time_notification = null,
            };
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_by_category/{0}/{1}", reqUser.login, task.name));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            ReqCategories category = null;

            Assert.DoesNotThrowAsync(async () =>
            {
                category = JsonConvert.DeserializeObject<ReqCategories>(await result.Content.ReadAsStringAsync());
            });
            Assert.AreEqual(category.Category, "Your");
        }

        [Test]
        public async Task GetByCategoryFailByUserIsNull()
        {
            Tasks task = new Tasks
            {
                name = "task",
                priority = true,
                state = false,
                repetition = false,
                time_notification = null,
            };
            var reqUser = new Users
            {
                login = "login1",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_by_category/{0}/{1}", reqUser.login, task.name));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task GetByCategoryFailByTaskIsNull()
        {
            Tasks task = new Tasks
            {
                name = "task1",
                priority = true,
                state = false,
                repetition = false,
                time_notification = null,
            };
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };

            var result = await Client.GetAsync(string.Format("/api/tasks/get_by_category/{0}/{1}", reqUser.login, task.name));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        #endregion

        #region Update task
        [Test]
        public async Task UpdateTaskDone()
        {
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };

            var task = await Client.GetAsync(string.Format("/api/tasks/get_task/{0}/{1}", reqUser.login, "task"));
            Tasks reqTasks = null;

            Assert.DoesNotThrowAsync(async () =>
            {
                reqTasks = JsonConvert.DeserializeObject<Tasks>(await task.Content.ReadAsStringAsync());
            });

            var result = await Client.PutAsync(string.Format("/api/tasks/put/{0}/{1}", reqUser.login, "Do"), JsonContent.Create(reqTasks));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }
        [Test]
        public async Task UpdateTaskWithCategoryDone()
        {
            Tasks task = new Tasks()
            {
                name = "taskss",
                priority = true,
                state = false,
                repetition = false,
                time_notification = null
            };
            var reqTask = JsonContent.Create(task);
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };

            var result = await Client.PostAsync(string.Format("/api/tasks/create/{0}/{1}", reqUser.login, "Do"), reqTask);
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);

            result = await Client.GetAsync(string.Format("/api/tasks/get_task/{0}/{1}", reqUser.login, "taskss"));
            
            Tasks reqTasks = null;

            Assert.DoesNotThrowAsync(async () =>
            {
                reqTasks = JsonConvert.DeserializeObject<Tasks>(await result.Content.ReadAsStringAsync());
            });
            result = await Client.PutAsync(string.Format("/api/tasks/put/{0}/{1}", reqUser.login, "Your"), JsonContent.Create(reqTasks));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.OK);
        }


        [Test]
        public async Task UpdateTaskFailByTaskIsNull()
        {
            Tasks task = new Tasks()
            {
                name = "task",
                priority = true,
                state = false,
                repetition = false,
                time_notification = null,
                date_finish = DateTime.Now
            };

            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };
            ;
            var result = await Client.PutAsync(string.Format("/api/tasks/put/{0}/{1}", reqUser.login, "Do"), JsonContent.Create(task));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateTaskFailByUserIsNull()
        {
            var reqUser = new Users
            {
                login = "login1",
                password = "password"
            };
            ;
            var task = await Client.GetAsync(string.Format("/api/tasks/get_task/{0}/{1}", reqUser.login, "task"));
            Tasks reqTasks = null;

            Assert.DoesNotThrowAsync(async () =>
            {
                reqTasks = JsonConvert.DeserializeObject<Tasks>(await task.Content.ReadAsStringAsync());
            });

            reqTasks.name = "Doo";
            var result = await Client.PutAsync(string.Format("/api/tasks/put/{0}/{1}", reqUser.login, "Do"), JsonContent.Create(reqTasks));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }

        [Test]
        public async Task UpdateTaskFailByCategoryIsNull()
        {
            var reqUser = new Users
            {
                login = "login",
                password = "password"
            };
            ;
            var task = await Client.GetAsync(string.Format("/api/tasks/get_task/{0}/{1}", reqUser.login, "task"));
            Tasks reqTasks = null;

            Assert.DoesNotThrowAsync(async () =>
            {
                reqTasks = JsonConvert.DeserializeObject<Tasks>(await task.Content.ReadAsStringAsync());
            });

            reqTasks.name = "Doo";
            var result = await Client.PutAsync(string.Format("/api/tasks/put/{0}/{1}", reqUser.login, "Doo"), JsonContent.Create(reqTasks));
            Assert.AreEqual(result.StatusCode, HttpStatusCode.BadRequest);
        }
        #endregion
    }
}
