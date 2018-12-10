using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CADWeb.SQL;

namespace WebApplication2
{
    public partial class AddTest : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submit_Click(object sender, EventArgs e)
        {
            if (this.name.Text.Trim()==null|| this.testType.Text.Trim() == null || this.score.Text.Trim() == null || this.time.Text.Trim() == null ) {
                Response.Write("<script>alert('以上四个为必填项目')</script>");
                return ;
            }
            Object choice = null;
            Object judge = null;
            Object draw = null;
            if (Session["ChoiceID"] != null)
            {
                 choice = Session["ChoiceID"].ToString().Replace("[", "").Replace("]", "");
            }
            if (Session["JudgeID"] != null)
            {
                 judge = Session["JudgeID"].ToString().Replace("[", "").Replace("]", "");
            }
            if (Session["DrawID"] != null)
            {
                draw = Session["DrawID"].ToString().Replace("[","").Replace("]","");
            }
            SqlConnection conn = SQLConnect.GetConnection();
            conn.Open();
            try
            {
                
                    string sql=null;
                    if (choice != null || judge != null || draw != null)
                    {
                    if (choice == null || choice.Equals("")) choice = DBNull.Value;
                    if (judge == null || judge.Equals("")) judge = DBNull.Value;
                    if (draw == null || draw.Equals("")) draw = DBNull.Value;

                    int count = countID(choice.ToString()) + countID(judge.ToString()) + countID(draw.ToString());
                        sql = "INSERT INTO 题组库 (卷名,试卷类型,单选题题目序号,判断题题目序号,作图题题目序号,题目总数,总分,答题时长,单选题数目,判断题数目,作图题数目) VALUES ('" + this.name.Text + "','" + this.testType.Text + "'," +
                       "@choice,@judge,@draw," + count + "," + this.score.Text + ",'" + this.time.Text + "'," + countID(choice.ToString()) + "," + countID(judge.ToString()) + "," + countID(draw.ToString()) + ")";
                    } 
                    SqlCommand command = new SqlCommand(sql, conn);
                command.Parameters.AddWithValue("@choice",choice);
                command.Parameters.AddWithValue("@judge", judge);
                command.Parameters.AddWithValue("@draw", draw);
                command.ExecuteNonQuery();

                Response.Write("<script>alert('組卷完成')</script>");
                //Response.Redirect("AddTest.aspx");
            }
            catch (Exception ex){
                Response.Write(ex.Message);
            }
            finally
            {
                conn.Close();
                if (Session["ChoiceID"] != null) Session.Remove("ChoiceID");
                if (Session["JudgeID"] != null) Session.Remove("JudgeID");
                if (Session["DrawID"] != null) Session.Remove("DrawID");
                //Response.Redirect(Request.RawUrl);
            }
        }
        int countID(string str) {
            if (str == null||str.Equals("")) {
                return 0;
            }
            int index = 0;
            int count = 0;
            while ((index = str.IndexOf(",", index)) != -1)
            {
                count++;
                index = index + 1;
            }
            return count + 1;
        }
    }
}