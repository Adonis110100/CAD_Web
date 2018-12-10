using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;
namespace WebApplication2
{
    /// <summary>
    /// AddDraw1 的摘要说明
    /// </summary>
    public class AddDraw1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string page = "1";
            if (context.Request["page"] != null)
                page = context.Request["page"];
            string str = context.Request["DrawID"].ToString();
            if (context.Session["DrawID"] != null&& !(context.Session["DrawID"].ToString().Equals("")))
            {
                string IdSession = context.Session["DrawID"].ToString();
                
                
                if (IdSession.IndexOf(str) == -1)
                {
                    Regex regex = new Regex(",");
                    if (regex.Matches(IdSession, 0).Count >= 2)
                    {
                        context.Response.Write("<script>alert(',,,,')</script>");
                        context.Response.Redirect("AddDraw.aspx?page=" + page);
                        return;
                    }
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
                context.Session["DrawID"] = IdSession;
            }
            else
            {
                context.Session.Add("DrawID", str);
            }
            context.Response.ContentType = "text/html";
            context.Response.Redirect("AddDraw.aspx?page=" + page);
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