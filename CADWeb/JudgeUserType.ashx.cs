using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb
{
    /// <summary>
    /// JudgeUserType 的摘要说明
    /// </summary>
    public class JudgeUserType : IHttpHandler
    {
        private UserInfo userInfo;

        public JudgeUserType(UserInfo userInfo)
        {
            this.userInfo = userInfo;
        }

        public void ProcessRequest(HttpContext context)
        {
            HttpResponse response = context.Response;
            //context.Response.ContentType = "text/html";
            SQLQuery query = new SQLQuery();
            string userType = query.QueryUserType(userInfo.UserName);
            if (!string.IsNullOrEmpty(userType))
            {
                userInfo.UserType = userType;
                response.Write("<script> $.cookie('PageTransferUserType', '" + userType + "') </script>");
                switch (userType)
                {
                    case "Student":
                        response.Write("<script languge='javascript'> window.location.href = 'PageTransfer.html?userType=" + userType + "' </script>");
                        break;
                    case "Teacher":
                        response.Write("<script languge='javascript'> window.location.href = 'PageTransfer.html?userType=" + userType + "' </script>");
                        break;
                    case "Admin":
                        response.Write("<script languge='javascript'> window.location.href = 'PageTransfer.html?userType=" + userType + "' </script>");
                        break;
                    case "SuperAdmin":
                        response.Write("<script languge='javascript'> window.location.href = 'PageTransfer.html?userType=" + userType + "' </script>");
                        break;
                }
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