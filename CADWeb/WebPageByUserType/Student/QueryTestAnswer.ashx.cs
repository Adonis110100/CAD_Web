using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb.WebPageByUserType.Student
{
    /// <summary>
    /// QueryTestAnswer 的摘要说明
    /// </summary>
    public class QueryTestAnswer : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.AddHeader("P3P", "CP=CAO PSA OUR");
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string school = HttpUtility.UrlDecode(request.Params["School"]);
            string gradeClass = HttpUtility.UrlDecode(request.Params["gradeClass"]);
            response.ContentType = "text/html";
            response.Write("<script type = 'text/javascript' src = '../../js/jquery-1.7.2.min.js' ></script>");
            response.Write("<script type='text/javascript' src='../../js/jquery.cookie.js'></script>");
            response.Write("<script type = 'text/javascript' src = '../../js/custom.js' ></script>");
            response.Write("<link rel='stylesheet' href='../css/style.css'>");
            response.Write("<title>查询试题</title>");
            SQLQuery query = new SQLQuery();
            string stuName = HttpUtility.UrlDecode(request.Cookies["UserName"].Value.Trim());
            string className = gradeClass; //query.QueryTableNameFromStuName(stuName).Remove(0, 10);
            string[] stuInfoArray = query.QueryStuInfoFromClass(school,className, stuName).Split('|');
            string[] stuAnswerStateArray = stuInfoArray[stuInfoArray.Length - 2].TrimEnd('&').Split('&');
            if (string.IsNullOrEmpty(stuAnswerStateArray[0]))
            {
                response.Write("<script type='text/javascript'>alert('您尚未提交过任何试题！'); window.location.href='Student.html'</script>");
                response.Flush();
                return;
            }
            response.Write("<div id=\"banner\">&nbsp;&nbsp;查询试题</div>");
            response.Write("<div id=\"center\"><br>");
            response.Write("<HR width=\"100%\" color=#E4E4E4 SIZE=2>");
            
            string html = "<h2>请选择你要查询的试题</h2><table border='1'><tr><th>试题名称</th></tr>";
            string allAnswer = "";
            for (int i = 0; i < stuAnswerStateArray.Length; i++)
            {
                string queryResult = query.QueryStudentTestAnswer(school,stuAnswerStateArray[i], stuName);
                if (!string.IsNullOrEmpty(queryResult))
                {
                    allAnswer += queryResult + "&";
                    html += "<tr><td><label for='exam_" + stuAnswerStateArray[i] + "'><input style='width:50px' type='radio' id='exam_" + stuAnswerStateArray[i] + "' name='exam' />" + stuAnswerStateArray[i] + "</label></td></tr>";
                }
            }

            html += "</table><br><br><input type='submit' class='but_search' value='查询' onclick='BeginQueryAnswer(\"" + allAnswer.TrimEnd('&') + "\");'/></div><div id='foot'></div>'";
           
            response.Write(html);
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}