function cbOnChange(hiddenID, cbID) {
    var isChecked = jQuery('#' + cbID).prop('checked');
    if (jQuery('#scWebEditRibbon').contents().find('#Check_BE500FA7DEF2746AF885224022DACA876').prop('checked')) {
        jQuery('#' + cbID).prop('checked', !jQuery('#' + cbID).prop('checked'))
    }
    var rtcSpan = jQuery(jQuery('#' + hiddenID + ' span.scWebEditInput')[0]);
    var fieldID = "#" + rtcSpan.attr('id').replace("_edit", "");
    var isCheckedVal = isChecked ? "0" : "1";
    jQuery(fieldID).val(isCheckedVal);
    Sitecore.PageModes.PageEditor.setModified(true);
}

function selOnChange(hiddenID, selID) {
    var rtcSpan = jQuery(jQuery('#' + hiddenID + ' span.scWebEditInput')[0]);
    var fieldID = "#" + rtcSpan.attr('id').replace("_edit", "");
    jQuery(fieldID).val(jQuery('#' + selID).val());
    Sitecore.PageModes.PageEditor.setModified(true);
}

function updateHiddenField(hiddenID, newVal) {
    var rtcSpan = jQuery('#' + hiddenID + ' span.scWebEditInput').get(0);
    var fieldID = "#" + rtcSpan.id.replace("_edit", "");
    jQuery(fieldID).val(newVal);
    Sitecore.PageModes.PageEditor.setModified(true);
}

function dlOnChange(fieldName, urlAction) {
    jQuery("#" + fieldName).on('change', function () {
        var changedValue = jQuery("#" + fieldName).val();

        jQuery.ajax({
            url: urlAction,
            type: "POST",
            data: { "changedValue": changedValue, "fieldName": fieldName },
            context: this,
            success: function (data) {
                Sitecore.PageModes.PageEditor.postRequest('webedit:save()');
                console.log("success", data);
            },
            error: function (data) {
                alert('error: ' + data);
                console.log("error", data);
            }
        });
    });
}

function addNewNavItems(parentPath, templatePath, urlAction) {
    showLoading();
    var postReq = jQuery.ajax({
        url: urlAction,
        type: "POST",
        data: { "parentPath": parentPath, "templatePath": templatePath },
        context: this,
        success: function (data) {

            var getNewItemId = jQuery(jQuery.parseHTML(data)).filter(".newItemId");
            console.log(getNewItemId);
            Sitecore.PageModes.PageEditor.postRequest("webedit:fieldeditor(command={11111111-1111-1111-1111-111111111111}, fields=Nav Bar Class|Nav Bar Label|Label h1 Class|Label i Class|ul Class|Title|Template Type|URL|openInNewTab|li Class|a Class|Inner Div Class|Outer Div Class|Menu Class|Menu Group Class|li Id, id=" + getNewItemId[0]['innerHTML'] + ")");
            //jQuery(jQuery.parseHTML(data)).filter(".newItemId").html("");
            jQuery(jQuery.parseHTML(data)).filter(".newItemId").remove();
            console.log(getNewItemId);
            //Sitecore.PageModes.PageEditor.postRequest('webedit:save()');
            //location.reload(true);
            //console.log("success", success);
        },
        error: function (data) {
            alert('error: ' + data);
            //console.log("error", data);
        }
    }).done(function () {

        hideLoading();
    });
}

function addNewChildItem(parentPath, templatePath, typeName, fieldNames, urlAction) {
    showLoading();
    var postReq = jQuery.ajax({
        url: urlAction,
        type: "POST",
        data: { 'parentPath_ML': parentPath, 'templatePath_ML': templatePath, 'typeName': typeName },
        context: this,
        success: function (data2) {
            var getNewItemId = jQuery(jQuery(data2)).find(".newItemId");
            console.log(getNewItemId);
            Sitecore.PageModes.PageEditor.postRequest("webedit:fieldeditor(command={11111111-1111-1111-1111-111111111111}, fields=" + fieldNames + ", id=" + getNewItemId[0]['innerHTML'] + ")");
            //jQuery(jQuery.parseHTML(data)).filter(".newItemId").remove();


            console.log("Done!");
        },
        error: function (data2) {
            alert('error: ' + data2);
            //console.log("error", data);
        }
    }).done(function () {
        hideLoading();
        console.log("Done2!");
        autoSave();
    });
}

