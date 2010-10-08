<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Site.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Developer.Site" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">


<div>
Choose the site you would like to package for sale within the Gooey Store: <br />
<asp:DropDownList ID="LstAvailableSites" runat="server" />
</div>

<div>
Input a title for this site: <br />
<asp:TextBox ID="TxtTitle" runat="server" />
</div>

<div>
Input the price you would like to sell this site for: <br />
<asp:TextBox ID="TxtPrice" runat="server" />
</div>

<div>
Input 3-4 features of this site (one-per line): <br />
<asp:TextBox ID="TxtFeatures" TextMode=MultiLine Rows="5" Columns="50" runat="server" />
</div>

<div>
Choose a category for this site: <br />
<asp:DropDownList ID="LstCategory" runat="server" />
</div>

<div>
<anthem:Button ID="BtnSave" Text="Package &amp; Deploy" OnClick="BtnSave_Click" PostCallBackFunction="deploy_site"  runat="server" />
<anthem:HiddenField ID="SavedPackageGuid" AutoUpdateAfterCallBack="true" runat="server" />
</div>

<script language="javascript" type="text/javascript">
    function deploy_site() {
        window.open('./dopackage.aspx?g=' + document.getElementById('<%=SavedPackageGuid.ClientID %>').value, '', 'top=40,left=200,width=450,height=115,scrollbars=no');
        window.location = './default.aspx';
    }
</script>

</asp:Content>
