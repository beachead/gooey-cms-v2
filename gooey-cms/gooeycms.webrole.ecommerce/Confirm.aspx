<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.store.Confirm" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
	<div id="content">
        <div style="padding-left:25px;">
            <span style="font-size:18px;font-weight:bold;">GooeyCMS Order Review:</span>        
        </div>
        <div style="height:330px;padding-top:10px;">
        <ul id="themes-panel" style="padding:0px;margin:0px;">
            <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" runat="server">
                <ItemTemplate>
                    <li class="theme">  
	                    <div class="title"><%# Eval("Title") %> - <%# DataBinder.Eval(Container.DataItem,"Price","{0:c}") %></div>
	                    <div class="logo"><img src="../images/___placeholder_logo.png" width="93" height="27" alt="" /></div>
	                    <ul class="thumbs">
                                <asp:Repeater ID="ThumbnailImages" runat="server">
                                    <ItemTemplate>
                                        <li><img src='<%# Container.DataItem %>' width="344" height="167" alt="" /></li>
                                    </ItemTemplate>
                                </asp:Repeater>
	                    </ul>
	                    <ul class="thumb-nav"></ul>
	                    <ul class="features">
                                <asp:Repeater ID="FeatureList" runat="server">
                                    <ItemTemplate>
                                        <li><%# Container.DataItem %></li>
                                    </ItemTemplate>
                                </asp:Repeater>
	                    </ul>
                    </li>                                      
                </ItemTemplate>
            </asp:Repeater>
        </ul>
        </div>
        <div style="padding-left:23px;">
            <table>
                <tr>
                    <td style="font-weight:bold;">Purchaser:</td>
                    <td><asp:Label ID="LblPurchaser" runat="server" /></td>
                    <td rowspan="2">
                        <input type="hidden" name="cmd" value="_xclick">
                        <input type="hidden" name="business" value="seller_1272749312_biz@gmail.com" />
                        <input type="hidden" name="amount" value="<%= Amount %>" />
                        <input type="hidden" name="item_name" value="<%= PackageTitle %>" />
                        <input type="hidden" name="item_number" value="<%= PackageGuid %>" />
                        <input type="hidden" name="custom" value="<%= ReceiptGuid %>" />
                        <input type="hidden" name="return" value="<%= ReturnUrl %>" />
                        <asp:ImageButton ID="BtnPaypalPurchase" ImageUrl="https://www.sandbox.paypal.com/en_US/i/btn/btn_paynowCC_LG.gif" PostBackUrl="https://www.sandbox.paypal.com/cgi-bin/webscr" runat="server" />                                        
                    </td>
                </tr>
                <tr>
                    <td><b>Total Price:</b></td>
                    <td><asp:Label ID="LblPrice" runat="server" /></td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>
