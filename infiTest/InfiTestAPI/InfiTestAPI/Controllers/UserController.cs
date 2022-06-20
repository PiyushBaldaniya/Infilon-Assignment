using InfiTestAPI.Models;
using InfiTestAPI.Services.Interface;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiTestAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUser _UserService;

        public UserController(IUser UserService)
        {
            _UserService = UserService;
        }

        [HttpGet]
        [Route("LoadDataOfUser")]
        public void LoadDataOfUser()
        {
            _UserService.LoadData();
        }

        [HttpGet]
        [Route("GetUserFromTable/{tableId}")]
        public List<UserModel> GetUserFromTable()
        {
            int tableId =Convert.ToInt32(Request.Path.ToString().Split("/").Last());

            return _UserService.GetUserFromTable(tableId);
        }


        [HttpPost]
        [Route("EditUser")]
        public List<UserModel> EditUser()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            bool completed = Convert.ToBoolean(Request.Form["completed"]);

            return _UserService.EditUser(id, Request.Form["title"], completed);
        }

        [HttpPost]
        [Route("DeleteUser")]
        public void DeleteUser()
        {
            int id = Convert.ToInt32(Request.Form["id"]);

            _UserService.DeleteUser(id);
        }

        [HttpPost]
        [Route("AddandEditUser")]
        public UserModelApi AddandEditUser()
        {
            int id = Convert.ToInt32(Request.Form["id"]);
            int userid = Convert.ToInt32(Request.Form["userid"]);
            bool completed = Convert.ToBoolean(Request.Form["completed"]);

            return _UserService.AddandEditUser(id,userid, Request.Form["title"], completed);
        }
    }
}
