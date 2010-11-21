<%@ Page Title="" Language="C#" MasterPageFile="~/SecureNoNavigation.Master" AutoEventWireup="true" CodeBehind="Edit.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Developer.Edit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">

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
<asp:Button ID="BtnSave" Text="Save" OnClick="BtnSave_Click" runat="server" />
</div>

</asp:Content>
