<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="gooeycms.webrole.debug.paypal.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="BtnCreateSubscription" OnClick="BtnCreate_Click" Text="Create Subscription" runat="server" />
        <asp:Label ID="LblStatus" runat="server" />
    </div>
    </form>
</body>
</html>
