<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddQuestion3.aspx.cs" Inherits="WebApplication2.AddQuestion3" %>
<%@ Import Namespace="System.Data.SqlClient" %>
<%@ Import Namespace="System.Data" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
<link rel='stylesheet' href='../css/style.css'>
    <title></title>
</head>
<style>
table{
    width: 800px;
}
</style>

<body>
    <form id="form1" runat="server">
        <div>
            <table border="1">                
                <% if (Session["testID"] == null){
                        Response.Redirect("AddQuestion3.aspx");
                    }
                    string testID = Session["testID"].ToString();
                    %>
                <tr><td>当前题号为：<%=testID %></td></tr>
                <tr><td> <a href="../../CanvasEXE/Canvas.application?testID=<%=testID %>&status=出题" style="text-decoration:none">添加绘图题</a></td></tr>
                <tr><td> <a href="../../CanvasEXE/Canvas.application?testID=<%=testID %>&status=标准答案"style="text-decoration:none">添加绘图题答案</a></td></tr>
                <tr><td><p style="color:red">tip:添加完绘图题题目后马上添加答案以及题目文字描述</p></td></tr>
                <tr><td><p style="color:red">题目信息(必填):</p><asp:textBox ID="timu" runat="server" Width="715px"></asp:textBox></td></tr>
                <tr><td><asp:Button ID="submit" runat="server" text="提交" OnClick="sumbit_Click" style="float: none;"/><asp:Button ID="next" runat="server" text="添加下一题"  style="float: none; margin-top: 0px;" OnClick="next_Click" Width="95px"/></td><td>&nbsp;</td></tr>
           </table>
        </div>
    </form>
</body>
</html>
