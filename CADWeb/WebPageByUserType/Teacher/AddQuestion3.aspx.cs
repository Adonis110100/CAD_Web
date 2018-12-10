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
    public partial class AddQuestion3 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Write("<script language='javascript'>alert('pageLoad!!')</script>");
            SqlConnection conn = SQLConnect.GetConnection();
            conn.Open();
            string sqltext = "select IDENT_CURRENT('作图题库')+IDENT_INCR('作图题库')";
            SqlCommand sqlcmd = new SqlCommand(sqltext, conn);
            int rows;
            object o = sqlcmd.ExecuteScalar();
            if (o == DBNull.Value)
            {
                rows = 0;
            }
            else
            {
                rows = Convert.ToInt32(o) ;
            }
            string testID = rows.ToString();
            if (Context.Session["testID"] != null)
            {
                if (!(Context.Session["testID"].ToString().Equals(testID)))
                {
                    Context.Session["testID"] = testID;
                }
            }
            else
            {
                Context.Session.Add("testID", testID);
            }
            conn.Close();
        }
       
        protected void sumbit_Click(object sender, EventArgs e)
        {
            SqlConnection conn = SQLConnect.GetConnection();
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            try
            {
                if (this.timu.Text == null || this.timu.Text.Equals("")) {
                    Response.Write("<script language='javascript'>alert('请输入题目文字信息!!')</script>");
                    return;
                }
                string wenzi = this.timu.Text.Trim();
                int testID = Convert.ToInt32(Session["testID"].ToString());
                string sql = "update 作图题库 set 题目='" + wenzi + "' where id=" +(testID-1);
                SqlCommand sqlCommand = new SqlCommand(sql, conn);
                sqlCommand.ExecuteNonQuery();
                if (Session["testID"] != null) Session.Remove("testID");
            }
            catch(Exception ex) {
                
            }
            finally
            {
                conn.Close();               
            }
        }

        protected void next_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.Url.ToString());
        }
    }
}