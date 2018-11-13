using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CADWeb.SQL
{
    public class SQLQuery
    {
        SqlConnection conn = SQLConnect.GetConnection();

        /// <summary>
        /// 查询数据库判断用户是否存在，同时返回用户状态，以中杠符分隔"|"
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns></returns>
        public string IsUserExist(string userName, string userPassword)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select * from userInfo", conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (userName.Equals(reader["userName"]) && userPassword.Equals(reader["userPassword"]))
                    {
                        string userState = reader["userState"].ToString();
                        reader.Close();
                        return "true" + "|" + userState;
                    }
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return "false" + "|-1";
        }

        /// <summary>
        /// 根据数据库查询用户类型
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public string QueryUserType(string userName)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("select userType from userInfo where userName=@temp", conn);
            cmd.Parameters.AddWithValue("@temp", userName);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return reader["userType"].ToString();
                }
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
            return null;
        }

        public List<UserInfo> UserQuery(int queryNum)
        {
            List<UserInfo> queryList = new List<UserInfo>();
            switch(queryNum)
            {
                case 1:
                    
                    break;
                case 2:
                    QueryUserInfo("Admin", out queryList);
                    break;
                case 3:
                    QueryUserInfo("Teacher", out queryList);
                    break;
                case 4:
                    QueryUserInfo("Student", out queryList);
                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:

                    break;
            }
            return queryList;
        }

        private void QueryUserInfo(string userType, out List<UserInfo> queryList)
        {
            queryList = new List<UserInfo>();
            conn.Open();
            string sql = "select * from userInfo where userType='" + userType + "'";
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    UserInfo userInfo = new UserInfo();
                    userInfo.UserName = reader["userName"].ToString();
                    userInfo.UserPassword = reader["UserPassword"].ToString();
                    userInfo.UserType = userType;
                    userInfo.UserState = Convert.ToInt32(reader["UserState"]);
                    queryList.Add(userInfo);
                }
                reader.Close();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
        
        public void SuperAdminOperation(string name, string operation)
        {
            switch(operation)
            {
                case "Add":
                    ExecuteSqlFromSuperAdmin(name, "insert into userInfo(userName, userPassword, userType, userState) values(@name, '88888888', 'Admin', '1')");
                    break;
                case "Block":
                    ExecuteSqlFromSuperAdmin(name, "update userInfo set userState=0 where userName=@name");
                    break;
                case "Delete":
                    ExecuteSqlFromSuperAdmin(name, "delete from userInfo where userName=@name");
                    break;
            }
        }

        private void ExecuteSqlFromSuperAdmin(string name, string sql)
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", name);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }

        public void SuperAdminBatchDelete(string[] strArray)
        {
            string sql = "delete from userInfo where userName in(";
            foreach(string temp in strArray)
            {
                sql += "'" + temp + "',"; 
            }
            sql = sql.TrimEnd(',') + ")";
            conn.Open();
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw e;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}