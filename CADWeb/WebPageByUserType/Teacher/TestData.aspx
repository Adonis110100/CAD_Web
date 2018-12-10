<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestData.aspx.cs" Inherits="CADWeb.WebPageByUserType.Teacher.TestData" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="stylesheet" href="../css/style.css">
    <title></title>
    <script src='../../js/jquery-1.7.2.min.js'></script>
    <script>

    window.onload=function(){
        var ifr = document.getElementById('ifr');
        if(ifr.contentWindow.document.body.offsetHeight>600){
        ifr.height = ifr.contentWindow.document.body.offsetHeight + 'px';    
        }
    }
    </script>
</head>

<body>
    <div id="banner">&nbsp;&nbsp;统计单次和多次成绩，各次学生作答题数统计，学生各次作答时间统计，导出成绩</div>

    <div id="center">
        <br>
        <HR width="100%" color=#E4E4E4 SIZE=2>
        <form id="form1" runat="server" action="Testquery.ashx">
            <div>
                <h2>班级</h2>
                <asp:DropDownList ID="DropDownList1" runat="server"></asp:DropDownList>
                <input type="submit" class='but_search' value="查询" onclick="getClassScoreTable(this)"/>
            </div>
        </form>

        <%
             if (Request["i"] != null) {            
             %>

        <iframe id='ifr' class='iframe' frameborder="0" height="600px" src="showTestData.aspx"></iframe>
        <%} %>

    </div>
    <div id="foot"></div>
</body>

</html>