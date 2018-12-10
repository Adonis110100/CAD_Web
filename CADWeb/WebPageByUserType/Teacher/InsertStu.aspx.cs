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

namespace CADWeb.WebPageByUserType.Teacher
{
    public partial class InsertStu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void FileUploadButton_Click(object sender, EventArgs e)
        {
            SqlConnection conn = SQLConnect.GetConnection();

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
                String[] restrictExtension = {".xlsx" };
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
            string className = this.ClassName.Text;
            InsertSql( filepath,classtable,className);
            File.Delete(filepath);
        }
        public void InsertSql( string filepath,string classtable,string className)//参数（数据库连接语句）
        {
            try
            {
                SqlConnection conn = SQLConnect.GetConnection();
                conn.Open();//打开链接
                string fileName = filepath;
                //SqlCommand command = new SqlCommand("set identity_insert "+classtable+".dbo ON", conn);//设置可插入主键
                //command.ExecuteNonQuery();//执行sql语句  
                Bind(fileName, conn,classtable,className);
            }

            catch (Exception e)
            {
                Response.Write("操作失败");
                return;
            }

        }
        public void Bind(string fileName, SqlConnection conn,string classtable,string className)//获取excel中的数据
        {
            string strConn = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                 "Data Source=" + fileName + ";" +
                 "Extended Properties='Excel 12.0; HDR=No; IMEX=1'";//excel链接语句                                                               
            OleDbDataAdapter da = new OleDbDataAdapter("SELECT *  FROM [学生$]", strConn);//选中excel工作表
            DataSet ds = new DataSet();
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                da.Fill(ds);//将excel表中的数据保存到DataTable中。  
                dt = ds.Tables[0];
                //this.dataGridView1.DataSource = dt;  
                if (dt.Columns.Count < 4)
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
                table.Columns.Add(new DataColumn("学号", Type.GetType("System.String")));
                table.Columns.Add(new DataColumn("姓名", Type.GetType("System.String")));
                table.Columns.Add(new DataColumn("密码", Type.GetType("System.String")));
                table.Columns.Add(new DataColumn("学生状态", Type.GetType("System.Int32")));
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow dr = table.NewRow();
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        if (j == 4 && dt.Rows[i][j] != DBNull.Value)
                        {
                            FileStream fs = new FileStream(dt.Rows[i][j].ToString(), FileMode.Open);
                            byte[] bytes = new byte[fs.Length];
                            fs.Read(bytes, 0, bytes.Length);
                            fs.Close();
                            dr[j] = bytes;
                        }
                        else
                            dr[j] = dt.Rows[i][j];
                    }
                    table.Rows.Add(dr);
                }

                bool b = InsertToSql(table, conn,classtable,className);
                //if (b == true) Response.Write("<script>alter('导入成功')</script>");
                //else Response.Write("<script>alter('导入失败')</script>");
            }
            catch (Exception err)
            {
                Response.Write("操作失败！" + err.ToString());
            }

        }
        private bool InsertToSql(System.Data.DataTable table, SqlConnection conn,string classtable,string className)//导入数据库
        {
            string school = HttpUtility.UrlDecode(Request.Params["School"]);
            //excel表中的列名和数据库中的列名一定要对应  \
            string sql = "use [CAD__" + school + "]";
            SqlCommand cmd = new SqlCommand(sql,conn);
            try
            {
                cmd.ExecuteNonQuery();
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn, SqlBulkCopyOptions.KeepIdentity | SqlBulkCopyOptions.KeepNulls | SqlBulkCopyOptions.FireTriggers, null))
                    {
                        bulkCopy.DestinationTableName = "dbo.classInfo_"+className;//设置数据库中对象的表名
                                                                               //设置数据表table和数据库中表的列对应关系

                        bulkCopy.ColumnMappings.Add("学号", "学号");
                        bulkCopy.ColumnMappings.Add("姓名", "姓名");
                        bulkCopy.ColumnMappings.Add("密码", "密码");
                        bulkCopy.ColumnMappings.Add("学生状态", "学生状态");
                        bulkCopy.WriteToServer(table);//将数据表table复制到数据库中
                        Response.Write("共导入" + table.Rows.Count + "条数据");
                    }
                
                return true;

            }
            catch (Exception e)
            {
                Response.Write("操作失败！" + e.ToString() );
                return false;
            }
            finally
            {
                conn.Close();
            }
        }
    }
}