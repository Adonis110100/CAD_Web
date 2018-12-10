using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CADWeb.SQL;

namespace CADWeb.WebPageByUserType.Student.ExamQuestions
{
    public partial class DrawingQuertions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string drawID = "";
            string examName = "";
            try
            {
                Response.AddHeader("P3P","CP=CAO PSA OUR");
                examName = HttpUtility.UrlDecode(Request.Params["examName"].ToString());
                //string examName = "卷6";
                SqlConnection conn = SQLConnect.GetConnection();
                if(conn.State==System.Data.ConnectionState.Closed)
                    conn.Open();
                string sql = "select 作图题题目序号 from 题组库 where 卷名='" + examName + "'";
                SqlCommand command = new SqlCommand(sql, conn);
                SqlDataReader sr = command.ExecuteReader();
                if (sr.Read())
                {
                    drawID = sr["作图题题目序号"].ToString();
                    //drawID = sr.GetString(0);
                    Session.Add("drawID", drawID);
                }
                sr.Close();
                conn.Close();
            }
            catch (Exception a)
            {
                Console.WriteLine(a.Message);
            }
        }     
    }
}