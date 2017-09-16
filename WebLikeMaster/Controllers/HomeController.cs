using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using VkApi;
using VkApi.Services;
using VkNet.Enums.Filters;

namespace WebLikeMaster.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {

        List<User> users = new List<User>();
        ulong _applicationId = 5212368;
        public HomeController()
        {
            VkApi.VkApi api = new VkApi.VkApi((int)_applicationId, "messages");
            //api.Authorization("+79687361740", ",eheyler"); //Моя авторизация перестала работать
            VkNet.VkApi net = new VkNet.VkApi();
            net.Authorize(new VkNet.ApiAuthParams //Авторизация работает, но апи слабое
            {
                Login = "+79254306274",
                ApplicationId = _applicationId,
                Password = "Maximurz1k",
                Settings = Settings.All
            });
            VkApi.VkApi.Token = net.Token; //А токен пригодится :)

            int membersCount = GroupService.GetMembersCount(27794994);

            for (int i = 0; membersCount > 0; membersCount -= 100, i++)
            {
                int count = membersCount >= 100 ? 100 : membersCount;
                IEnumerable<User> currentUsers = GroupService.GetMembers(27794994, (0 + i * 100), count, 10);
                currentUsers = currentUsers.Where(u => u.sex == Sex.Female && (u.relation != Relation.HaveFriend || u.relation != Relation.Engaged || u.relation != Relation.Married || u.relation != Relation.Love) && u.deactivated == null);
                users.AddRange(currentUsers);
                Console.WriteLine($"Добавлено {currentUsers.Count()} пользователей. Осталось {membersCount} пользователей. Осталось {TimeSpan.FromSeconds(membersCount / 200)}");
            }
        }
        public ActionResult Index()
        {
            return View();
        }
    }
}