function updateItemName(itemID, itemName) {
    showLoading();
    console.log("Posting...");
    var postReq = jQuery.post("/SetDataSource.aspx", {
        method: 'UpdateItemName'
        , itemID: itemID
        , itemName: itemName
    });
    postReq.done(function (data) {
        hideLoading();
        console.log("Done!");
        console.log(data);
    });
}

function setDataSrc(itemID, templatePath, suffix, renderingID, dbName, deviceID, renderingUID, btnID, reload, callBack) {
    itemID = itemID.trim();
    templatePath = templatePath.trim();
    suffix = suffix.trim();
    renderingID = renderingID.trim();
    dbName = dbName.trim();
    deviceID = deviceID.trim();
    renderingUID = renderingUID.trim();
    btnID = btnID.trim();
    showLoading();
    console.log("Posting...");
    var postReq = jQuery.post("/SetDataSource.aspx", {
        method: 'SetDataSource'
        , itemID: itemID
        , templatePath: templatePath
        , suffix: suffix
        , renderingID: renderingID
        , dbName: dbName
        , deviceID: deviceID
        , renderingUID: renderingUID
    });

    if (typeof btnID != 'undefined' && btnID.length > 0) { jQuery("#" + btnID).attr("disabled", "disabled"); }
    postReq.done(function (data) {
        console.log("Done!");
        console.log(data);
        if (reload != false) {
            location.reload(true);
            hideLoading();
        }
        if (typeof btnID != 'undefined' && btnID.length > 0) { jQuery("#" + btnID).removeAttr("disabled"); }
        if (typeof callBack == 'function') {
            callBack(data);
            hideLoading();
        }
    });
}

function addItemToDataSrc(ItemID, TemplatePath, Suffix, DbName, DsFolderName, reload, callBack) {
    showLoading();
    DsFolderName = DsFolderName.trim();
    reload = typeof reload !== 'undefined' ? reload : true;
    console.log("Posting...");
    var postReq = jQuery.post("/SetDataSource.aspx", {
        method: 'AddItemToDataSource'
        , itemID: ItemID
        , templatePath: TemplatePath
        , suffix: Suffix
        , dbName: DbName
        , dsFolderName: DsFolderName
    });

    postReq.done(function (data) {
        console.log("Done!");
        console.log(data);
        if (typeof callBack == 'function') {
            callBack(data);
            hideLoading();
        }
        else {
            console.log("Callback undefined...");
            try {
                if (reload != false) {
                    console.log("Saving page now!");
                    Sitecore.PageModes.PageEditor.postRequest('webedit:save()', function () { location.reload(true); hideLoading(); }, true);
                }
                else { showEditDialog(data); }
            }
            catch (ex) {
                console.log("Error saving: " + ex.message);
            }
        }
    });
}

function updateDataSrc(dbName, deviceID, itemID, dsID, rID, reload) {
    showLoading();
    Sitecore.PageModes.PageEditor.postRequest('webedit:updatedatasource(itemID=' + itemID + ', dataSourceID=' + dsID + ', renderingID=' + rID + ', dbName=' + dbName + ', deviceID=' + deviceID + ')',
        function () {
            if (reload != false) {
                console.log("Reloading page!");
                //Sitecore.PageModes.PageEditor.postRequest('webedit:save()', function () { }, false);
                location.reload(true);
                hideLoading();
            }
        },
        false
    );
}

