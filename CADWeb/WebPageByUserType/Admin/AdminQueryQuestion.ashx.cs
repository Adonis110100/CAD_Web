using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb.WebPageByUserType.Admin
{
    /// <summary>
    /// AdminQueryQuestion 的摘要说明
    /// </summary>
    public class AdminQueryQuestion : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            SQLQuery query = new SQLQuery();
            string questionName = context.Request["questionName"];
            string operationType = context.Request["operationType"];
            int qState = operationType.Equals("正常") ? 1 : 0;
            string questionType = HttpUtility.HtmlDecode(context.Request["questionType"]).TrimStart('<').TrimEnd('>') + "库";
            query.UpdateQuestionState(questionName, qState, questionType);
            context.Response.Write(qState);
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