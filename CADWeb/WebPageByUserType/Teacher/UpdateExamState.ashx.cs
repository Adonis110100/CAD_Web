using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb.WebPageByUserType.Teacher
{
    /// <summary>
    /// UpdateExamState 的摘要说明
    /// </summary>
    public class UpdateExamState : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            string className = request["className"].ToString().Replace('，', '|');
            string examName = request["examName"].ToString();
            SQLQuery query = new SQLQuery();
            query.UpdateExamState(examName, className);
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