<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ExcelTest.aspx.cs" Inherits="Radial.Test.WebForm.ExcelTest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <p>
                <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="输出" />
            </p>
            <p>
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="导入" />
                <asp:GridView ID="GridView1" runat="server">
                </asp:GridView>

            </p>

        </div>

    </form>
</body>
</html>
