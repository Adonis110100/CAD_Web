using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
namespace WebApplication2
{
    /// <summary>
    /// AddChoice1 的摘要说明
    /// </summary>
    public class AddChoice1 : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {

            try
            {
                string page = "1";
                if (context.Request["page"] != null)
                    page = context.Request["page"];
                string str = context.Request["ChoiceID"].ToString();
                if (context.Session["ChoiceID"] != null && !(context.Session["ChoiceID"].ToString().Equals("")))
                {
                    string IdSession = context.Session["ChoiceID"].ToString();
                    if (IdSession.IndexOf(str) == -1)
                    {
                        IdSession = IdSession + "," + str;
                    }
                    else
                    {
                        if (IdSession.IndexOf(str) == 0) {
                            IdSession = IdSession.Replace(str, "");
                        }
                        else
                        {
                            IdSession = IdSession.Replace("," + str, "");
                        }
                    }
                    context.Session["ChoiceID"] = IdSession;
                }
                else
                {
                    context.Session.Add("ChoiceID", str);
                }
                context.Response.ContentType = "text/html";               
                context.Response.Redirect("AddChoice.aspx?page=" + page);
            }
            catch (Exception e)
            {
                context.Response.Write(e.Message);
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