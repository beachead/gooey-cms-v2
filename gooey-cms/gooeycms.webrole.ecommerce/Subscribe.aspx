<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Subscribe.aspx.cs" Inherits="gooeycms.webrole.ecommerce.Subscribe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceholder" runat="server">
<div>
REVIEW THE DATA FROM PREVIOUS PAGE
</div>

<div>
    <input type="hidden" name="cmd" value="_xclick-subscriptions">
    <input type="hidden" name="business" value="seller_1272749312_biz@gmail.com" />
    <input type="hidden" name="item_name" value="Business Plan + Salesforce Integration" />
    <input type="hidden" name="a1" value="0" />
    <input type="hidden" name="p1" value="1" />
    <input type="hidden" name="t1" value="M" />
    <input type="hidden" name="a3" value="300" />
    <input type="hidden" name="p3" value="1" />
    <input type="hidden" name="t3" value="M" />
    <input type="hidden" name="src" value="1" />
    <input type="hidden" name="sra" value="1" />
    <input type="hidden" name="custom" value="<%= Guid %>" />
    <input type="hidden" name="return" value="<%=ReturnUrl %>" />
    <asp:ImageButton ImageUrl="https://www.sandbox.paypal.com/en_US/i/btn/btn_subscribeCC_LG.gif" PostBackUrl="https://www.sandbox.paypal.com/cgi-bin/webscr" runat="server" />
</div>
</asp:Content>
