using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb.WebPageByUserType.Admin
{
    /// <summary>
    /// AdminQuery 的摘要说明
    /// </summary>
    public class AdminQuery : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            SQLQuery query = new SQLQuery();
            string operation = request["operation"].Trim();
            string name = request["name"].Trim();
            string userType = request["userType"].Trim();
            query.AdminOpertion(name, operation, userType);
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