using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CADWeb.WebPageByUserType.Admin
{
    /// <summary>
    /// AdminManage 的摘要说明
    /// </summary>
    public class AdminManage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            SQLQuery query = new SQLQuery();
            List<UserInfo> queryList = new List<UserInfo>();
            string html = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"WebPageByUserType\Admin\Admin.html");
            string content = "";
            string queryType = request.Form["select_Query"].Trim();
            switch(queryType)
            {
                case "教师":
                    queryList = query.UserQuery(3);
                    break;
                case "班级":
                    queryList = query.UserQuery(7);
                    break;
                case "学生":
                    queryList = query.UserQuery(4);
                    break;
            }
            string userState = "";
            for (int i = 0; i < queryList.Count; i++)
            {
                if (queryList[i].UserState == 0)
                {
                    userState = "已冻结";
                }
                else
                {
                    userState = "正常";
                }
                content += string.Format("<tr><td id='queryResult" + i + "' name='queryResult'>{0}</td><td name='state" + i + "'>{1}</td><td><button value='select' name='" + i + "' onclick='this.Block(this);'>冻结</button></td></tr>", queryList[i].UserName, userState);
            }
            html = html.Replace("$col1", queryType).Replace("$col2", "冻结状态").Replace("$content", content).Replace("style=\"display:none\"", "style=\"display:table\"").Replace("style=\"display:table-row\"", "style=\"display:none\"");
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