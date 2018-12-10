using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CADWeb.SQL;
using System.Data;

namespace WebApplication2
{
    public partial class AddChoice : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //每次从数据库获取相当于pagesize数量的条数,利用page*pagesiz得出从数据库获取的
            //根据页数从数据库或者datatable获取值
            ///数据库操作
            ///
            SqlConnection conn = SQLConnect.GetConnection();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            try
            {
                int PageSize = 10;
                int Page = 1;
                string max = "SELECT COUNT(xid) FROM 选择题库";
                SqlCommand command = new SqlCommand(max, conn);
                int Count = Convert.ToInt32(command.ExecuteScalar());               
                if (Request["Page"] != null)
                {
                    Page = Convert.ToInt32(Request["Page"]);
                }
                if (Count < PageSize)
                {
                    Page = 1;
                }
                if (Count / PageSize + 1 < Page)
                {
                    Page = Count / PageSize;
                }
                if (Page < 1) Page = 1;
                string sql = "SELECT TOP " + PageSize + " * FROM 选择题库 WHERE xid NOT IN(SELECT TOP " + ((Page - 1) * PageSize) + " xid FROM 选择题库 ORDER BY xid DESC) ORDER BY xid DESC";
                //Response.Write("<script>alert('"+sql+"')</script>");
                command = new SqlCommand(sql, conn);
                SqlDataReader sr = command.ExecuteReader();
                Response.Write("<link rel='stylesheet' href='../css/style.css'>");
                Response.Write("<table>");
                while (sr.Read())
                {
                    string ID = sr.GetInt32(0).ToString();
                    string question = sr.GetString(1);
                    if (Session["ChoiceID"] == null || Session["ChoiceID"].ToString().IndexOf(ID) == -1)
                    {

                        Response.Write("<tr><td><input class='type1' type='checkbox' name='choice'  onchange=\"window.location.href='AddChoice.ashx?ChoiceID=[" + ID + "]&page=" + Page + "'\">" + question + "</input></td></tr>");
                    }
                    else
                    {
                        if (Session["ChoiceID"].ToString().IndexOf(ID) != -1)
                        {
                            Response.Write("<tr><td><input class='type2' type='checkbox' name='choice' checked='checked' onchange=\"window.location.href='AddChoice.ashx?ChoiceID=[" + ID + "]&page=" + Page + "'\">" + question + "</input></td></tr>");
                        }
                    }
                }
                Response.Write("</table>");
                Response.Write("<a style='text-decoration:none' href='AddChoice.aspx?Page=" + (Page - 1) + "'>上一页</a><a style='text-decoration:none;float:right' href='AddChoice.aspx?Page=" + (Page + 1) + "'>下一页</a><span class='span1'>当前页数：" + Page + "</span>");
            }
            finally
            {
                conn.Close();
            }

        }


    }
}