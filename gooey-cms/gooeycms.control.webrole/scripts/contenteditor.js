/*
*  This javascript editor is based upon the ScrewTurnWiki Editor.
*  http://www.screwturn.eu
*/
function getMozSelection() {
    return document.getSelection();
}

// IE only - stores the current cursor position on any textarea activity
function storeCaret(txtarea) {
    if (txtarea.createTextRange) {
        txtarea.caretPos = document.selection.createRange().duplicate();
    }
}

// IE only - wraps selected text with lft and rgt
function WrapIE(lft, rgt) {
    strSelection = document.selection.createRange().text;
    if (strSelection != "") {
        document.selection.createRange().text = lft + strSelection + rgt;
    }
}

// Moz only - wraps selected text with lft and rgt
function wrapMoz(txtarea, lft, rgt) {
    var selLength = txtarea.textLength;
    var selStart = txtarea.selectionStart;
    var selEnd = txtarea.selectionEnd;
    if (selEnd == 1 || selEnd == 2) selEnd = selLength;
    var s1 = (txtarea.value).substring(0, selStart);
    var s2 = (txtarea.value).substring(selStart, selEnd)
    var s3 = (txtarea.value).substring(selEnd, selLength);
    txtarea.value = s1 + lft + s2 + rgt + s3;
    txtarea.setSelectionRange(selStart + lft.length, selStart + lft.length);
}

// Chooses technique based on browser
function wrapTag(txtarea, lft, rgt) {
    lft = unescape(lft);
    rgt = unescape(rgt);
    if (document.all) {
        WrapIE(lft, rgt);
    }
    else if (document.getElementById) {
        wrapMoz(txtarea, lft, rgt);
    }
}

// IE only - Insert text at caret position or at start of selected text
function insertIE(txtarea, text) {
    if (txtarea.createTextRange && txtarea.caretPos) {
        var caretPos = txtarea.caretPos;
        caretPos.text = caretPos.text.charAt(caretPos.text.length - 1) == ' ' ? text + caretPos.text + ' ' : text + caretPos.text;
    }
    else {
        var strStartTag = text;
        var strEndTag = "";
        var objTextArea = txtarea

        objTextArea.focus();
        var objSelectedTextRange = document.selection.createRange();
        var strSelectedText = objSelectedTextRange.text;
        if (strSelectedText.substring(0, strStartTag.length) == strStartTag && strSelectedText.substring(strSelectedText.length - strEndTag.length, strSelectedText.length) == strEndTag) {
            objSelectedTextRange.text = strSelectedText.substring(strStartTag.length, strSelectedText.length - strEndTag.length);
        }
        else {
            objSelectedTextRange.text = strStartTag + strSelectedText + strEndTag;
        }
        objSelectedTextRange.select();
    }
    return false;
}

// Moz only - Insert text at caret position or at start of selected text
function insertMoz(txtarea, lft) {
    var rgt = "";
    wrapTag(txtarea, lft, rgt);
    return false;
}

// Switch function based on browser - Insert text at caret position or at start of selected text
function insertTag(txtarea, lft) {
    if (document.all) {
        insertIE(txtarea, lft);
    }
    else if (document.getElementById) {
        insertMoz(txtarea, lft);
    }
}

function __Insert(strInsertText, editor) {
    var objTextArea = document.getElementById(editor);
    insertTag(objTextArea, strInsertText);
    return false;
}

function __Wrap(strStartTag, strEndTag, editor) {
    while (strStartTag.indexOf("$") != -1) {
        // The symbol '$' is used to replace double quotes inside javascript links
        strStartTag = strStartTag.replace("$", "\"");
    }
    var objTextArea = document.getElementById(editor);
    if (objTextArea) {
        if (document.selection && document.selection.createRange) {
            objTextArea.focus();
            var objSelectedTextRange = document.selection.createRange();
            var strSelectedText = objSelectedTextRange.text;
            if (strSelectedText.substring(0, strStartTag.length) == strStartTag && strSelectedText.substring(strSelectedText.length - strEndTag.length, strSelectedText.length) == strEndTag) {
                objSelectedTextRange.text = strSelectedText.substring(strStartTag.length, strSelectedText.length - strEndTag.length);
            }
            else {
                objSelectedTextRange.text = strStartTag + strSelectedText + strEndTag;
            }
            objSelectedTextRange.select();
        }
        else {
            objTextArea.focus();
            var selStart = objTextArea.selectionStart;
            var strFirst = objTextArea.value.substring(0, objTextArea.selectionStart);
            var strSelected = objTextArea.value.substring(objTextArea.selectionStart, objTextArea.selectionEnd);
            var strSecond = objTextArea.value.substring(objTextArea.selectionEnd);
            if (strSelected.substring(0, strStartTag.length) == strStartTag && strSelected.substring(strSelected.length - strEndTag.length, strSelected.length) == strEndTag) {
                // Remove tags
                strSelected = strSelected.substring(strStartTag.length, strSelected.length - strEndTag.length);
                objTextArea.value = strFirst + strSelected + strSecond;
                objTextArea.selectionStart = selStart;
                objTextArea.selectionEnd = selStart + strSelected.length;
            }
            else {
                objTextArea.value = strFirst + strStartTag + strSelected + strEndTag + strSecond;
                objTextArea.selectionStart = selStart;
                objTextArea.selectionEnd = selStart + strStartTag.length + strSelected.length + strEndTag.length;
            }
        }
    }
    return false;
}