<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MarkupEditor.ascx.cs" Inherits="Beachead.Web.CMS.controls.MarkupEditor" %>
<%@ Register src="~/controls/ContentEditor/ResizableTextBox/ResizableTextBox.ascx" tagname="ResizableTextBox" tagprefix="uc" %>
    <link rel="stylesheet" type="text/css" href="<% Response.Write(EditorCssPath); %>" />
    <script type="text/javascript" src="<% Response.Write(EditorScriptPath); %>"></script>  
    <script type="text/javascript">
        dojo.addOnLoad(function () {
            dojo.connect(dojo.byId("preview-frame"), "onload", "endPreview");
        });
    </script>  
    <style type="text/css">
        #waitDialog .dijitDialogTitleBar .dijitDialogCloseIcon {
            display:none;
        }
    </style>  
    <div dojoType="dijit.Dialog" id="waitDialog" closable="false" draggable="false">
        Please wait while your preview is generated.
    </div>    

    <div dojoType="dijit.TitlePane" id="preview-panel" title="Preview Window" open="false">
        <div style="height:350px;overflow:auto;">
            <iframe id="preview-frame" src="" width="90%" height="95%"></iframe>
        </div>
    </div>
    <div dojoType="dijit.TitlePane" title="Markup Editor">
        <asp:Panel ID="ToolbarPanel" runat="server">
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
        <li><anthem:LinkButton ID="PreviewLink" runat="server" OnClick="Preview_Click" OnClientClick="startPreview();" ToolTip="Preview Window" CssClass="formatlink PreviewLink" /></li>                     
        </ul>
        </div>
        </asp:Panel>
        <div style="width:950px;"> 
            <uc:ResizableTextBox ID="PageMarkupText" runat="server" />
        </div>
    </div>
<script language="javascript" type="text/javascript">
    var parent = null;

    function onimage_selected(imageName) {
    <% if (UseStandardImageTags) { %>
        __Insert('../images/' + imageName,'<%=PageMarkupText.TextboxId %>');
    <% } else { %>
        __Insert('[[image:~/' + imageName + ']]{BR}','<%=PageMarkupText.TextboxId %>');
    <% } %>
    }

    function startPreview() {
        var pane = dijit.byId("preview-panel");
        if (!pane.open) 
            pane.toggle();

        dijit.byId('waitDialog').show();
    }

    function endPreview() {
        dijit.byId('waitDialog').hide();
    }

    function DisplayPreview(url) {
        if (url.indexOf("::ALERT::") != -1) {
            alert(url.replace("::ALERT::", ""));
        }
        else {
            var iframe = dojo.byId("preview-frame");
            iframe.src = url;
        }
    }
    
    $('<%=PageMarkupText.TextboxId %>').addEvent('keydown', function(event) {
            if (event.key == 'tab') {
                return __Insert ('    ','<%=PageMarkupText.TextboxId %>');
            }
        });
</script>