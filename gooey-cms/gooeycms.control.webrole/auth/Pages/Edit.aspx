<%@ Page Title="" Language="C#" MasterPageFile="~/Secure.Master" AutoEventWireup="true" ValidateRequest="false" CodeBehind="Edit.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.Pages.Edit" %>
<%@ MasterType VirtualPath="~/Secure.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Subnavigation" runat="server">
        <ul>
            <li class=""><a href="./Default.aspx">Page Listing</a></li>        
            <li class="on"><%=PageAction %> Page</li>
            <li><a href="../promotion/PagePromotion.aspx">Promotion</a></li>        
        </ul>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Editor" runat="server">
    <b><%=PageAction %> Page:</b><br /><br />
    <asp:Label ID="Status" runat="server" />
    <table style="width:100%;" cellpadding="0" cellspacing="0">
        <tr>
            <td class="page-group" style="width:100%;">
                <div class="page-group">
                    <div style="float:right;">
                        <a href="#" tabindex="100" onclick="toggleVisibility('path-options');"><span id="path-options-text">Hide</span></a>
                    </div>                
                    <div style="display:none;" id="path-options-name">---Path Options---<hr /></div>
                    <div id="path-options">
                        <b>Path Options:</b><br /><br />
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
                </div>
            </td>
        </tr>
        <tr>
            <td colspan="2" class="page-group">
                <div class="page-group">
                    <div style="float:right;">
                        <a href="#" tabindex="100"  onclick="toggleVisibility('metatag-options');"><span id="metatag-options-text">Hide</span></a>
                    </div>                
                    <div style="display:none;" id="metatag-options-name">---Meta Tag Options---<hr /></div>
                    <div id="metatag-options">
                        <b>Metatag Options:</b><br /><br />
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
                                    <a href="#" onclick="window.open('Css.aspx?pid=<%= Request.QueryString["pid"] %>','csswindow','width=800,height=600,scrollbars=yes'); return false;">Manage CSS</a>&nbsp;
                                    <a href="#" onclick="window.open('Javascript.aspx?pid=<%= Request.QueryString["pid"] %>','jswindow','width=800,height=600,scrollbars=yes'); return false;">Manage Javascript</a>
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
                </div>

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
            </td>
        </tr>
    </table>
</asp:Content>
