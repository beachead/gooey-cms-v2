<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Developer.Default" %>
<asp:Content ID="localJS" ContentPlaceHolderID="localJS" runat="server">
	<script type="text/javascript" src="../../scripts/jquery-1.4.2.min.js"></script>
	<script type="text/javascript" src="../../scripts/jquery.cycle.all.min.js"></script>
	<script type="text/javascript" src="../../scripts/store.js"></script>
</asp:Content>
<asp:Content id="localCSS" ContentPlaceHolderID="localCSS" runat="server">
    <link rel="stylesheet" type="text/css" href="../../css/store.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
    <ul>
        <li class="on">DEVELOPERS HOME</a></li>
        <li><a href="./site.aspx?g=<%=System.Guid.NewGuid().ToString() %>">PACKAGE A NEW SITE</a></li>
        <li><a href="./theme.aspx?g=<%=System.Guid.NewGuid().ToString() %>">PACKAGE A NEW THEME</a></li> 
        <li class="last"><a href="Settings.aspx">DEVELOPER SETTINGS</a></li> 
    </ul>    
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
    <div style="overflow: auto;">
        <div style="float: right; width: 333px;">
            <div style="float: left; margin-right: 30px;">
                <asp:Image ID="LogoSrc" runat="server" />
            </div>
            <div style="float: left;">
                <ul class="check">
	                <li class="alt">You pick the price</li>
	                <li>You get 70% of sales revenue</li>
	                <li class="alt">Receive revenue checks quarterly</li>
	                <li>No charge for free sites</li>
	                <li class="alt">No credit card fees</li>
	                <li>No hosting fees</li>
	                <li class="alt">No marketing fees</li>
                </ul>
            </div>
        </div>

        <h1><img src="/images/build_your_biz.png" width="495px" height="37px" border="0" /></h1>
        <p>View the sites you have purchased, apply them to your subscriptions or your clients and create new sites or themes to sell.</p>
    </div>
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">


		    <ul id="themes-panel">
                <asp:Repeater ID="SitePackages" OnItemDataBound="SitePackages_OnItemDataBound" OnItemCommand="SitePackages_OnItemCommand" runat="server">
                    <ItemTemplate>
			            <li class="theme">    

                            <!-- start: theme-header -->
                            <div class="theme-header">
				                <div class="title"><%# Eval("Title") %> - <%# DataBinder.Eval(Container.DataItem,"Price","{0:c0}") %></div>
				                <div class="logo"><asp:Label ID="LblApprovalStatus" runat="server" /></div>
				                <ul class="options-links">
                                    <li><a href='./Edit.aspx?e=<%# Eval("Guid") %>'>Edit</a></li>
                                    <li><asp:LinkButton ID="BtnDelete" OnClientClick="return confirm('Are you sure you want to delete this site package? \n\n WARNING: This will immediately remove your site from the GooeyCMS Store.');" CommandName="DeletePackage" CommandArgument='<%# Eval("Guid") %>' runat="server">Delete</asp:LinkButton></li>
                                </ul>
                            </div>
                            <!-- end: theme-header -->

                            <!-- start: preview -->
                            <div class="preview">
                                <ul class="thumbs">
                                    <asp:Repeater ID="ThumbnailImages" runat="server">
                                        <ItemTemplate>
                                            <li><img src='<%# Container.DataItem %>' width="344" height="167" alt="" /></li>
                                        </ItemTemplate>
                                    </asp:Repeater>
				                </ul>

				                <div class="theme-add-screenshot">
                                    <asp:FileUpload ID="FileUpload" runat="server" />&nbsp;<asp:Button ID="BtnAddScreenshot" CommandName="AddScreenshot" CommandArgument='<%# Eval("Guid") %>' Text="Add Screenshot" runat="server" />
                                </div>
				        
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
                    </ItemTemplate>
                </asp:Repeater>
            </ul>                        


</asp:Content>
