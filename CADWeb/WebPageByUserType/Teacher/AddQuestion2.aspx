﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddQuestion2.aspx.cs" Inherits="WebApplication2.AddQuestion2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link rel='stylesheet' href='../css/style.css'>
    <style type="text/css">
        .auto-style1 {
            height: 22px;
        }
        table{
            width: 800px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        
            <asp:Button ID="ChoiceToSql" runat="server" text="导入判断题" OnClick="judgeToSql_Click" />
         <div class="tips" style="vertical-align: middle;text-align: center;">
                <p>导入注意事项：</p><br />
                <p>示例如下，依次为题目,答案（正确或者错误）,答案解析,题目状态（默认为1）</p>
                <img src="../../image/panduantishili1.png" /><br />
                <p>请将sheet页的页名改为判断题，示例如下：</p><br />
                将<img src="../../image/xueshengshili2.png" />改为<img src="../../image/panduantishili2.png" /><br /><br /><br />
                <span>tip:excel导入多条选择题，不支持图片导入</span>
                 <br />
                </div>           
            <asp:FileUpload ID="MyFileUpload" runat="server" Visible="false" />
            <asp:Button ID="FileUploadButton" runat="server" Visible="false" Text="上传" onclick="FileUploadButton_Click" />

            <table border="1">
                <tr>
                    <td style="width:20%">问题：</td>
                    <td><asp:TextBox ID="question" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>答案:</td>
                    <td><asp:TextBox ID="answer" runat="server"></asp:TextBox>(对或者错)</td>
                </tr>
                <tr>
                    <td>答案解析:</td>
                    <td><asp:TextBox ID="answerjx" runat="server"></asp:TextBox>
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
                        <asp:Button ID="AdQuestion" runat="server" text="添加" OnClick="AdQuestion_Click" style="float:none" />
                    </td>
                </tr>
            </table>
        
    </form>
</body>

</html>