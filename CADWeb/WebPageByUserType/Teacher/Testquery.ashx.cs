using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace CADWeb.WebPageByUserType.Teacher
{
    /// <summary>
    /// Testquery 的摘要说明
    /// </summary>
    public class Testquery : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            string thisclass = context.Request.Form["DropDownList1"];
            if (context.Session["class"] != null)
            {
                if (!(thisclass.Equals(context.Session["class"].ToString())))
                {
                    context.Session["class"] = thisclass;
                }
            }
            else { context.Session.Add("class", thisclass); }
            context.Response.Redirect("TestData.aspx?i=0");
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