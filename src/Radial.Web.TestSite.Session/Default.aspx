<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Radial.Web.TestSite.Session.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnSet" runat="server" Text="设置Session" 
            onclick="btnSet_Click" />
        <asp:Button ID="btnRead" runat="server" Text="读取Session" 
            onclick="btnRead_Click" />
        <p>
            <asp:Literal ID="litValue" runat="server"></asp:Literal>
        </p>
    </div>
    </form>
</body>
</html>
