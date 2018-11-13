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
            string handleType = request["handleType"].Trim();
            string name = request["name"].Trim();
            if("Add".Equals(handleType))
            {

            }
            else if("Block".Equals(handleType))
            {

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