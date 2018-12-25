using CADWeb.Model;
using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb.WebPageByUserType.Teacher
{
    /// <summary>
    /// ReleaseWorkOrTest 的摘要说明
    /// </summary>
    public class ReleaseWorkOrTest : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            response.ContentType = "text/html";
            response.Write("<script type = 'text/javascript' src = '../../js/jquery-1.7.2.min.js' ></script>");
            response.Write("<script type = 'text/javascript' src = '../../js/custom.js' ></script>");
            response.Write("<link rel='stylesheet' href='../css/style.css'>");
            response.Write("<div id=\"banner\">&nbsp;&nbsp;发布作业或考试题组</div>");
            response.Write("<div id=\"center\"><br>");
            response.Write("<HR width=\"100%\" color=#E4E4E4 SIZE=2>");
            SQLQuery query = new SQLQuery();
            List<ExamInfo> exams = query.QueryExamsInfo();
            string html = "<div><input type='text' id='inputClass' placeholder='请输入发布的班级（例：16级2班 格式为1602）'/></div><div><table border='1'><tr><th>卷名</th><th>试卷类型</th><th>试卷可用班级</th><th></th></tr>";
            for (int i = 0; i < exams.Count; i++)
            {
                html += string.Format("<tr><td id='exam_{3}'>{0}</td><td>{1}</td><td id='examState_{3}'>{2}</td><td style='width:50px;'><input style='width:50px; 'type='radio' id='{3}' name='exam' /></td></tr>", exams[i].ExamName, exams[i].ExamType, exams[i].ExamState.Replace('|', '，'), i);
            }
            html += "</table></div><br><br><button value='select' onclick='Release();'>发布</button></div><div id='foot'></div>";
            response.Write(html);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}