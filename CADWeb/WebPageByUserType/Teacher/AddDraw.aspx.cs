using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CADWeb.SQL;

namespace WebApplication2
{
    public partial class AddDraw : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //每次从数据库获取相当于pagesize数量的条数,利用page*pagesiz得出从要数据库获取的数据开始条目
            //根据页数从数据库或者datatable获取值
            ///数据库操作
            SqlConnection conn = SQLConnect.GetConnection();
            try
            {
                if(conn.State==ConnectionState.Closed)
                    conn.Open();
                int PageSize = 10;
                int Page = 1;            
                string max = "SELECT COUNT(id) FROM 作图题库";
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
                if (Count/PageSize+1<Page)
                {
                    Page = Count / PageSize;
                }                                       
                if (Page < 1) Page = 1;
                string sql = "SELECT TOP " + PageSize + " * FROM 作图题库 WHERE id NOT IN(SELECT TOP " + ((Page - 1) * PageSize) + " id FROM 作图题库 ORDER BY id DESC) ORDER BY id DESC";
                //Response.Write("<script>alert('"+sql+"')</script>");
                command = new SqlCommand(sql, conn);
                SqlDataReader sr = command.ExecuteReader();
                Response.Write("<link rel='stylesheet' href='../css/style.css'>");
                Response.Write("<table>");
                while (sr.Read())
                {

                    string ID = sr.GetInt32(0).ToString();
                    if (sr["题目"] == DBNull.Value)
                        continue;
                    string question = sr["题目"].ToString();                   
                    if (Session["DrawID"] == null || Session["DrawID"].ToString().IndexOf(ID) == -1)
                    {
                        Response.Write("<tr><td><input class='type1' type='checkbox' name='draw'  onchange=\"window.location.href='AddDraw.ashx?DrawID=[" + ID + "]&page=" + Page + "'\">"+question+"</input></td></tr>");
                    }
                    else
                    {
                        if (Session["DrawID"].ToString().IndexOf(ID) != -1)
                            Response.Write("<tr><td><input class='type2' type='checkbox' name='draw'checked='checked' onchange=\"window.location.href='AddDraw.ashx?DrawID=[" + ID + "]&page=" + Page + "'\">"+question+"</input></td></tr>");
                    }
                }
                Response.Write("</table>");
                Response.Write("<a style='text-decoration:none' href='AddDraw.aspx?Page="+(Page-1) +"'>上一页</a><a style='text-decoration:none;float:right' href='AddDraw.aspx?Page=" + (Page + 1) + "'>下一页</a><span class='span1'>当前页数：" + Page + "</span>");
            }
            finally
            {
                conn.Close();
            }
        }     
    }
}