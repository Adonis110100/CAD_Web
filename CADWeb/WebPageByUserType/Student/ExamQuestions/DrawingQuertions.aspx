<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DrawingQuertions.aspx.cs" Inherits="CADWeb.WebPageByUserType.Student.ExamQuestions.DrawingQuertions" %>
    <%@ Import Namespace="System.Data.SqlClient" %>
        <%@ Import Namespace="System.Data" %>
            <%@ Import Namespace="CADWeb.SQL" %>
                <!DOCTYPE html>

                <html xmlns="http://www.w3.org/1999/xhtml">

                <head runat="server">
                    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
                    <meta http-equiv="P3P" content='CP="IDC DSP COR CURa ADMa OUR IND PHY ONL COM STA"'>
                    <link rel="stylesheet" href="../../css/style.css">
                    <title></title>
                </head>

                <body>
                    <div>
                        <div>
                            <%
                string stuName = Request.Cookies["UserName"].Value.ToString();
                string gradeClass = Request["gradeClass"].ToString();
                string stuID = Request["userID"].ToString();
                string school = HttpUtility.UrlDecode(Request["school"].ToString());
                string examName = Request["examName"].ToString();
                //string stuName = "yxt";
                //string gradeClass ="1701";
                //string stuID = "16240070";
                //string school ="深职院";
                //string examName ="juan_10";
                SqlConnection conn = SQLConnect.GetConnection();
                string sql = "";

                //string ID = "卷6";
                //string stuID = "张三";
                string img = "";
                //string rubbish = Session["DrawID"].ToString();
                if (Session["DrawID"] != null && !Session["DrawID"].Equals(""))
                {
                    string[] drawID;
                    if (Session["DrawID"].ToString().IndexOf(',') == -1)
                    {
                        drawID = new string[1] { Session["DrawID"].ToString() };
                    }
                    else
                    {
                        drawID = Session["DrawID"].ToString().Split(',');
                    }
                    //string[] drawID = "7,8".Split(',');
                    for (int i = 0; i < drawID.Length; i++)
                    {
                        string drawid = drawID[i];
                        ///数据库操作
                        conn.Close();
                        if(conn.State==ConnectionState.Closed)
                            conn.Open();
                        sql = "select 作图题题目图片 from 作图题库 where id=" + drawid.Trim();
                        SqlCommand command = new SqlCommand(sql, conn);
                        SqlDataReader sr = command.ExecuteReader();
                        if (sr.Read())
                        {
                            if (sr["作图题题目图片"] != DBNull.Value)
                            {
                                byte[] question = (byte[])sr["作图题题目图片"];
                                img = Convert.ToBase64String(question);
                            }
                        }
                        sr.Close();
                        conn.Close();

                %>
                                <h3>绘图题
                                    <%=i + 1 %>
                                </h3>
                                <a href="../../../CanvasEXE/Canvas.application?examName=<%=examName %>&status=答题&stuID=<%=stuID %>&testID=<%=(i + 1) %>&className=<%=gradeClass %>&school=<%=school %>&stuName=<%=stuName %>">
                                   <img src="data:image/jpg;base64,<%=img %>" width="700" height="500" alt="<%=ID %>" /><br>
                                <span class='but_add5'>开始答题</span></a>
                                <a href="../../../CanvasEXE/Canvas.application?examName=<%=examName %>&status=修改答案&stuID=<%=stuID %>&testID=<%=(i + 1) %>&className=<%=gradeClass %>&school=<%=school %>&stuName=<%=stuName %>">
                                    <span class='but_add5'>修改答案</span>
                                </a>
                                <%}
                   }%>
                        </div>
                    </div>
                </body>

                </html>