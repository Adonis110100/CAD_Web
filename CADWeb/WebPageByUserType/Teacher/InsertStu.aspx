<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InsertStu.aspx.cs" Inherits="CADWeb.WebPageByUserType.Teacher.InsertStu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <script type="text/javascript" src="../../js/jquery-1.7.2.min.js"></script>
    <script type="text/javascript" src="../../js/custom.js"></script>
    <link rel='stylesheet' href='../css/style.css'>
</head>
<body>
        <div id="banner">&nbsp;&nbsp;导入学生名单</div>
        <div id="center">
            <br>
            <HR width="100%" color=#E4E4E4 SIZE=2>
                <br>

    <form id="form1" runat="server">
        <div>
             <div class="tips" style="vertical-align: middle;text-align: center;">
                <p>导入注意事项：</p><br />
                <p>示例如下，依次为学号，姓名，密码，学生状态（默认为1，填1就好） </p>
                <img src="../../image/xueshengshili.png" /><br />
                <p>请将sheet页的页名改为学生，示例如下：</p><br />
                将<img src="../../image/xueshengshili2.png" />改为<img src="../../image/xueshengshili3.png" /><br /><br /><br />
                </div>
             <asp:FileUpload ID="MyFileUpload" runat="server" />
            <br/><br/>年级班级:<asp:TextBox ID="ClassName" runat="server"></asp:TextBox>
            <br/><br/><br/>
        <asp:Button ID="FileUploadButton" runat="server" Text="上传" 
            onclick="FileUploadButton_Click" />
        </div>
    </form>
    </div>
    <div id="foot"></div>
</body>
</html>
