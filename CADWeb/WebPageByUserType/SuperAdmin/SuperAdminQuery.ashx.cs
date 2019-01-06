using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb.WebPageByUserType.SuperAdmin
{
    /// <summary>
    /// SuperAdminQuery 的摘要说明
    /// </summary>
    public class SuperAdminQuery : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string school = HttpUtility.UrlDecode(context.Request.Params["School"]);
            SQLQuery query = new SQLQuery();
            string queryType = context.Request["queryType"].Trim();
            string name = context.Request["name"].Trim();
            string type = context.Request["type"].Trim();
            if (type.Equals("成绩"))
            {
                if (queryType.Equals("Delete"))
                {
                    string testName = context.Request["testName"];
                    testName = HttpUtility.HtmlDecode(testName).TrimStart('{').TrimEnd('}');
                    query.DeleteTestScoreData(name, "use [CAD__" + school + "] delete from testScore_" + testName + " where 学号=@name");
                }
                else if (queryType.Equals("BatchDelete"))
                {
                    name = HttpUtility.HtmlDecode(name);
                    string[] delArray = name.TrimEnd('|').Split('|');
                    string[] delObject;
                    foreach (string temp in delArray)
                    {
                        delObject = temp.Split(':');
                        query.DeleteTestScoreData(delObject[1], "use [CAD__" + school + "] delete from testScore_" + delObject[0].TrimStart('{').TrimEnd('}') + " where 学号=@name");
                    }
                }
            }
            else if (type.Equals("学生"))
            {
                if (queryType.Equals("Delete"))
                {
                    string className = context.Request["className"];
                    className = HttpUtility.HtmlDecode(className).TrimStart('{').TrimEnd('}');
                    query.DeleteTestScoreData(name, "use [CAD__" + school + "] delete from classInfo_" + className + " where 学号=@name");
                }
                else if (queryType.Equals("BatchDelete"))
                {
                    name = HttpUtility.HtmlDecode(name);
                    string[] delArray = name.TrimEnd('|').Split('|');
                    string[] delObject;
                    foreach (string temp in delArray)
                    {
                        delObject = temp.Split(':');
                        query.DeleteTestScoreData(delObject[1], "use [CAD__" + school + "] delete from classInfo_" + delObject[0].TrimStart('{').TrimEnd('}') + " where 学号=@name");
                    }
                }
            }
            else
            {
                switch (queryType)
                {
                    case "Add":
                        if (type.Equals("学校名"))
                            query.SchoolManage(name, queryType, type);
                        else if (type.Equals("管理员"))
                        {
                            query.SuperAdminOperation(name, queryType, type);
                        }
                        break;
                    case "Block":
                    case "Unblock":
                        if (type.Equals("学校名"))
                            query.SchoolManage(name, queryType, type);
                        else
                            query.SuperAdminOperation(name, queryType, type);
                        break;
                    case "Delete":
                        if (type.Equals("学校名"))
                            query.SchoolManage(name, queryType, type);
                        else
                            query.SuperAdminOperation(name, queryType, type);
                        break;
                    case "BatchDelete":
                        string[] deleteStr = name.TrimEnd('|').Split('|');
                        query.SuperAdminBatchDelete(deleteStr, type);
                        break;
                }
            }
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