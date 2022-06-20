using InfiTestAPI.Models;
using InfiTestAPI.Services.Interface;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InfiTestAPI.Services.Implementation
{
    public class User:IUser
    {
        public static IConfiguration _config;
        public static string conStr; 
        public User(IConfiguration config)
        {
            _config = config;
            conStr = _config.GetSection("ConnectionStrings")["conStr"];
        }
        public void LoadData()
        {
            string str = _config.GetSection("ApiUrl")["UrlName"];
            RestClient res = new RestClient(str);

            RestRequest request = new RestRequest();

            RestResponse response = res.Execute(request);
            
            List<UserModel> users = new List<UserModel>();
            users = Newtonsoft.Json.JsonConvert.DeserializeObject<List<UserModel>>(response.Content);

            using (SqlConnection cn = new SqlConnection(conStr))
            {
                cn.Open();
                foreach (var user in users)
                {
                    try
                    {
                        string queryOdd = "INSERT INTO dbo.OddUser (userId, id, title, completed, edited) " +
                        "VALUES (@userId, @id, @title, @completed, @edited) ";

                        string queryEven = "INSERT INTO dbo.EvenUser (userId, id, title, completed, edited) " +
                          "VALUES (@userId, @id, @title, @completed, @edited) ";

                        SqlCommand cmd = new SqlCommand((user.id % 2 == 0) ? queryEven : queryOdd, cn);
                        // define parameters and their values
                        cmd.Parameters.Add("@userId", SqlDbType.Int, 50).Value = user.userId;
                        cmd.Parameters.Add("@id", SqlDbType.Int, 50).Value = user.id;
                        cmd.Parameters.Add("@title", SqlDbType.NVarChar, 50).Value = user.title;
                        cmd.Parameters.Add("@completed", SqlDbType.Bit, 50).Value = user.completed;
                        cmd.Parameters.Add("@edited", SqlDbType.Bit).Value = user.edited;
                        cmd.ExecuteNonQuery();

                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                   
                }

                cn.Close();
            }

        }

        public List<UserModel> GetUserFromTable(int tableId)
        {
            List<UserModel> users = new List<UserModel>();
            using (SqlConnection cn = new SqlConnection(conStr))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("GetTableData" , cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    // define parameters and their values
                    cmd.Parameters.Add("@TableId", SqlDbType.Int, 50).Value = tableId;
                    SqlDataReader reader =  cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        users.Add(new UserModel()
                        {
                            userId = reader.GetFieldValue<int>(0),
                            id = reader.GetFieldValue<int>(1),
                            title = reader.GetFieldValue<string>(2),
                            completed = reader.GetFieldValue<bool>(3),
                            edited = reader.GetFieldValue<bool>(4)
                        });

                    }
                    return users;
                }
                catch (Exception e)
                {

                    return new List<UserModel>();
                }

            }
        }

        public List<UserModel> EditUser(int id,string title,bool completed)
        {
            List<UserModel> users = new List<UserModel>();
            using (SqlConnection cn = new SqlConnection(conStr))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("EditData", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    // define parameters and their values
                    cmd.Parameters.Add("@id", SqlDbType.Int, 50).Value = id;
                    cmd.Parameters.Add("@Tableid", SqlDbType.Int, 50).Value = (id % 2 == 0) ? 2 : 1;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value = title;
                    cmd.Parameters.Add("@completed", SqlDbType.Int, 50).Value = completed;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        users.Add(new UserModel()
                        {
                            userId = reader.GetFieldValue<int>(0),
                            id = reader.GetFieldValue<int>(1),
                            title = reader.GetFieldValue<string>(2),
                            completed = reader.GetFieldValue<bool>(3),
                            edited = reader.GetFieldValue<bool>(4)
                        });

                    }
                    return users;
                }
                catch (Exception e)
                {

                    return new List<UserModel>();
                }

            }
        }

        public void DeleteUser(int id)
        {
            using (SqlConnection cn = new SqlConnection(conStr))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("DeleteUser", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    // define parameters and their values
                    cmd.Parameters.Add("@id", SqlDbType.Int, 50).Value = id;
                    cmd.Parameters.Add("@Tableid", SqlDbType.Int, 50).Value = (id % 2 == 0) ? 2 : 1;
                    int i = cmd.ExecuteNonQuery();

                    
                }
                catch (Exception e)
                {
                }

            }
        }

        public UserModelApi AddandEditUser(int id,int userid, string title, bool completed)
        {
            UserModelApi user = new UserModelApi();
            using (SqlConnection cn = new SqlConnection(conStr))
            {
                cn.Open();
                try
                {
                    SqlCommand cmd = new SqlCommand("AddandEditData", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    // define parameters and their values
                    cmd.Parameters.Add("@id", SqlDbType.Int, 50).Value = id;
                    cmd.Parameters.Add("@Userid", SqlDbType.Int, 50).Value = userid;
                    cmd.Parameters.Add("@Tableid", SqlDbType.Int, 50).Value = (id % 2 == 0) ? 2 : 1;
                    cmd.Parameters.Add("@Title", SqlDbType.NVarChar, 200).Value = title;
                    cmd.Parameters.Add("@completed", SqlDbType.Int, 50).Value = completed;
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        user = new UserModelApi()
                        {
                            userId = reader.GetFieldValue<int>(1),
                            id = reader.GetFieldValue<int>(0),
                            title = reader.GetFieldValue<string>(2),
                            completed = reader.GetFieldValue<bool>(3),
                            edited = reader.GetFieldValue<bool>(4),
                            edited_via_api = reader.GetFieldValue<bool>(5)
                        };

                    }
                    return user;
                }
                catch (Exception e)
                {

                    return new UserModelApi();
                }

            }
        }

    }
}
