<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Edit.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Pages.Edit" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li class=""><a href="./Default.aspx">MANAGE PAGES</a></li>        
            <li class="on"><%=PageAction %> PAGE</li>
            <li><a href="../promotion/PagePromotion.aspx">PROMOTION</a></li>        
        </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <style type="text/css">
        #blanket {
        background-color:#111;
        opacity: 0.65;
        filter:alpha(opacity=65);
        position:absolute;
        z-index: 9001;
        top:0px;
        left:0px;
        width:100%;
        }
        #popUpDiv {
        position:absolute;
        background-color:#eeeeee;
        width:300px;
        height:300px;
        z-index: 9002;    
    </style>
    <b><%=PageAction %> Page:</b><br /><br />
    <asp:Label ID="Status" runat="server" />

    <div dojoType="dijit.TitlePane" title="Path Options">
        <table>
            <tr>
                <td>Parent Directory</td>
                <td colspan="3">Page Name</td>
            </tr>
            <tr>
                <td style="vertical-align:top;"><asp:DropDownList ID="ParentDirectories" runat="server" /></td>
                <td colspan="3">
                    <asp:TextBox ID="PageName" TabIndex="1" Width="300px" runat="server" /><br />
                    <asp:RegularExpressionValidator ID="PageNameValidator" ValidationExpression=".*" Text="Page name does not conform to naming standard" ControlToValidate="PageName" runat="server" />
                </td>
            </tr>             
        </table>
    </div>

    <div dojoType="dijit.TitlePane" title="Metatag Options">
        <table>
            <tr>
                <td style="vertical-align:top;">Page Title:</td>
                <td>
                    <asp:TextBox ID="PageTitle" TabIndex="2" Width="300px" runat="server" />
                    <asp:RequiredFieldValidator ID="PageTitleRequired" Text="You must enter a title for this page" ControlToValidate="PageTitle" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top;">Page Description:</td>
                <td>
                    <asp:TextBox ID="PageDescription" Width="300px" TabIndex="3" TextMode="MultiLine" Rows="5" runat="server" /><br />
                    <asp:RequiredFieldValidator ID="PageDescriptionRequired" Enabled="false" Text="You must provide a description for this page" ControlToValidate="PageDescription" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top;">Page Keywords:</td>
                <td>
                    <asp:TextBox ID="PageKeywords" Width="300px" TabIndex="4" runat="server" /><br />
                    <asp:RequiredFieldValidator ID="PageKeywordsRequired" Enabled="false" Text="You must enter keywords for this page" ControlToValidate="PageKeywords" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="vertical-align:top;">Body Load Options:</td>
                <td>
                    <asp:TextBox ID="BodyLoadOptions" Width="300px" TabIndex="4" runat="server" /><br />
                </td>
            </tr>                            
            <asp:Panel ID="CssManagePanel" Visible="false" runat="server">
            <tr>
                <td colspan="2">
                    <a href="#" onclick="window.open('Stylesheet.aspx?pid=<%= Request.QueryString["pid"] %>','csswindow','width=900,height=590,scrollbars=yes'); return false;">Manage CSS</a>
                    <a href="#" onclick="window.open('Javascript.aspx?pid=<%= Request.QueryString["pid"] %>','jswindow','width=900,height=590,scrollbars=yes'); return false;">Manage Javascript</a>
                </td>
            </tr>
            </asp:Panel>
            <asp:Panel ID="CssNotAvailablePanel" Visible="true" runat="server">
            <tr>
                <td colspan="2"><i>Manage CSS&nbsp;Manage Javascript<br />(Page must be saved before this option is available)</i></td>
            </tr>
            </asp:Panel>
        </table>
    </div>

    <div dojoType="dijit.TitlePane" title="Page Editor">
        <div class="page-group">
            <b>Template:</b><br />
            <asp:DropDownList ID="PageTemplate" runat="server" />
        </div>
        <div class="page-group">
        <b>Body:</b><br />
        <beachead:Editor ID="PageMarkupText" TabIndex="5" runat="server" />
        </div>
        <div>
            <asp:Button ID="BtnSave" OnClick="OnSave_Click" Text="Save" runat="server" />
            &nbsp;&nbsp;
            <asp:Button ID="BtnDelete" OnClick="OnDelete_Click" OnClientClick="return confirm('Are you sure you want to delete this page?')" Text="Delete" runat="server" />
        </div>
    </div>

    <script language="javascript" type="text/javascript">
        function onPopupSave() {
            var btnSave = dojo.byId('<% Response.Write(BtnSave.ClientID); %>');
            btnSave.click();
        }
    </script>
</asp:Content>
