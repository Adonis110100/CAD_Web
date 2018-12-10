using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using CADWeb.SQL;
namespace CADWeb.WebPageByUserType.Teacher
{
    /// <summary>
    /// updateClassTestState 的摘要说明
    /// </summary>
    public class updateClassTestState : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string classString= request.QueryString["classString"].ToString();
            string testName = request.QueryString["testName"].ToString();
            string addClassString = request.QueryString["addClassString"].ToString();
            string[] temp = classString.Split(',');
            string final = "";
            foreach(string x in temp)
            {
                final = final + x + ",";
            }
            final = final + addClassString;
            SqlConnection conn = SQLConnect.GetConnection();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            string sql = "update 题组库 set 试卷状态=@state where 卷名=@testName";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@state",final);
            cmd.Parameters.AddWithValue("@testName", testName);
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                conn.Close();
                context.Response.Redirect("ReleaseWorkOrTest.ashx");
            }
            //context.Response.ContentType = "text/plain";
            //context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}