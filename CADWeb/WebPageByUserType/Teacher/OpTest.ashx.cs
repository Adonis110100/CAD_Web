using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb.WebPageByUserType.Teacher
{
    /// <summary>
    /// stopTest 的摘要说明
    /// </summary>
    public class OpTest : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            string testID = context.Request["testID"];
            int op= Convert.ToInt32(context.Request["testID"]);
            if (op == 1)//关闭试卷作答
            {

            }
            else if (op == 2)//开启试卷作答
            {

            }
            else//显示学生成绩
            {
                context.Response.Redirect("showTestScore.aspx?testID="+testID);
            }
            
            ///数据库操作
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