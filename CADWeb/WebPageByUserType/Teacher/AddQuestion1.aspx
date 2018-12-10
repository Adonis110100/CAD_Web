﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddQuestion1.aspx.cs" Inherits="WebApplication2.AddQuestion1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel='stylesheet' href='../css/style.css'>
    <title></title>
</head>
<style>
    table {
        width: 800px;
    }
</style>

<body>
    <form id="form1" runat="server">
        
            <asp:Button ID="ChoiceToSql" runat="server" text="导入选择题" OnClick="ChoiceToSql_Click" />
            <span>tip:excel导入多条选择题，不支持图片导入</span>
        <br>
        <asp:FileUpload ID="MyFileUpload" runat="server" Visible="false" />
        <asp:Button ID="FileUploadButton" runat="server" Visible="false" Text="上传" onclick="FileUploadButton_Click" />
        
        <table id="Add" border="1">
            <tr>
                <td style="width:20%">问题：&nbsp;</td>
                <td>
                    <asp:TextBox ID="question" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>A:&nbsp;&nbsp;</td>
                <td>
                    <asp:TextBox ID="A" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>B:&nbsp;&nbsp;</td>
                <td>
                    <asp:TextBox ID="B" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>C:&nbsp;&nbsp;</td>
                <td>
                    <asp:TextBox ID="C" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>D:&nbsp;&nbsp;</td>
                <td>
                    <asp:TextBox ID="D" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>答案:</td>
                <td>
                    <asp:TextBox ID="answer" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>答案解析:</td>
                <td>
                    <asp:TextBox ID="answerjx" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>附加图片:</td>
                <td>

                    <asp:FileUpload ID="questionPic" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Button ID="AdQuestion" runat="server" text="添加" OnClick="AdQuestion_Click" style="float: none;"/>
                </td>
            </tr>
        </table>

    </form>

</body>

</html>