using InfiTestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfiTestAPI.Services.Interface
{
    public interface IUser
    {
        void LoadData();
        List<UserModel> GetUserFromTable(int tableId);
        List<UserModel> EditUser(int id, string title, bool completed);
        void DeleteUser(int id);
        UserModelApi AddandEditUser(int id, int userid, string title, bool completed);
    }
}
