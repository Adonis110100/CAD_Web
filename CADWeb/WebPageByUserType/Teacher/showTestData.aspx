<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="showTestData.aspx.cs" Inherits="CADWeb.WebPageByUserType.Teacher.showTestData" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="CADWeb.SQL" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>

<link rel="stylesheet" href="../css/style.css">
    <title></title>
</head>
<body>
        <div>
            <table>
                <tr>
                    <th>试卷名</th>
                    <th>查看学生成绩</th>             
                </tr>
            <% if (Session["class"] != null) {
                    ///数据库操作
                    ///
                    string school=HttpUtility.UrlDecode(Request.Params["school"]);
                    SqlConnection conn = SQLConnect.GetConnection();
                    conn.Open();
                    DataSet ds = new DataSet();
                    string sql = "use [CAD__"+school+"] select name from sysobjects where name like 'testScore_%'";
                    SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                    da.Fill(ds);
                    DataTable dt = ds.Tables[0];
                    for(int i=0;i<dt.Rows.Count;i++) {
                        string tableName = dt.Rows[i][0].ToString();
                        string sqltext = "select * from "+ tableName+ " where 年级班级='" + Session["class"]+"'";
                        SqlCommand command = new SqlCommand(sqltext, conn);
                        SqlDataReader sr = command.ExecuteReader();
                        if (sr.Read())
                        {
                            string name = dt.Rows[i][0].ToString(); ;
                            //string testName = name.Split('_')[1];.
                        %>
                        <tr>
                            <td>
                                <%=name %>
                            </td>                           
                            <td style="width:30%">
                                <a class='showT' href="showTestScore.aspx?testName=<%=name %>&class=<%=Session["class"]%>">查看学生成绩</a>                                                       
                            </td>
                        </tr>
                          <% 
                        }
                        sr.Close();
                    }
                    conn.Close();
               }
                %>
                </table>
        </div>
</body>
</html>
