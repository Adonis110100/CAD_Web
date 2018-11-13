using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace CADWeb.WebPageByUserType.SuperAdmin
{
    /// <summary>
    /// SuperAdminManage 的摘要说明
    /// </summary>
    public class SuperAdminManage : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            SQLQuery query = new SQLQuery();
            List<UserInfo> queryList = new List<UserInfo>();
            string html = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"WebPageByUserType\SuperAdmin\SuperAdmin.html");
            string content = "";
            string displayStr = "";
            string serialNum = request.Form["select_Query"].ToString();
            switch(serialNum)
            {
                case "学校名添加、冻结、删除":
                    displayStr = "学校名";
                    queryList = query.UserQuery(1);
                    break;
                case "管理员添加、冻结、删除":
                    displayStr = "管理员";
                    queryList = query.UserQuery(2);
                    break;
                case "教师删除":
                    displayStr = "教师";
                    queryList = query.UserQuery(3);
                    break;
                case "学生批量删除":
                    displayStr = "学生";
                    queryList = query.UserQuery(4);
                    break;
                case "题库批量删除":
                    displayStr = "题库";
                    queryList = query.UserQuery(5);
                    break;
                case "成绩批量删除":
                    displayStr = "成绩";
                    queryList = query.UserQuery(6);
                    break;
            }
            if(displayStr.Equals("学校名") || displayStr.Equals("管理员"))
            {
                string userState = "";
                for (int i = 0; i < queryList.Count; i++)
                {
                    if(queryList[i].UserState == 0)
                    {
                        userState = "已冻结";
                    }
                    else
                    {
                        userState = "正常";
                    }
                    content += string.Format("<tr><td id='queryResult' name='queryResult" + i + "'>{0}</td><td name='state" + i + "'>{1}</td><td><button value='select' name='" + i + "' onclick='Block(this);'>冻结</button></td><td><button value='select' name='" + i + "' onclick='Delete(this);'>删除</button></td></tr>", queryList[i].UserName, userState);
                }
                html = html.Replace("$input", "<input id='inputName' type='text' placeholder='请输入姓名'/><button value='select' onclick='Add(this);'>添加</button>");
            }
            else if(displayStr.Equals("教师"))
            {
                for (int i = 0; i < queryList.Count; i++)
                {
                    content += string.Format("<tr><td id='queryResult' name='queryResult" + i + "'>{0}</td><td><button value='select' name='" + i + "' onclick='Delete(this);'>删除</button></td></tr>", queryList[i].UserName);
                }
            }
            else if(displayStr.Equals("学生") || displayStr.Equals("题库") || displayStr.Equals("成绩"))
            {
                for (int i = 0; i < queryList.Count; i++)
                {
                    content += string.Format("<tr><td id='queryResult' name='queryResult" + i + "'>{0}</td><td><input name='BatchDelTarget' type='checkbox'></input></td><td><button value='select' name='" + i + "' onclick='Delete(this);'>删除</button></td></tr>", queryList[i].UserName);
                }
                html = html.Replace("$batchDelete", "<button value='select' onclick='BatchDelete();'>批量删除</button>");
            }
            html = html.Replace("$content", content).Replace("$input", "").Replace("$batchDelete", "").Replace("$col1", displayStr).Replace("style=\"display:none\"", "style=\"display:table\"").Replace("style=\"display:table-row\"", "style=\"display:none\"").Replace("style=\"display:table-cell\"", "style=\"display:none\"");
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