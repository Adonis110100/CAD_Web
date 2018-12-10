using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CADWeb.SQL;

namespace WebApplication2
{
    public partial class AddQuestion1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void AdQuestion_Click(object sender, EventArgs e)
        {
            SqlConnection conn = SQLConnect.GetConnection();
            bool fileIsValid = false;
            string src =null;
            if (this.questionPic.HasFile)
            {
                String fileExtension = System.IO.Path.GetExtension(this.questionPic.FileName).ToLower();
                String[] restrictExtension = { ".jpg",".png",".jpeg",".bmp" };
                for (int i = 0; i < restrictExtension.Length; i++)
                {
                    if (fileExtension.Equals(restrictExtension[i]))
                    {
                        fileIsValid = true;
                    }
                    //如果文件类型符合要求,调用SaveAs方法实现上传,并显示相关信息
                    if (fileIsValid == true)
                    {
                        //上传文件是否大于10M
                        if (questionPic.PostedFile.ContentLength > (10 * 1024 * 1024))
                        {

                            return;
                        }
                        try
                        {
                            questionPic.SaveAs(Server.MapPath("~/UpQuestionImages/" + questionPic.FileName));
                            src = Server.MapPath("~/UpQuestionImages/" + questionPic.FileName);
                        }
                        catch
                        {
                            //Response.Write(Server.MapPath("~/ File / ") + questionPic.FileName);
                            Response.Write("文件上传失败!");
                        }
                        finally
                        {
                        }
                    }
                    else
                    {
                        Response.Write("文件类型错误");
                    }
                }
            }
            string question = this.question.Text;
            string A = this.A.Text;
            string B = this.B.Text;
            string C = this.C.Text;
            string D = this.D.Text;
            string answer = this.answer.Text;
            string answerjx = this.answerjx.Text;
            
            //数据库提交操作
            if(conn.State==ConnectionState.Closed)
                conn.Open();
            string sql;
            if (src == null)
            {
                sql = "INSERT INTO 选择题库 (题目,选项A,选项B,选项C,选项D,答案,答案解析,题目状态) VALUES ('" + question + "','" + A + "','" + B + "','" + C +
                 "','" + D + "','" + answer + "','" + answerjx + "',1)";
            }
            else
            {
                sql = "INSERT INTO 选择题库 (题目,选项A,选项B,选项C,选项D,答案,答案解析,图片路径,题目状态) VALUES ('" + question + "','" + A + "','" + B + "','" + C +
                 "','" + D + "','" + answer + "','" + answerjx + "','" + src + "',1)";
            }
            SqlCommand command = new SqlCommand(sql, conn);
            command.ExecuteNonQuery();
            conn.Close();
            Response.Redirect("AddQuestion1.aspx");
        }

