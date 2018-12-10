using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CADWeb.SQL;

namespace CADWeb.WebPageByUserType.Teacher
{
    public partial class TestData : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string school=HttpUtility.UrlDecode(Request.Params["school"]);
            this.DropDownList1.Items.Clear();
            SqlConnection conn = SQLConnect.GetConnection();
            //SqlConnection conn = new SqlConnection(connStr);//创建连接  
            try
            {
                conn.Open();
                string cmdText = "select 年级班级 From  学校班级信息 where 学校名=@school";
                SqlCommand cmd = new SqlCommand(cmdText, conn);
                cmd.Parameters.AddWithValue("@school",school);
                SqlDataReader sdr = cmd.ExecuteReader();
                while (sdr.Read())
                {
                    this.DropDownList1.Items.Add(new ListItem(sdr[0].ToString()));
                }

            }
            catch (Exception ex) { }
            finally
            {
                conn.Close();
            }

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }  
}