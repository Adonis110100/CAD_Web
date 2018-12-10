using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb.WebPageByUserType.Admin
{
    /// <summary>
    /// AdminManageQuestions 的摘要说明
    /// </summary>
    public class AdminManageQuestions : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string school = HttpUtility.UrlDecode(request.Params["School"]);
            response.ContentType = "text/html";
            response.Write("<script type = 'text/javascript' src = '../../js/jquery-1.7.2.min.js' ></script>");
            response.Write("<script type = 'text/javascript' src = '../../js/custom.js' ></script>");
            response.Write("<link rel='stylesheet' href='../css/style.css'>");
            response.Write("<div id=\"banner\">&nbsp;&nbsp;发布作业或考试题组");
            response.Write("<a href='Admin.html'><button class='but_back'>返回</button></a></div>");
            response.Write("<div id=\"center\"><br>");
            response.Write("<HR width=\"100%\" color=#E4E4E4 SIZE=2>");


            SQLQuery query = new SQLQuery();
            List<string> queryListQuestion = query.UserQuery(5, school) as List<string>;
            string tableStr = "<table border='1'><caption style='color:red'>题库管理（题库冻结与解冻）</caption><tr><th>题目</th><th>冻结状态</th></tr>";
            int index = 0;
            int x = -1;
            for (int i = 0; i < queryListQuestion.Count; i++)
            {
                if (queryListQuestion[i].Equals("<选择题>") || queryListQuestion[i].Equals("<判断题>") || queryListQuestion[i].Equals("<作图题>"))
                {
                    x++;
                    tableStr += "<tr><th id='type" + x + "'>" + queryListQuestion[i] + "</th></tr>";
                }
                else
                {
                    string[] resultArray = queryListQuestion[i].Split('|');
                    resultArray[1] = resultArray[1].Equals("1") ? "正常" : "已冻结";
                    tableStr += string.Format("<tr><td id='question{2}' name='{3}'>{0}</td><td id='state{2}'>{1}</td><td><button value='select' name='{2}' onclick='BlockOrUnblock(this,\"已冻结\");'>冻结</button></td><td><button value='select' name='{2}' onclick='BlockOrUnblock(this,\"正常\");'>解冻</button></td></tr>", resultArray[0], resultArray[1], index, x);
                    index++;
                }
            }
            tableStr += "</table>";
            
            response.Write(tableStr);
            response.Write("</div><div id='footer1'></div>");
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