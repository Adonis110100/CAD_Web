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
            SQLQuery query = new SQLQuery();
            string queryType = context.Request["queryType"].Trim();
            string name = context.Request["name"].Trim();
            switch (queryType)
            {
                case "Add":
                    string type = context.Request["type"].Trim();
                    if (type.Equals("学校名"))
                    {
                        //TODO

                    }
                    else if (type.Equals("管理员"))
                    {
                        query.SuperAdminOperation(name, queryType);
                    }
                    break;
                case "Block":
                    query.SuperAdminOperation(name, queryType);
                    break;
                case "Delete":
                    query.SuperAdminOperation(name, queryType);
                    break;
                case "BatchDelete":
                    string[] deleteStr = name.TrimEnd('|').Split('|');
                    query.SuperAdminBatchDelete(deleteStr);
                    break;
            }
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