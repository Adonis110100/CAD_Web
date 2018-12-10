using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CADWeb.WebPageByUserType.Student
{
    /// <summary>
    /// QueryAvailableExam 的摘要说明
    /// </summary>
    public class QueryAvailableExam : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string school = HttpUtility.UrlDecode(request.Params["School"]);
            response.ContentType = "text/html";
            response.Write("<script type = 'text/javascript' src = '../../js/jquery-1.7.2.min.js' ></script>");
            response.Write("<script type='text/javascript' src='../../js/jquery.cookie.js'></script>");
            response.Write("<script type = 'text/javascript' src = '../../js/custom.js' ></script>");
            response.Write("<link rel='stylesheet' href='../css/style.css'>");
            SQLQuery query = new SQLQuery();
            string stuName = HttpUtility.UrlDecode(request.Cookies["UserName"].Value.Trim());
            string tableName = "classInfo_"+context.Request.Params["gradeClass"];//query.QueryTableNameFromStuName(stuName);//
            if (string.IsNullOrEmpty(tableName))
            {
                response.Write("<script languge='javascript'> alert('用户信息有误，请联系管理员处理！'); window.location.href='Login.html'; </script>");
            }
            string className = tableName.Remove(0, 10);
            string cookie_stuInfo = query.QueryStuInfoFromClass(school,className, stuName);
            response.Write("<script type='text/javascript'> $.cookie('stuInfo', '" + cookie_stuInfo + "'); </script>");// alert('成功进入！'); 
            string[] stuInfoArray = cookie_stuInfo.Split('|');
            string stuAnswerState = stuInfoArray[stuInfoArray.Length - 2];
            Dictionary<string, string> examDic = new Dictionary<string, string>();
            examDic = query.QueryAllExamState();
            response.Write("<div id=\"banner\">&nbsp;&nbsp;选择试题</div>");
            response.Write("<div id=\"center\"><br>");
            response.Write("<HR width=\"100%\" color=#E4E4E4 SIZE=2>");
            string html = "<h2>请选择试题</h2><table border='1'><tr><th style='width:50px'>试题名称</th><th>考试时长</th></tr>";
            foreach (string key in examDic.Keys)
            {
                string[] keys = key.Split('|');
                keys[1] = string.IsNullOrEmpty(keys[1]) ? "不限时长" : keys[1];
                if (stuAnswerState.Contains(keys[0]))
                {
                    continue;
                }
                if (examDic[key].Contains(className))
                {
                    html += string.Format("<tr><td><label for='exam_{0}'><input style='width:50px' type='radio' id='exam_{0}' name='exam' />{0}</label></td><td id='time_{0}'>{1}</td></tr>", keys[0], keys[1]);
                }
            }
            html += "</table><br><br><div><input type='submit' class='but_search' value='开始考试' onclick='BeginExam();'/></div></div><div id='foot'></div>";
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