using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace CADWeb.WebPageByUserType.Admin
{
    /// <summary>
    /// AdminManage 的摘要说明
    /// </summary>
    public class AdminManage : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string school = HttpUtility.UrlDecode(request.Params["School"]);
            SQLQuery query = new SQLQuery();
            List<UserInfo> queryList = new List<UserInfo>();
            string html = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"WebPageByUserType\Admin\Admin.html");
            string content = "";
            string queryType = request.Form["select_Query"].Trim();
            switch (queryType)
            {
                case "教师":
                    queryList = query.UserQuery(3, school) as List<UserInfo>;
                    break;
                case "班级":
                    queryList = query.QueryClassInfo(request.Form["defaultInput"].ToString());
                    break;
                case "学生":
                    queryList = query.UserQuery(4, school) as List<UserInfo>;
                    break;
            }
            string userState = "";
            int x = -1;
            int y = 0;
            for (int i = 0; i < queryList.Count; i++)
            {
                if (!queryType.Equals("学生"))
                {
                    if (queryList[i].UserState == 0)
                    {
                        userState = "已冻结";
                    }
                    else
                    {
                        userState = "正常";
                    }
                    content += string.Format(
                        "<tr>" +
                            "<td id='queryResult{0}' name='queryResult{0}'>{1}</td>" +
                            "<td name='state{0}'>{2}</td>" +
                            "<td><button value='select' name='{0}' onclick='Block_Unblock(this,\"已冻结\");'>冻结</button></td>" +
                            "<td><button value='select' name='{0}' onclick='Block_Unblock(this,\"正常\");'>解冻</button></td>" +
                        "</tr>", i, queryList[i].UserName, userState);
                    continue;
                }
                else
                {
                    if (queryList[i].UserName.Contains("{"))
                    {
                        x++;
                        content += string.Format("<tr><td id='className{0}' name='className'>{1}</td></tr>", x, queryList[i].UserName);
                        continue;
                    }
                    else
                    {
                        if (queryList[i].UserState == 0)
                        {
                            userState = "已冻结";
                        }
                        else
                        {
                            userState = "正常";
                        }
                        content += string.Format(
                            "<tr>" +
                                "<td id='queryResult{2}' name='queryResult{3}'>{0}</td>" +
                                "<td>{4}</td>" +
                                "<td name='state{2}'>{1}</td>" +
                                "<td><button value='select' name='{2}' onclick='Block_UnblockStudent(this, {3}, \"{4}\", \"已冻结\");'>冻结</button></td>" +
                                "<td><button value='select' name='{2}' onclick='Block_UnblockStudent(this, {3}, \"{4}\", \"正常\");'>解冻</button></td>" +
                            "</tr>", queryList[i].UserName, userState, y, x, queryList[i].UserPassword);
                        y++;
                    }
                }
            }
            if (queryType.Equals("学生"))
                html = html.Replace("$col1", queryType).Replace("$col3", "冻结状态").Replace("$col2", "学号")
                    .Replace("$content", content).Replace("style=\"display:none\"", "style=\"display:table\"")
                    .Replace("style=\"display:table-row\"", "style=\"display:none\"");
            else
                html = html.Replace("$col1", queryType).Replace("$col2", "冻结状态").Replace("<th>$col3</th>", "")
                    .Replace("$content", content).Replace("style=\"display:none\"", "style=\"display:table\"")
                    .Replace("style=\"display:table-row\"", "style=\"display:none\"");
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