using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb
{
    /// <summary>
    /// LoginRequest 的摘要说明
    /// </summary>
    public class LoginRequest : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            response.ContentType = "text/html";
            string userName = request.Params["logname"];
            string userPass = request.Params["logpass"];

            SQLQuery query = new SQLQuery();
            string queryResult = query.IsUserExist(userName, userPass);
            string[] str = queryResult.Split('|');
            string isUserExist = str[0];
            string userState = str[1];

            if ("true".Equals(isUserExist) && "1".Equals(userState))
            {
                UserInfo userInfo = new UserInfo();
                userInfo.UserName = userName;
                userInfo.UserPassword = userPass;

                response.Write("<script type='text/javascript' src='js/jquery-1.7.2.min.js'></script>");
                response.Write("<script type='text/javascript' src='js/jquery.cookie.js'></script>");
                response.Write("<script> $.cookie('PageTransferUserName', '" + userName + "'); alert('成功进入！'); </script>");
                response.Flush();
                //判断用户类型跳转至功能不同的系统
                JudgeUserType judgeUserType = new JudgeUserType(userInfo);
                judgeUserType.ProcessRequest(context);
                //context.Server.Transfer(judgeUserType, false);
            }
            else if ("true".Equals(isUserExist) && "0".Equals(userState))
            {
                response.Write("<script languge='javascript'> alert('账户已冻结，请重新登录或联系管理员处理！'); window.location.href='Login.html' </script>");
            }
            else if ("false".Equals(isUserExist))
            {
                response.Write("<script languge='javascript'> alert('用户名或密码错误，请重新输入！'); window.location.href='Login.html' </script>");
            }
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