using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb.WebPageByUserType.Student
{
    /// <summary>
    /// SubmitAnswer 的摘要说明
    /// </summary>
    public class SubmitAnswer : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            HttpResponse response = context.Response;
            SQLQuery query = new SQLQuery();
            string[] stuAnswer = request["studentAnswer"].ToString().TrimEnd('|').Split('|');
            string examName = request["examName"].ToString();
            string[] stuInfo = request["stuInfo"].ToString().Split('|');
            int count = Convert.ToInt32(request["count"]);
            long time = Convert.ToInt64(request["time"]);
            query.InsertStudentTestScore(stuInfo[4],examName, stuInfo[0], stuInfo[1], stuInfo[2], stuAnswer[0].TrimEnd(','),
                stuAnswer[1].TrimEnd(',').Replace("True","正确").Replace("False", "错误"), time, count);
            query.UpdateAnswerStateInClass(stuInfo[4],stuInfo[2], examName, stuInfo[1]);
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