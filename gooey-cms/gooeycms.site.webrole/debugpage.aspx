<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="debugpage.aspx.cs" Inherits="Gooeycms.webrole.sites.debugpage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="BtnTest" Text="Test" OnClick="BtnTest_Click" runat="server" />
        <asp:Label ID="Token" runat="server" />

        <asp:HyperLink ID="HyperLink" OnPreRender="AppendQuerystring" NavigateUrl="./debugpas.aspx" Text="Click Here" runat="server" />

        <img src="./image.aspx" />
    </div>
    </form>
</body>
</html>
