using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace WebApplication2
{
    /// <summary>
    /// AddJudge1 的摘要说明
    /// </summary>
    public class AddJudge1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string page = "1";
                if (context.Request["page"] != null)
                    page = context.Request["page"];
                string str = context.Request["JudgeID"].ToString();
                if (context.Session["JudgeID"] != null && !(context.Session["JudgeID"].ToString().Equals("")))
                {
                    string IdSession = context.Session["JudgeID"].ToString();
                    if (IdSession.IndexOf(str) == -1)
                    {
                        IdSession = IdSession + "," + str;
                    }
                    else
                    {
                        if (IdSession.IndexOf(str) == 0) { IdSession = IdSession.Replace(str, ""); }
                        else
                        {
                            IdSession = IdSession.Replace("," + str, "");
                        }
                    }
                    context.Session["JudgeID"] = IdSession;
                }
                else
                {
                    context.Session.Add("JudgeID", str);
                }
                context.Response.ContentType = "text/html";
                context.Response.Redirect("AddJudge.aspx?page=" + page);
            }
            catch (Exception)
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