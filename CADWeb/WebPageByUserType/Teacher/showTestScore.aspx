<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="showTestScore.aspx.cs" Inherits="CADWeb.WebPageByUserType.Teacher.showTestScore" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="CADWeb.SQL" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script src="../../js/jquery-1.7.2.min.js"></script>
    <link rel="stylesheet" href="../css/style.css">
</head>
<body>
    <%
        string school = HttpUtility.UrlDecode(Request.Params["school"]);
        string testID = Request.QueryString["testName"];
        string class1 = Request.QueryString["class"];
        //数据库操作
        SqlConnection conn = SQLConnect.GetConnection();
        conn.Open();
        string sql = "use [CAD__"+school+"] select * from " + testID + " where 年级班级='" + class1 + "'";
        SqlCommand command = new SqlCommand(sql, conn);
        SqlDataReader sr = command.ExecuteReader();
        string divname = testID + class1;
        %>
    <div id="score" >
        <h3 id="lbl_innum_link"><%=divname %></h3>
    <table border="1" >
               <tr>
                   <th>学号</th>
                   <th>姓名</th>
                   <th>答题时间</th>
                   <th>答题数</th>
                   <th>成绩</th>
               </tr>
        <%
            while (sr.Read())
            {                           
                string stuName="";
                string stuID="";
                string anTime="";
                string no="";
                int Score=0;
                if (sr["学号"] != DBNull.Value)
                {
                    stuID = sr["学号"].ToString();
                }
                if (sr["姓名"] != DBNull.Value)
                {
                    stuName = sr["姓名"].ToString();
                }if (sr["答题时间"] != DBNull.Value)
                {
                    anTime = sr["答题时间"].ToString();
                }if (sr["答题数"] != DBNull.Value)
                {
                    no = sr["答题数"].ToString();
                }if (sr["得分"] != DBNull.Value)
                {
                    Score =Convert.ToInt32(sr["得分"].ToString());
                }
            %>
               <tr>
                   <td><%=stuID%></td>
                   <td><%=stuName%></td>
                   <td><%=no%></td>
                   <td><%=anTime%></td>
                   <td><%=Score %></td>
               </tr>

        
        <%}
            sr.Close();           
            %>
    </table><br>
         <input id="Button1" type="button" value="导出EXCEL" class="rbtn23" onclick="HtmlExportToExcel('score')" />
    </div>   
    <%
        string sqlname = "select distinct 姓名 from  classInfo_"+class1+" where 学号 not in (select 学号 from "+testID+")";
        command = new SqlCommand(sqlname,conn);
        sr = command.ExecuteReader();
        string name = "";
        while (sr.Read())
        {
            if (name.IndexOf(",") == -1)
            {
                name = sr.GetString(0);
            }
            else {
                name = name + "," + sr.GetString(0);
            }
        }
        sr.Close();
        conn.Close();
        %>
        <div><P>未完成此考试（作业）的同学还有:<%=name %></P></div>
         <script >          
         function TabColRemove(_this, isDellast) {
            if (isDellast) {
                $(_this).parent().parent().parent().find("tr td:nth-child(" + ($(_this).parent().index() + 1) + ")").remove();
            } else {
                $(_this).parent().parent().parent().find("tr:not(:last-child)").find("td:nth-child(" + ($(_this).parent().index() + 1) + ")").remove();
            }
         }
        //jQuery HTML导出Excel文件(兼容IE及所有浏览器)
        function HtmlExportToExcel(tableid) {          
            var filename = $('#lbl_innum_link').text();
            if (getExplorer() == 'ie' || getExplorer() == undefined) {
                HtmlExportToExcelForIE(tableid, filename);
            }
            else {
                HtmlExportToExcelForEntire(tableid, filename)
            }
        }
        //IE浏览器导出Excel
        function HtmlExportToExcelForIE(tableid, filename) {
            try {
                var winname = window.open('', '_blank', 'top=10000');
                var strHTML = document.getElementById(tableid).innerHTML;
 
                winname.document.open('application/vnd.ms-excel', 'export excel');
                winname.document.writeln(strHTML);
                winname.document.execCommand('saveas', '', filename + '.xls');
                winname.close();
 
            } catch (e) {
                alert(e.description);
            }
        }
        //非IE浏览器导出Excel
        var HtmlExportToExcelForEntire = (function() {
            var uri = 'data:application/vnd.ms-excel;base64,',
        template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
        base64 = function(s) { return window.btoa(unescape(encodeURIComponent(s))) },
        format = function(s, c) { return s.replace(/{(\w+)}/g, function(m, p) { return c[p]; }) }
            return function(table, name) {
                if (!table.nodeType) { table = document.getElementById(table); }
                var ctx = { worksheet: name || 'Worksheet', table: table.innerHTML }
                document.getElementById("score").href = uri + base64(format(template, ctx));
                document.getElementById("score").download = name + ".xls";
                document.getElementById("score").click();
            }
        })()
        function getExplorer() {
            var explorer = window.navigator.userAgent;
            //ie 
            if (explorer.indexOf("MSIE") >= 0) {
                return 'ie';
            }
            //firefox 
            else if (explorer.indexOf("Firefox") >= 0) {
                return 'Firefox';
            }
            //Chrome
            else if (explorer.indexOf("Chrome") >= 0) {
                return 'Chrome';
            }
            //Opera
            else if (explorer.indexOf("Opera") >= 0) {
                return 'Opera';
            }
            //Safari
            else if (explorer.indexOf("Safari") >= 0) {
                return 'Safari';
            }
        }
       
    </script>
</body>
</html>
