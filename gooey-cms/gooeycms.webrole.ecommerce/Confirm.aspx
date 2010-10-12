<%@ Page Title="" Language="C#" MasterPageFile="~/ContentPage.Master" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="Gooeycms.Webrole.Ecommerce.store.Confirm" %>

<asp:Content ContentPlaceHolderID="localCSS" runat="server" ID="localCSS">
    <link rel="Stylesheet" type="text/css" href="/css/store.css" />
    <style type="text/css">
    table.form td.controls {
        border-top: 1px solid #ccc;
        vertical-align: bottom;
    }    
    </style>
</asp:Content>

<asp:Content id="localJS" ContentPlaceHolderID="localJS" runat="server">
    <script type="text/javascript" src="/js/store.js"></script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">

    <!-- START: content -->
	<div id="content">
        <h2>GooeyCMS Order Review:</h2>

        <!-- START: columns -->
        <div class="columns">
        
            <!-- START: left-column -->
            <div class="left-column">

                <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" runat="server">
                    <ItemTemplate>
                        <ul id="themes-panel" class="collapse-margin">
                            <li class="theme">  
	                            <div class="title"><%# Eval("Title") %> - <%# DataBinder.Eval(Container.DataItem,"Price","{0:c}") %></div>
	                            <div class="logo"><img src="../images/___placeholder_logo.png" width="93" height="27" alt="" /></div>

                                <!-- start: preview -->
                                <div class="preview">
                                    <ul class="thumbs">
                                            <asp:Repeater ID="ThumbnailImages" runat="server">
                                                <ItemTemplate>
                                                    <li><img src='<%# Container.DataItem %>' width="344" height="167" alt="" /></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
				                    </ul>

				                    <ul class="thumb-nav"></ul>
				        
				                    <div class="features">
                                        <a href="#" class="showFeatures">Features</a>
				                        <ul>
                                            <asp:Repeater ID="FeatureList" runat="server">
                                                <ItemTemplate>
                                                    <li><%# Container.DataItem %></li>
                                                </ItemTemplate>
                                            </asp:Repeater>
				                        </ul>
                                    </div>
                                </div>
                                <!-- end: preview -->

                            </li>
                        </ul>


                    </ItemTemplate>
                </asp:Repeater>
            
            </div>
            <!-- END: left-column -->

            <!-- START: right-column -->
            <div class="right-column">

                <table class="form pct-100">
                <tr>
                <td style="font-weight:bold;">Purchaser:</td>
                <td><asp:Label ID="LblPurchaser" runat="server" /></td>
                </tr>

                <tr>
                <td><b>Total Price:</b></td>
                <td><asp:Label ID="LblPrice" runat="server" /></td>
                </tr>

                <tr class="controls">
                <td colspan="2" class="controls">
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
                </table>
            
            </div>
            <!-- END: right-column -->

        </div>
        <!-- END: columns -->

    </div>
    <!-- END: content -->

</asp:Content>
