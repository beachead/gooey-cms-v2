<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Developer.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
	<script type="text/javascript" src="../../scripts/jquery-1.4.2.min.js"></script>
	<script type="text/javascript" src="../../scripts/jquery.cycle.all.min.js"></script>
	<script type="text/javascript" src="../../scripts/store.js"></script>

    <link rel="stylesheet" type="text/css" href="../../css/store.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li><a href="./site.aspx?g=<%=System.Guid.NewGuid().ToString() %>">PACKAGE A NEW SITE</a></li>
            <li><a href="./theme.aspx?g=<%=System.Guid.NewGuid().ToString() %>">PACKAGE A NEW THEME</a></li> 
        </ul>    
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
This page will allow you to package your site or theme for sale in the Gooey Store.
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
<div style="width:900px;">
   <table>
        <tr>
            <td valign="top"><asp:Image ID="LogoSrc" runat="server" />&nbsp;Upload Logo:
            <asp:FileUpload ID="LogoFile" runat="server" /><asp:Button ID="BtnUploadLogo" OnClick="BtnUploadLogo_Click" Text="Upload" runat="server" /></td>
            <td>
                
            </td>
 <td width="100px;">&nbsp;</td>
 <td>
 <div class="column last maincontent">
<ul class="benefits" id="distribute-perks">
	<li class="even">You pick the price</li>
	<li>You get 70% of sales revenue</li>
	<li class="even">Receive revenue checks quarterly</li>
	<li>No charge for free sites</li>
	<li class="even">No credit card fees</li>
	<li>No hosting fees</li>
	<li class="even">No marketing fees</li>
</ul>
</div>
 </td>       </tr>
    </table>
    <hr />
    <div dojoType="dijit.layout.TabContainer" style="width: 100%; height: 100%;" doLayout="false">
        <div dojoType="dijit.layout.ContentPane" title="My Site Packages" selected="true">
		    <ul id="themes-panel">
                <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" OnItemCommand="SitePackages_OnItemCommand" runat="server">
                    <ItemTemplate>
			            <li class="theme">    
                            <div style="padding-left:30px;padding-top:5px;">
                                <asp:Label ID="LblApprovalStatus" runat="server" />
                            </div>
				            <div class="title" style="margin-top:0px;">
                                <%# Eval("Title") %> - <%# DataBinder.Eval(Container.DataItem,"Price","{0:c}") %>
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <span style="color:#666666;font-size:11px;"><a style="color:#646464;" href='./Edit.aspx?e=<%# Eval("Guid") %>'>Edit</a>&nbsp;&nbsp;&nbsp;&nbsp;<asp:LinkButton ID="BtnDelete" ForeColor="#646464" OnClientClick="return confirm('Are you sure you want to delete this site package? \n\n WARNING: This will immediately remove your site from the GooeyCMS Store.');" CommandName="DeletePackage" CommandArgument='<%# Eval("Guid") %>' runat="server">Delete</asp:LinkButton>&nbsp;&nbsp;</span>
                            </div>
				            <ul class="thumbs" style="padding-left:0px;">
                                <asp:Repeater ID="ThumbnailImages" runat="server">
                                    <ItemTemplate>
                                        <li><img src='<%# Container.DataItem %>' width="344" height="167" alt="" /></li>
                                    </ItemTemplate>
                                </asp:Repeater>
				            </ul>                    
                            <div style="padding-left:20px;padding-top:5px;">
                                <asp:FileUpload ID="FileUpload" runat="server" />&nbsp;<asp:Button ID="BtnAddScreenshot" CommandName="AddScreenshot" CommandArgument='<%# Eval("Guid") %>' Text="Add Screenshot" runat="server" />
                            </div>
				            <ul class="thumb-nav"></ul>
				            <ul class="features" style="margin:0px;">
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
    </div>
</div>
</asp:Content>
