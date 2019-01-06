using CADWeb.SQL;
using System;
using System.Collections;
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
            string school = HttpUtility.UrlDecode(request.Params["School"]);
            SQLQuery query = new SQLQuery();
            IList queryList = new List<Object>();
            List<UserInfo> queryListUserInfo = new List<UserInfo>();
            List<string> queryListQuestion = new List<string>();
            List<string> queryListScore = new List<string>();
            string html = File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"WebPageByUserType\SuperAdmin\SuperAdmin.html");
            string content = "";
            string displayStr = "";
            string serialNum = request.Form["select_Query"].ToString();
            switch (serialNum)
            {
                case "学校名添加、冻结、解冻、删除":
                    displayStr = "学校名";
                    queryListUserInfo = query.UserQuery(1,school) as List<UserInfo>;
                    break;
                case "管理员添加、冻结、解冻、删除":
                    displayStr = "管理员";
                    queryListUserInfo = query.UserQuery(2,school) as List<UserInfo>;
                    break;
                case "教师删除":
                    displayStr = "教师";
                    queryListUserInfo = query.UserQuery(3,school) as List<UserInfo>;
                    break;
                case "学生批量删除":
                    displayStr = "学生";
                    queryListUserInfo = query.UserQuery(4,school) as List<UserInfo>;
                    break;
                case "题库批量删除":
                    displayStr = "题库";
                    queryListQuestion = query.UserQuery(5,school) as List<string>;
                    break;
                case "成绩批量删除":
                    displayStr = "成绩";
                    queryListScore = query.UserQuery(6,school) as List<string>;
                    break;
            }
            if (displayStr.Equals("学校名") || displayStr.Equals("管理员"))
            {
                string userState = "";
                if (displayStr.Equals("学校名"))
                {
                    for (int i = 0; i < queryListUserInfo.Count; i++)
                    {
                        if (queryListUserInfo[i].UserState == 0)
                        {
                            userState = "已冻结";
                        }
                        else
                        {
                            userState = "正常";
                        }
                        content += string.Format(
                            "<tr>" +
                                "<td id='queryResult' name='queryResult" + i + "'>{0}</td>" +
                                "<td name='state" + i + "'>{1}</td>" +
                                "<td><button value='select' name='" + i + "' onclick='BlockOrUnblock_SuperAdmin(this, \"已冻结\");'>冻结</button></td>" +
                                "<td><button value='select' name='" + i + "' onclick='BlockOrUnblock_SuperAdmin(this, \"正常\");'>解冻</button></td>" +
                                "<td><button value='select' name='" + i + "' onclick='Delete(this,\"" + displayStr + "\",-1);'>删除</button></td>" +
                            "</tr>", queryListUserInfo[i].School, userState);
                    }
                }
                else
                {
                    for (int i = 0; i < queryListUserInfo.Count; i++)
                    {
                        if (queryListUserInfo[i].UserState == 0)
                        {
                            userState = "已冻结";
                        }
                        else
                        {
                            userState = "正常";
                        }
                        content += string.Format(
                            "<tr>" +
                                "<td id='queryResult' name='queryResult" + i + "'>{0}</td>" +
                                "<td name='state" + i + "'>{1}</td>" +
                                "<td><button value='select' name='" + i + "' onclick='BlockOrUnblock_SuperAdmin(this, \"已冻结\");'>冻结</button></td>" +
                                "<td><button value='select' name='" + i + "' onclick='BlockOrUnblock_SuperAdmin(this, \"正常\");'>解冻</button></td>" +
                                "<td><button value='select' name='" + i + "' onclick='Delete(this,\"" + displayStr + "\",-1);'>删除</button></td>" +
                            "</tr>", queryListUserInfo[i].UserName, userState);
                    }
                }
                html = html.Replace("$input", "<input id='inputName' type='text' placeholder='请输入姓名'/><button value='select' onclick='Add(this);'>添加</button>");
            }
            else if (displayStr.Equals("教师"))
            {
                for (int i = 0; i < queryListUserInfo.Count; i++)
                {
                    content += string.Format(
                        "<tr>" +
                            "<td id='queryResult' name='queryResult" + i + "'>{0}</td>" +
                            "<td><button value='select' name='" + i + "' onclick='Delete(this,\"" + displayStr + "\",-1);'>删除</button></td>" +
                        "</tr>", queryListUserInfo[i].UserName);
                }
            }
            else
            {
                if (displayStr.Equals("学生"))
                {
                    int x = -1;
                    int y = 0;
                    for (int i = 0; i < queryListUserInfo.Count; i++)
                    {
                        if (queryListUserInfo[i].UserName.Contains("{"))
                        {
                            x++;
                            content += string.Format(
                                "<tr>" +
                                    "<th id='className_" + x + "'>{0}</th>" +
                                "<tr>", queryListUserInfo[i].UserName);
                        }
                        else
                        {
                            content += string.Format(
                                "<tr>" +
                                    "<td id='queryResult" + y + "' name='queryResult" + x + "'>{0}</td>" +
                                    "<td>{1}</td><td><input name='BatchDelTarget' type='checkbox'></input></td>" +
                                    "<td><button value='select' name='" + x + "' onclick='Delete(this,\"" + displayStr + "\"," + y + ");'>删除</button></td>" +
                                "</tr>", queryListUserInfo[i].UserPassword, queryListUserInfo[i].UserName);
                            y++;
                        }
                    }
                    html = html.Replace("请先进行查询操作", " 学号</th><th>姓名");
                }
                else if (displayStr.Equals("题库"))
                {
                    int x = 0;
                    for (int i = 0; i < queryListQuestion.Count; i++)
                    {
                        if(queryListQuestion[i].Equals("<选择题>") || queryListQuestion[i].Equals("<判断题>") || queryListQuestion[i].Equals("<作图题>"))
                        {
                            content += string.Format("<tr><th>{0}</th><tr>", queryListQuestion[i]);
                        }
                        else
                        {
                            content += string.Format(
                                "<tr>" +
                                    "<td id='queryResult' name='queryResult" + x + "'>{0}</td>" +
                                    "<td><input name='BatchDelTarget' type='checkbox'></input></td>" +
                                    "<td><button value='select' name='" + x + "' onclick='Delete(this,\"" + displayStr + "\",-1);'>删除</button></td>" +
                                "</tr>", queryListQuestion[i].Split('|')[0]);
                            x++;
                        }
                    }
                }
                else if (displayStr.Equals("成绩"))
                {
                    int x = -1;
                    int y = 0;
                    string[] scoreInfo;
                    for (int i = 0; i < queryListScore.Count; i++)
                    {
                        if (queryListScore[i].Contains("{"))
                        {
                            x++;
                            content += string.Format("<tr>" +
                                "<th id='testName_"+x+"'>{0}</th>" +
                                "<tr>", queryListScore[i]);
                        }
                        else
                        {
                            scoreInfo = queryListScore[i].Split('|');
                            content += string.Format(
                                "<tr>" +
                                    "<td id='queryResult" + y + "' name='queryResult" + x + "'>{0}</td>" +
                                    "<td>{1}</td>" +
                                    "<td>{2}</td>" +
                                    "<td>{3}</td>" +
                                    "<td><input name='BatchDelTarget' type='checkbox'></input></td>" +
                                    "<td><button value='select' name='" + x + "' onclick='Delete(this,\"" + displayStr + "\"," + y + ");'>删除</button></td>" +
                                "</tr>", scoreInfo[0], scoreInfo[1], scoreInfo[2], scoreInfo[3]);
                            y++;
                        }
                    }
                    //html=html.Remove(html.LastIndexOf("<th></th>"));
                    html = html.Replace("请先进行查询操作", " 学号</th><th>姓名</th><th>年级班级</th><th>得分");
                }
                html = html.Replace("$batchDelete", "<button value='select' onclick='BatchDelete(\"" + displayStr + "\");'>批量删除</button>");
            }
            html = html.Replace("请先进行查询操作", "").Replace("$content", content).Replace("$input", "")
                .Replace("$batchDelete", "").Replace("$title", displayStr).Replace("style=\"display:none\"", "style=\"display:table\"")
                .Replace("style=\"display:table-row\"", "style=\"display:none\"").Replace("style=\"display:table-cell\"", "style=\"display:none\"");
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