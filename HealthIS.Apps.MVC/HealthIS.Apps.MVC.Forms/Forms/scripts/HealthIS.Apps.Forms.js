Element.prototype.remove = function () {
    this.parentElement.removeChild(this);
}
NodeList.prototype.remove = HTMLCollection.prototype.remove = function () {
    for (var i = this.length - 1; i >= 0; i--) {
        if (this[i] && this[i].parentElement) {
            this[i].parentElement.removeChild(this[i]);
        }
    }
}
try {
    document.getElementById("scWebEditRibbon").remove();
    document.getElementById("ribbonPreLoadingIndicator").remove();
    console.log('removing annoying spinner');
    document.getElementById("scCrossPiece").remove();
}
catch (ex) { }
//Done removing Sitecore ribbon.

function reSizeParentIframe() {
    //console.log("Page load stuff");
    var formName = theForm.action.substring(theForm.action.split("/", 3).join("/").length, theForm.action.length);

    // prevent form from being opened as single webpage
    var refer = document.referrer;
    if (refer.length == 0) {
        window.location.href = "/PageNotFound";
    }
    else {
        var iframeObj = jQuery(parent.document).find('iframe[src*="' + formName + '"]').get(0);
        if (typeof iframeObj != 'undefined') {
            parent.resizeIframeByName(formName);
        }
        else {
            window.location.href = "/PageNotFound";
        }
    }
}

parent.theForm = window.theForm;

Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(reSizeParentIframe);

function WebForm_OnSubmit() {
    var isInvalid = false;
    var invalidCtrls = [];
    if (typeof (ValidatorOnSubmit) == "function" && ValidatorOnSubmit() == false) {
        for (var i in Page_Validators) {
            try {
                var controltovalidate = Page_Validators[i].controltovalidate;
                var control = document.getElementById(controltovalidate);
                if (!Page_Validators[i].isvalid) {
                    jQuery(control).attr("class", "form-control form-control-invalid");
                    isInvalid = true;
                    invalidCtrls[invalidCtrls.length] = controltovalidate;
                } else if (invalidCtrls.indexOf(controltovalidate) == -1) {
                    jQuery(control).attr("class", "form-control");
                }
            } catch (e) { }
        }
        if (isInvalid) {
            if (jQuery("div.alert-danger").length == 0) {
                jQuery(".btn-submit").after(jQuery('<div class="alert alert-danger" role="alert" tabindex="0">Required fields cannot be left empty. Please fill out required fields and re-submit.</div>'));
                try {
                    var formUrlSplit = window.location.href.split('/');
                    parent.formUrlSplit = formUrlSplit;
                    var form = formUrlSplit[formUrlSplit.length - 1];
                    parent.resizeIframeByName(form);
                } catch (ex) { parent.console.log("IFrame resizing error: " + ex.message); }
            }
        }
        return false;
    }
    return true;
}