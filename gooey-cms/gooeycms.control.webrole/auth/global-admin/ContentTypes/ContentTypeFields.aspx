<%@ Page Title="" Language="C#" MasterPageFile="~/SecureGlobalAdmin.Master" AutoEventWireup="true" CodeBehind="ContentTypeFields.aspx.cs" Inherits="Gooeycms.Webrole.Control.auth.global_admin.ContentTypes.ContentTypeFields" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="localCSS" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="localJS" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="Subnavigation" runat="server">
    <beachead:Subnav ID="Subnav" runat="server" NavSection="global_admin_contenttypes" NavItem="default" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="Instructions" runat="server">
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="Editor" runat="server">
    <h1>Manage Content Fields for '<asp:Label ID="LblContentTypeName" ForeColor="#4395F1" runat="server" />'</h1>

<div style="padding-bottom:5px;">
<asp:Button ID="BtnAddNew" Text="Add New Field" CausesValidation="false" OnClick="BtnAddNew_Click" runat="server" />
</div>

<div dojoType="dijit.TitlePane" title="Manage Existing Fields">
Default Display Field: <asp:DropDownList ID="LstDefaultDisplayField" runat="server" />&nbsp;&nbsp;<asp:LinkButton ID="BtnUpdateDefaultDisplayField" OnClick="BtnUpdateDefaultDisplayField_Click" Text="Update" CausesValidation="false" runat="server" />
<asp:GridView ID="FieldTable" runat="server" AutoGenerateColumns="False" 
        CssClass="data" ForeColor="#333333" 
        GridLines="None" OnRowCommand="OnRowCommand" 
        DataSourceID="ContentFieldDataSource">
    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
    <Columns>
        <asp:BoundField DataField="Name" HeaderText="Label" SortExpression="Name">
            <HeaderStyle HorizontalAlign="Left" />
        </asp:BoundField>
        <asp:BoundField DataField="SystemName" HeaderText="System Name" SortExpression="Name">
            <HeaderStyle HorizontalAlign="Left" />
        </asp:BoundField>
        <asp:BoundField DataField="Description" HeaderText="Description" 
            SortExpression="Description">
            <HeaderStyle HorizontalAlign="Left" />
        </asp:BoundField>
        <asp:BoundField DataField="FieldType" HeaderText="Field Type" 
            SortExpression="FieldType">
            <HeaderStyle HorizontalAlign="Left" />
        </asp:BoundField>
        <asp:TemplateField HeaderText="Operations">
            <HeaderStyle HorizontalAlign="Left" />
            <ItemTemplate>
                <asp:HiddenField ID="ExistingFieldId" Value='<%# Eval("Id") %>' Visible='<%# Eval("IsUserField") %>' runat="server" />
                <asp:LinkButton ID="EditItem" CommandName="editid" Text="edit" Visible='<%# Eval("IsUserField") %>' CausesValidation="false" runat="server" />
                <asp:LinkButton ID="DeleteItem" CommandName="deleteid" Text="delete" Visible='<%# Eval("IsUserField") %>' OnClientClick="return confirm('Are you sure you want to delete this field? This will not impact existing CMS content');" CausesValidation="false" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <EditRowStyle BackColor="#999999" />
    <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
</asp:GridView>
<asp:ObjectDataSource ID="ContentFieldDataSource" runat="server" 
        SelectMethod="GetContentTypeFields" 
        TypeName="Gooeycms.Business.Content.ContentManagerDataAdapter">
    <SelectParameters>
        <asp:QueryStringParameter Name="guid" 
            QueryStringField="tid" Type="Object" />
    </SelectParameters>
</asp:ObjectDataSource>
</div>
<br />
<div dojoType="dijit.TitlePane" title="Add/Edit Field">
    <table class="form">
        <tr>
            <td class="label">Field Type</td>
            <td>
                <asp:DropDownList ID="FieldType" AutoPostBack="true" OnSelectedIndexChanged="FieldType_Change" runat="server">
                    <asp:ListItem Text="...select..." Value="" /> 
                    <asp:ListItem Text="Textbox" Value="Textbox" />
                    <asp:ListItem Text="DateTime" Value="DateTime" />
                    <asp:ListItem Text="Textarea" Value="Textarea" />
                    <asp:ListItem Text="Dropdown" Value="Dropdown" />
                </asp:DropDownList>      
                <asp:RequiredFieldValidator ID="FieldTypeValidator" ControlToValidate="FieldType" Text="* You must select a data type for this field" runat="server" />  
            </td>
        </tr>
        <tr>
            <td class="label">System Name (no spaces)</td>
            <td><asp:TextBox ID="TxtSystemName" dojoType="dijit.form.ValidationTextBox" promptMessage="The system name is used when referencing this field within the CMS system. May only contain letters and no spaces" regExp="[a-zA-Z]+" invalidMessage="The system name may only contain letters without numbers or spaces" required="true"  runat="server" /></td>
            <asp:RegularExpressionValidator ID="SystemNameValidator" ControlToValidate="TxtSystemName" ValidationExpression="[a-zA-Z]+" Display="None" runat="server" />
        </tr>  
        <tr>
            <td class="label">Label</td>
            <td><asp:TextBox ID="TxtName" dojoType="dijit.form.ValidationTextBox" required="true" promptMessage="Input the human-readable label for this field" regExp="[a-zA-Z_0-9\s]+" invalidMessage="The label may only contain letters, numbers and/or spaces." runat="server" /></td>
            <asp:RegularExpressionValidator ID="TxtNameValidator" ControlToValidate="TxtName" ValidationExpression="[a-zA-Z_0-9\s]+" Display="None" runat="server" />
        </tr>    
        <tr>
            <td class="label">Description</td>
            <td><asp:TextBox ID="Description" dojoType="dijit.form.ValidationTextBox" required="false" Width="250px" runat="server" /></td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td>
            <asp:CheckBox ID="ChkRequiredField" Text="Required Field" runat="server" />
            <div dojoType="dijit.Tooltip" connectId="<%= ChkRequiredField.ClientID %>" position="after">
                This is your <b>arbitrary</b> HTML here
            </div>
            </td>
        </tr>
        <asp:Panel ID="TextAreaFields" Visible="false" runat="server">
        <tr>
            <td class="label">Rows</td>
            <td><asp:TextBox ID="Rows" MaxLength="3" Width="50px" Text="10" runat="server" /></td>
        </tr>
        <tr>
            <td class="label">Columns</td>
            <td><asp:TextBox ID="Cols" MaxLength="3" Width="50px" Text="150" runat="server" /></td>
        </tr>    
        </asp:Panel>
    
        <asp:Panel ID="DropdownFields" Visible="false" runat="server">
        <tr>
            <td class="label">
                Values<br />
                (one per line)
            </td>
            <td>
                <asp:TextBox ID="Options" TextMode="MultiLine" Rows="5" Columns="40" runat="server" />
            </td>
        </tr>
        </asp:Panel>
        <tr class="controls">
            <td>&nbsp;</td>
            <td style="text-align:left;padding-left:170px;">
                <asp:HiddenField ID="ExistingId" runat="server" />
                <asp:Button ID="Add" Text="Add Field" OnClick="Add_Click" runat="server" />
            </td>
        </tr>
    </table>    
</div>
<br />
</asp:Content>