function showEditDialog(id, fields) {
    id = id.trim();
    if (id.indexOf("ERROR:") == -1) {
        if (typeof fields == 'undefined') {
            fields = "Title|Link";
        }
        Sitecore.PageModes.PageEditor.postRequest("webedit:fieldeditor(command={11111111-1111-1111-1111-111111111111}, fields=" + fields + ", id=" + id + ")");
    }
}

function autoSave() {
    whenAvailable("Sitecore", function (t) {
        if (Sitecore.PageModes.PageEditor.isModified()) {
            showLoading();
            console.log("Auto-saving now...");
            Sitecore.PageModes.PageEditor.postRequest('webedit:save()', function () { console.log("Done auto-saving!"); location.reload(true); hideLoading(); }, true);
        }
    });
}

function showLoading() {
    /*console.log('show loading');
    if (jQuery('#loadingOverlay').length <= 0) {
        jQuery('body').append("<div id='loadingOverlay'></div>");
    }
    jQuery('#loadingOverlay').fadeIn('fast');*/
}

function hideLoading() {
    console.log('hide loading');
    if (jQuery('#loadingOverlay').length > 0) {
        jQuery('#loadingOverlay').fadeOut('fast');
    }
}

function whenAvailable(name, callback) {
    var interval = 10; // ms
    window.setTimeout(function () {
        if (window[name]) {
            if (typeof callBack == 'function') {
                callback(window[name]);
            }
        } else {
            window.setTimeout(arguments.callee, interval);
        }
    }, interval);
}

var objDoc;

function resizeIframe(obj) {
    if (typeof obj != 'undefined') {
        if (typeof obj.style != 'undefined') {
            objDoc = obj.contentWindow.document;
            obj.style.height = objDoc.body.clientHeight + 'px';
        } else {
            obj.height(objDoc.body.clientHeight);
        }
        try {
            objDoc.getElementsByTagName("html")[0].style.overflow = "hidden";
        } catch (ex) { }
        window.onresize = function (ev) {
            resizeIframe(obj);
        }
        objDoc.body.onresize = function (ev) {
            resizeIframe(obj);
        }
    }
    else {
        parent.iframeObj = obj;
        console.log('Illegal obj for resizing');
    }
}

function resizeIframeByName(name) {
    var iframeObj = jQuery(document).find('iframe[src*="' + name + '"]').get(0);
    if (typeof iframeObj != 'undefined') {
        iframeObj.style.height = iframeObj.contentWindow.document.body.clientHeight + 'px';
        window.onresize = function (ev) {
            resizeIframe(iframeObj);
        }
    }
    else { console.log('Could not find iframe: ' + name + ' to resize.'); }
}

function deleteItem(id) {
    id = id.trim();
    Sitecore.PageModes.PageEditor.postRequest("webedit:delete(id=" + id + ")");
}

// Add child page item
function addChildPage() {
    Sitecore.PageModes.PageEditor.postRequest('webedit:new()');
}

// Check empty placeholders in page editor
function listEmptyPlaceholder() {
    jQuery(".scEmptyPlaceholder").each(function () {
        var codeElement = jQuery(this).next().find('.scpm');
        var value = (codeElement["context"]["nextElementSibling"]["attributes"]["hintname"]["nodeValue"]);
        var msg = "Please add " + value;
        if (value == "HPM Page Type") {
            msg = "Start here by clicking 'Add to here' and selecting a page-type";
        }
        jQuery(this).html("<br /><br /><span style='font-size: 17px;'>" + msg + "</span>");
    });
}

// Display message for empty placeholder
function msgEmptyPlaceholder(currentPath) {

    var postReq = jQuery.post(currentPath, function (data) {
    }).done(function (data) {
        Sitecore.PageModes.PageEditor.postRequest(null, listEmptyPlaceholder(), true);
    });
}

document.addEventListener('paste', function (e) {
    var content = e.clipboardData.getData('text/plain');
    document.execCommand('insertText', false, content);
    e.preventDefault();
    return false;
});