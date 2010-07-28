<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarkupEditor.ascx.cs" Inherits="Beachead.Web.CMS.controls.MarkupEditor" %>
<%@ Register src="~/controls/ContentEditor/ResizableTextBox/ResizableTextBox.ascx" tagname="ResizableTextBox" tagprefix="uc" %>
<link rel="stylesheet" type="text/css" href="<% Response.Write(EditorCssPath); %>" />
<script type="text/javascript" src="<% Response.Write(EditorScriptPath); %>"></script>      
    <asp:FileUpload ID="HiddenFileUpload" CssClass="hidden" runat="server" />
    <div>
        <div style="width:900px;height:25px;">    
        <ul id="FormatUl">
        <li><a href="#" onclick="javascript:return __Wrap('<b>', '</b>','<%=PageMarkupText.TextboxId %>');" title="Bold" class="formatlink" id="BoldLink"></a></li>
        <li><a href="#" onclick="javascript:return __Wrap('<i>', '</i>','<%=PageMarkupText.TextboxId %>');" title="Italics" class="formatlink" id="ItalicLink"></a></li>
        <li><a href="#" onclick="javascript:return __Wrap('__ ', ' __','<%=PageMarkupText.TextboxId %>');" title="Underline" class="formatlink" id="UnderlineLink"></a></li>
        <li><a href="#" onclick="javascript:return __Wrap('== ', ' ==','<%=PageMarkupText.TextboxId %>');" title="H1" class="formatlink" id="H1Link"></a></li>
        <li><a href="#" onclick="javascript:return __Wrap('=== ', ' ===','<%=PageMarkupText.TextboxId %>');" title="H2" class="formatlink" id="H2Link"></a></li>
        <li><a href="#" onclick="javascript:return __Wrap('---StartList---\r\n* Item 1\r\n* Item 2 ', '\r\n---EndList---','<%=PageMarkupText.TextboxId %>');" title="Unordered List" class="formatlink" id="UnorderedList"></a></li>
        <li><a href="#" onclick="javascript:return __Insert('{BR}','<%=PageMarkupText.TextboxId %>');" title="Linebreak" class="formatlink" id="BrLink"></a></li>
        <li><a href="#" onclick="javascript:return __Wrap('<nomarkup>', '</nomarkup>','<%=PageMarkupText.TextboxId %>');" title="No Markup" class="formatlink" id="NoMarkup"></a></li>
        <li><a href="#" onclick="javascript:return __Wrap('<esc>', '</esc>','<%=PageMarkupText.TextboxId %>');" title="Escape HTML" class="formatlink" id="EscapeLink"></a></li>             
        <li><a href="#" onclick="javascript:window.open('ImageBrowser.aspx','','width=500,height=400,left=400,top=400,titlebar=no,toolbar=no,resizable=no,modal=yes,centerscreen=yes;scroll=no;status=no,menubar=no,location=no'); return false;" title="Image Browser" class="formatlink" id="ImageLink"></a></li>             
        <li><anthem:LinkButton ID="PreviewLink" runat="server" OnClick="Preview_Click" ToolTip="Preview Window" OnClientClick="OpenPreviewWindow();" CssClass="formatlink PreviewLink" /></li>                     
        </ul>
        </div>
        <div style="width:950px;"> 
            <uc:ResizableTextBox ID="PageMarkupText" runat="server" />
        </div>
    </div>
<script language="javascript" type="text/javascript">
    var parent = null;

    function onimage_selected(imageName) {
        __Insert('[image:~/' + imageName + ']{BR}','<%=PageMarkupText.TextboxId %>');
    }

    function OpenPreviewWindow() {
        parent = window.open('', 'preview', 'width=800,height=600,scrollbars=yes');
    }

    function DisplayPreview(url) {
        if (url.indexOf("::ALERT::") != -1) {
            alert(url.replace("::ALERT::", ""));
            parent.close();
        }
        else {
            parent.location = url;
        }
    }
    
    $('<%=PageMarkupText.TextboxId %>').addEvent('keydown', function(event) {
            if (event.key == 'tab') {
                return __Insert ('    ','<%=PageMarkupText.TextboxId %>');
            }
        });
</script>