        protected void ChoiceToSql_Click(object sender, EventArgs e)
        {
            MyFileUpload.Visible = true;
            FileUploadButton.Visible = true;
        }
        protected void FileUploadButton_Click(object sender, EventArgs e)
        {
            string classtable = ((Button)sender).CommandArgument.ToString();
            if (MyFileUpload.FileName == "")
            {

                return;
            }

            bool fileIsValid = false;
            //如果确认了上传文件，则判断文件类型是否符合要求
            if (this.MyFileUpload.HasFile)
            {
                //获取上传文件的后缀
                String fileExtension = System.IO.Path.GetExtension(this.MyFileUpload.FileName).ToLower();
                String[] restrictExtension = { ".xlsx",".xls" };
                //判断文件类型是否符合要求
                for (int i = 0; i < restrictExtension.Length; i++)
                {
                    if (fileExtension.Equals(restrictExtension[i]))
                    {
                        fileIsValid = true;
                    }
                    //如果文件类型符合要求,调用SaveAs方法实现上传,并显示相关信息
                    if (fileIsValid == true)
                    {
                        //上传文件是否大于10M
                        if (MyFileUpload.PostedFile.ContentLength > (10 * 1024 * 1024))
                        {

                            return;
                        }
                        try
                        {
                            MyFileUpload.SaveAs(Server.MapPath("~/UpFile/" + MyFileUpload.FileName));
                            Response.Write("文件上传成功!");
                        }
                        catch
                        {
                            //Response.Write(Server.MapPath("~/ File / ") + MyFileUpload.FileName);
                            Response.Write("文件上传失败!");
                        }
                        finally
                        {
                        }
                    }
                    else
                    {
                        Response.Write("文件类型错误");
                    }
                }
            }
            string filepath = Server.MapPath("~/UpFile/" + MyFileUpload.FileName);
            InsertSql( filepath, classtable);
            File.Delete(filepath);
        }
        public void InsertSql( string filepath, string classtable)//参数（数据库连接语句）
        {

            try
            {
                string fileName = filepath;
                //SqlCommand command = new SqlCommand("set identity_insert "+classtable+".dbo ON", conn);//设置可插入主键
                //command.ExecuteNonQuery();//执行sql语句  
                Bind(fileName,classtable);
            }

            catch (Exception e)
            {
                Response.Write("操作失败");
                return;
            }

        }
        public void Bind(string fileName,string classtable)//获取excel中的数据
        {
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                 "Data Source=" + fileName + ";" +
                 "Extended Properties='Excel 12.0; HDR=No; IMEX=1'";//excel链接语句                                                               
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT *  FROM [选择题$]", strConn);//选中excel工作表
            DataSet ds = new DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                da.Fill(ds);//将excel表中的数据保存到DataTable中。  
                dt = ds.Tables[0];
                //this.dataGridView1.DataSource = dt;  
                if (dt.Columns.Count < 8)
                {
                    Response.Write("请使用具有完整数据列的excel导入");
                    return;
                }
                if (dt.Rows.Count > 100)
                {
                    Response.Write("数据超过100条，每次导入请将数据条目限制在100条以内");
                    return;
                }
            }
            catch (Exception err)
            {
                Response.Write("操作失败！" + err.ToString());
            }
            try
            {
                System.Data.DataTable table = new System.Data.DataTable();//将excel中的数据进行处理并放进一个datatabl中
                table.Columns.Add(new DataColumn("题目", Type.GetType("System.String")));
                table.Columns.Add(new DataColumn("选项A", Type.GetType("System.String")));
                table.Columns.Add(new DataColumn("选项B", Type.GetType("System.String")));
                table.Columns.Add(new DataColumn("选项C", Type.GetType("System.String")));
                table.Columns.Add(new DataColumn("选项D", Type.GetType("System.String")));
                table.Columns.Add(new DataColumn("答案", Type.GetType("System.String")));
                table.Columns.Add(new DataColumn("答案解析", Type.GetType("System.String")));
                table.Columns.Add(new DataColumn("题目状态", Type.GetType("System.Int32")));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = table.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                            dr[j] = dt.Rows[i][j];
                    }
                    table.Rows.Add(dr);
                }

                bool b = InsertToSql(table, classtable);
                //if (b == true) Response.Write("<script>alter('导入成功')</script>");
                //else Response.Write("<script>alter('导入失败')</script>");
            }
            catch (Exception err)
            {
                Response.Write("操作失败！" + err.ToString());
            }

        }
        private bool InsertToSql(System.Data.DataTable table, string classtable)//导入数据库
        {
            //excel表中的列名和数据库中的列名一定要对应  
            try
            {
                using (SqlConnection conn = SQLConnect.GetConnection())
                {
                    conn.Open();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.KeepNulls | SqlBulkCopyOptions.FireTriggers, null))
                    {
                        bulkCopy.DestinationTableName = "dbo.选择题库";//设置数据库中对象的表名
                                                                   //设置数据表table和数据库中表的列对应关系

                        bulkCopy.ColumnMappings.Add("题目", "题目");
                        bulkCopy.ColumnMappings.Add("选项A", "选项A");
                        bulkCopy.ColumnMappings.Add("选项B", "选项B");
                        bulkCopy.ColumnMappings.Add("选项C", "选项C");
                        bulkCopy.ColumnMappings.Add("选项D", "选项D");
                        bulkCopy.ColumnMappings.Add("答案", "答案");
                        bulkCopy.ColumnMappings.Add("答案解析", "答案解析");
                        bulkCopy.ColumnMappings.Add("题目状态", "题目状态");
                        bulkCopy.WriteToServer(table);//将数据表table复制到数据库中
                        Response.Write("共导入" + table.Rows.Count + "条数据");
                    }
                    conn.Close();
                }
                return true;

            }
            catch (Exception e)
            {
                Response.Write("操作失败！" + e.ToString());
                return false;
            }
            finally
            {
                
            }
        }
    }
}