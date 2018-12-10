<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddTest.aspx.cs" Inherits="WebApplication2.AddTest" %>

    <!DOCTYPE html>

    <html xmlns="http://www.w3.org/1999/xhtml">

    <head runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <link rel="stylesheet" href="../css/style.css">
        <title></title>
        <script src='../../js/jquery-1.7.2.min.js'></script>
        <script>
            function changeit(url) {
                document.getElementById("addQuestion").src = url;
                document.getElementById('addQuestion').height = 0;
                $('#addQuestion').load(function() {
                    document.getElementById('addQuestion').height = document.getElementById('addQuestion').contentWindow.document.body.offsetHeight + 'px';
                })
            }
        </script>
        <style type="text/css">
            .auto-style1 {
                height: 23px;
            }
        </style>
    </head>

    <body>
        <div id="banner">&nbsp;&nbsp;教师系统</div>

        <div id="center">
            <br>
            <HR width="100%" color=#E4E4E4 SIZE=2>
            <form runat="server">
                <input type="button" class="but_add3" value="添加选择题" onclick=" changeit('AddChoice.aspx')" />
                <input type="button" class="but_add3" value="添加判断题" onclick=" changeit('AddJudge.aspx')" />
                <input type="button" class="but_add3" value="添加绘图题" onclick=" changeit('AddDraw.aspx')" />
                <br>
                <iframe id="addQuestion" class="iframe" frameborder="0" src="Default.html"></iframe>
                <table>
                    <tr>
                        <td style="width:20%">
                            <p>试卷名</p>
                        </td>
                        <td>
                            <asp:TextBox ID="name" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p>试卷类型</p>
                        </td>
                        <td>
                            <asp:TextBox ID="testType" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            <p>总分</p>
                        </td>
                        <td class="auto-style1">
                            <asp:TextBox ID="score" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <p>答题时长</p>
                        </td>
                        <td>
                            <asp:TextBox ID="time" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="submit" runat="server" Text="完成组卷 " OnClick="submit_Click" style="float:none" />
                        </td>
                    </tr>
                </table>
            </form>
        </div>
        <br><br><br><br><br>
        <div id="foot"></div>
    </body>

    </html>