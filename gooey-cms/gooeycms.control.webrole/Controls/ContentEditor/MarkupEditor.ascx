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

    <div dojoType="dijit.Dialog" id="fulleditor" style="width:90%;height:90%;" closeable="true" draggable="true" title="Pop-up Markup Editor">
        <div style="padding-bottom:7px;">
        <button onclick="savePopup();return false;">Save &amp; Close</button>&nbsp;
        <input type="checkbox" id="chkwrap" name="chkwrap" onclick="popup_wrap();" /> wrap text 
        </div>
        <textarea id="popupeditor" style="background: none repeat scroll 0% 0% rgb(248, 248, 248); border: 1px solid rgb(2, 2, 2);" wrap="off"></textarea>
    </div>

    <% if (ShowPreviewWindow) { %>
    <div dojoType="dijit.Dialog" id="preview-panel" style="width:90%;height:90%;border:0px;"  title="Preview Window" closeable="true">
            <iframe id="preview-frame" src=""></iframe>
    </div>
    <div dojoType="dijit.TitlePane" title="Markup Editor">
    <% } %>
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
        <li><a href="#" onclick="javascript:window.open('ImageBrowser.aspx?<%=ImageBrowserQuerystring %>','','width=700,height=500,left=400,top=400,titlebar=no,toolbar=no,resizable=no,modal=yes,centerscreen=yes;scroll=no;status=no,menubar=no,location=no'); return false;" title="Image Browser" class="formatlink" id="ImageLink"></a></li>             
        <% if (ShowPreviewWindow) { %><li><anthem:LinkButton ID="PreviewLink" runat="server" OnClientClick="keypressHandler(null); return false;" ToolTip="Preview Window" CssClass="formatlink PreviewLink" /></li><% } %>
        <li><a href="#" onclick="javascript:showEditor(); return false;" title="Popup Editor" class="formatlink" id="PopupEditor"></a></li>        
        </ul>
        </div>
        </asp:Panel>
        <div> 
            <uc:ResizableTextBox ID="PageMarkupText" runat="server" />
        </div>
    <% if (ShowPreviewWindow) { %>
    </div>
    <% } %>

<script language="javascript" type="text/javascript">
    var parent = null;

    function popup_wrap() {
        var chk = dojo.byId('chkwrap');
        var popup = dojo.byId('popupeditor');
        if (chk.checked) {
            if (popup.wrap) {
                popup.wrap = "soft";
            } else {
                popup.setAttribute("wrap","soft");
                var newpopup =  popup.cloneNode(true);
                newpopup.value = popup.value;
                popup.parentNode.replaceChild(newpopup,popup);
            }
        } else {
            if (popup.wrap) {
                popup.wrap = "off";
            } else {
                popup.setAttribute("wrap","off");
                var newpopup =  popup.cloneNode(true);
                newpopup.value = popup.value;
                popup.parentNode.replaceChild(newpopup,popup);
            }

        }
    }

    function showEditor() {
        var inline = dojo.byId('<%=PageMarkupText.TextboxId %>');
        var popup = dojo.byId('popupeditor');

        var container = dijit.byId('fulleditor');

        popup.style.width = (screen.width - 200) + "px";
        popup.style.height = (screen.height - 360) + "px";

        popup.value = inline.value;

        container.show();
    }

    function savePopup() {
        var inline = dojo.byId('<%=PageMarkupText.TextboxId %>');
        var popup = dojo.byId('popupeditor');

        inline.value = popup.value;

        dijit.byId('fulleditor').hide();
        
        var form = dojo.byId("aspnetform");
        if (typeof(onPopupSave) == 'function')
            onPopupSave();
    }

    function onimage_selected(imageName) {
    <% if (UseStandardImageTags) { %>
        __Insert('../images/' + imageName,'<%=PageMarkupText.TextboxId %>');
    <% } else { %>
        __Insert('[[image:~/' + imageName + ']]{BR}','<%=PageMarkupText.TextboxId %>');
    <% } %>
    }

    function keypressHandler(obj) {
        if (window.mytimeout) window.clearTimeout(window.mytimeout);
        startPreview();
        Anthem_InvokeControlMethod('<%=this.ClientID %>','Preview_Click', [],
            function (result) {
                DisplayPreview(result.value);
            }
        );
    }

    function startPreview() {
        var preview = dojo.byId("preview-frame");

        preview.style.width = (screen.width - 200) + "px";
        preview.style.height = (screen.height - 300) + "px";
        var pane = dijit.byId("preview-panel");
        pane.show();

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