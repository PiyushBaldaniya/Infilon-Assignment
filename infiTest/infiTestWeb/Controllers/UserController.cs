using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace infiTestWeb.Controllers
{
    public class UserController : Controller
    {
        public static IConfiguration _config;
        public static string BaseUrl;
        public UserController(IConfiguration config)
        {
            _config = config;
            BaseUrl = _config.GetSection("LocalApiUrl")["baseUrl"];
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetTable(int tableId)
        {
            string str = BaseUrl+ "User/GetUserFromTable/"+tableId;
            RestClient res = new RestClient(str);

            RestRequest request = new RestRequest();

            RestResponse response = res.Execute(request);
            return Json(new { data = response.Content});
        }

        [HttpPost]
        public JsonResult EditUser(int id,string title,bool completed)
        {
            string str = BaseUrl + "User/EditUser/";
            RestClient res = new RestClient(str);

            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            request.AddParameter("id", id);
            request.AddParameter("title", title) ;
            request.AddParameter("completed", completed);
            RestResponse response = res.Execute(request);
            return Json(new { data = response.Content });
        }

        [HttpPost]
        public void DeleteUser(int id)
        {
            string str = BaseUrl + "User/DeleteUser/";
            RestClient res = new RestClient(str);

            RestRequest request = new RestRequest();
            request.Method = Method.Post;
            request.AddParameter("id", id);
            RestResponse response = res.Execute(request);
        }

        public ActionResult OddUser()
        {
            return View();
        }


        public ActionResult EvenUser()
        {
            return View();
        }

        public ActionResult HistoryUser()
        {
            return View();
        }

    }
}
