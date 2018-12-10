using CADWeb.SQL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CADWeb
{
    /// <summary>
    /// QueryExistSchool 的摘要说明
    /// </summary>
    public class QueryExistSchool : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            SQLQuery query = new SQLQuery();
            string schools = query.QuerySchoolName();
            context.Response.Write(schools.TrimEnd('|'));
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