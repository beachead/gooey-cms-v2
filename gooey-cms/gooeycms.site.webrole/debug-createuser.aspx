<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="debug-createuser.aspx.cs" Inherits="WebRole1.debug_createuser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox ID="Username" runat="server" />
        <asp:TextBox ID="Roles" runat="server" />
        <asp:Button ID="BtnSave" OnClick="BtnSave_Click" runat="server" />
    </div>
    </form>
</body>
</html>
