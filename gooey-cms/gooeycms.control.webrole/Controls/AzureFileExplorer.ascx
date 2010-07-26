<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AzureFileExplorer.ascx.cs" Inherits="Gooeycms.Webrole.Control.Controls.AzureFileExplorer" %>
<table>
    <tr>
        <td>Upload</td>
        <td>From URL</td>
        <td>Library</td>
    </tr>
    <tr>
        <td colspan="3"><hr /></td>
    </tr>
    <anthem:Panel ID="PanelUpload" runat="server">
    <tr>
        <td colspan="3">Upload image(s) from your computer. (Zip files will automatically be extracted)</td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:FileUpload ID="FileUpload" runat="server" />
        </td>
    </tr>
    </anthem:Panel>

    <anthem:Panel ID="PanelFromUrl" runat="server">
    <tr>
    <tr>
        <td colspan="3">URL of an image to add to library</td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:TextBox ID="TxtImageUrl" runat="server" />
        </td>
    </tr>          
    </tr>
    </anthem:Panel>

    <anthem:Panel ID="PanelLibrary" runat="server">
    <tr>
          <td colspan="3">
            <asp:Table ID="TblImageLibrary" runat="server" />
            <asp:HiddenField ID="SelectedImageName" runat="server" />
          </td>
    </tr>
    </anthem:Panel>

    <tr>
        <td><asp:Button ID="BtnSubmit" Text="Submit" runat="server" /></td>
        <td><asp:Button ID="BtnCancel" Text="Cancel" runat="server" /></td>
    </tr>
</table>