// Add new branch
function addBranch(itemId, value, key) {
    $.ajax({
        url: window.location.href,
        type: "POST",
        data: { "itemId": itemId.trim(), "value": value, "key": key, "type": "Add New Branch" },
        context: this,
        success: function (data) {
            Sitecore.PageModes.PageEditor.postRequest(null, function () { location.reload(true); }, true);
        },
        error: function (data) {
            alert('Error: ' + data);
            console.log(data);
        }
    });
}

// Add new faculty
function addFaculty(itemId, key, value) {
    $.ajax({
        url: window.location.href,
        type: "POST",
        data: { "itemId": itemId.trim(), "key": key.trim(), "value": value, "type": "Add New Faculty" },
        context: this,
        success: function (data) {
            Sitecore.PageModes.PageEditor.postRequest(null, function () { location.reload(true); }, true);
        },
        error: function (data) {
            alert('Error: ' + data);
        }
    });
}

// Remove Faculty
function removeFaculty(itemId, key) {
    $.ajax({
        url: window.location.href,
        type: "POST",
        data: { "itemId": itemId.trim(), "key": key.trim(), "type": "Remove Faculty" },
        context: this,
        success: function (data) {
            Sitecore.PageModes.PageEditor.postRequest(null, function () { location.reload(true); }, true);
        },
        error: function (data) {
            alert('Error: ' + data);
        }
    });
}

// Sort Faculty List
function sortFaculty(itemId, key, movementTo) {
    $.ajax({
        url: window.location.href,
        type: "POST",
        data: { "itemId": itemId.trim(), "key": key.trim(), "movementTo": movementTo, "type": "Sort Faculty" },
        context: this,
        success: function (data) {
            Sitecore.PageModes.PageEditor.postRequest(null, function () { location.reload(true); }, true);
        },
        error: function (data) {
            alert('Error: ' + data);
        }
    });
}

// Get multiple IDs
function addMultipleFaculty(itemId, sectionId, data) {
    var data = data.replace(/ /g, '');
    if (data == "") { alert("Please add faculty ID"); return; }
    $('#multipleModal_' + sectionId.trim() + ' .btnAddMultipleIDs').attr("disabled", "disabled");
    $('#multipleModal_' + sectionId.trim() + ' .btnAddMultipleIDs').attr("value", "I'm working. Please wait...");
    $.ajax({
        url: window.location.href,
        type: "POST",
        data: { "itemId": itemId.trim(), "value": data, "type": "Add Multiple Faculty" },
        context: this,
        success: function (data) {
            $('#multipleModal_' + sectionId).modal("hide");
            Sitecore.PageModes.PageEditor.postRequest(null, function () { location.reload(true); }, true);
        },
        error: function (data) {
            alert('Error: ' + data);
        }
    });
}

// Let Sitecore know something has been modified
function updateData(sectionId) {
    var rtcSpan = jQuery('#hidAllData_' + sectionId.trim() + ' span.scWebEditInput').get(0);
    var fieldID = "#" + rtcSpan.id.replace("_edit", "");
    var allData = "";
    var count = 1;

    $('[id^="tbJTitle_' + sectionId.trim() + '_"]').each(function () {
        var personId = $(this).attr("id").replace("tbJTitle_" + sectionId.trim() + "_", "");
        var value = $(this).val();

        if (count > 1) { personId = "&" + personId; }

        if (value.trim() != "") {
            value = value.replace(/\&/g, ";amp;");
            value = value.replace(/\=/g, ";eq;");
        } else {
            value = ";dump;";
        }
        allData += personId + "=" + unescape(value);

        count++;
    });

    jQuery(fieldID).val(allData);
}

// Search directory
function searchFacDir(personId, sectionId) {
    var _fname = jQuery('#firstName_' + sectionId + '_' + personId).val();
    var _lname = jQuery('#lastName_' + sectionId + '_' + personId).val();
    var j = "";

    jQuery('#searchErr_' + sectionId.trim() + '_' + personId.trim()).text("");
    jQuery.ajax({
        url: "/Sublayouts/components/FacultyDirectory.aspx",
        data: {
            method: "searchhd",
            type: "Search Faculty",
            fname: _fname,
            lname: _lname
        },
        success: function (e) {
            j = JSON.parse(e);
            if (typeof j.Table == 'undefined' || j.Table[0] == null) {
                jQuery('#searchErr_' + sectionId.trim() + '_' + personId.trim()).text("Could not find person");
            }
            else {
                ul = jQuery('#searchModal_' + sectionId.trim() + '_' + personId.trim() + " .searchResults").get(0);
                ul.innerHTML = '<thead><tr><th style="text-align: center;">HSC ID</th><th style="text-align: center;">Full Name</th></tr></thead>';

                for (i = 0; i < j.Table.length; i++) {
                    // get another ajax to call additional data
                    var _person_id = j.Table[i].PERSON_ID;
                    var _hsc_id = j.Table[i].HSCNET_ID;
                    var _email = j.Table[i].EMAIL;
                    getListField(_person_id, personId, _hsc_id, _email, sectionId);
                }
                jQuery('#searchModal_' + sectionId.trim() + '_' + personId.trim()).modal("show");
            }
        }
    });
}

// Call ajax to get more data
function getListField(personID, num, hsc_id, email, sectionId) {
    jQuery.ajax({
        url: "/Sublayouts/components/FacultyDirectory.aspx",
        data: {
            method: "listfields",
            person_id: personID
        },
        success: function (e) {
            appt = JSON.parse(e);
            if (personID != null && typeof personID != 'undefined') {
                for (i = 0; i < appt.Table.length; i++) {
                    jQuery(ul).append("<tbody><tr onclick='applyChosenSearchResult(this, \"" + sectionId.trim() + "\")' data-num='" + num + "' data-fname='" + escape(appt.Table[i].FIRST_NAME) + "' data-lname='" + escape(appt.Table[i].LAST_NAME) + "' data-personid='" + personID + "' data-credentials='" + appt.Table[i].TITLE + "' data-hscid='" + hsc_id + "' data-email='" + email + "'><td>" + hsc_id + "</td><td>" + appt.Table[i].FIRST_NAME + " " + appt.Table[i].LAST_NAME + "</td></th></tbody>");
                }
            }
        }
    });
}

// Choose a facutly and update the info on page
function applyChosenSearchResult(li, sectionId) {
    if (typeof jQuery(li) != 'undefined') {
        var num = jQuery(li).data("num"); // original temporary id
        var fname = unescape(jQuery(li).data("fname"));
        var lname = unescape(jQuery(li).data("lname"));
        var credentials = jQuery(li).data("credentials");
        var personid = jQuery(li).data("personid");
        var hscid = jQuery(li).data("hscid");
        var email = jQuery(li).data("email");
        var link = jQuery(li).data("link");

        // If faculty exists, show error
        if ($('.lblPersonID_' + personid).length > 0) {
            alert("The faculty is already in the list");
            return false;
        }

        sectionId = sectionId.trim();

        // Set faculty info into each field
        jQuery('#lblFName_' + num).val(fname);
        jQuery('#lblLName_' + num).val(lname);
        jQuery('#lblCredentials_' + num).val(credentials);
        jQuery('#lblPersonID_' + num).val(personid);
        jQuery('#imgProfilePic_' + num).attr("src", "/resources/root-elements/FacultyDirectoryGetFile.cshtml?method=picture&person_id=" + personid);
        jQuery('#lblHSCID_' + num).val(hscid);
        jQuery('#lblEmail_' + num).val(email);
        jQuery('#tbJTitle_' + sectionId + '_' + num).html("");

        // Update faculty data elements with updated id
        jQuery('#lblFName_' + num).attr("id", "lblFName_" + personid);
        jQuery('#lblLName_' + num).attr("id", "lblLName_" + personid);
        jQuery('#lblCredentials_' + num).attr("id", "lblCredentials_" + personid);
        jQuery('#lblHSCID_' + num).attr("id", "lblHSCID_" + personid);
        jQuery('#lblEmail_' + num).attr("id", "lblEmail_" + personid);
        jQuery('#lblPersonID_' + num).attr("id", "lblPersonID_" + personid);
        jQuery('#imgProfilePic_' + num).attr("id", "imgProfilePic_" + personid);
        jQuery('#tbJTitle_' + sectionId + '_' + num).attr("id", "tbJTitle_" + sectionId + "_" + personid);

        jQuery('#searchModal_' + sectionId + '_' + num).attr("id", "searchModal_" + sectionId + '_' + personid);
        jQuery('#searchErr_' + sectionId + '_' + num).attr("id", "searchErr_" + sectionId + '_' + personid);
        jQuery('#btnSearchDir_' + sectionId + '_' + num).attr("onclick", "searchFacDir('" + personid + "', '" + sectionId + "')");
        jQuery('#btnSearchDir_' + sectionId + '_' + num).attr("id", "btnSearchDir_" + sectionId + "_" + personid);
        jQuery('#firstName_' + num).attr("id", "firstName_" + personid);
        jQuery('#lastName_' + num).attr("id", "lastName_" + personid);

        // Let Sitecore know something modified
        updateData(sectionId.trim());
        jQuery('#searchModal_' + sectionId.trim() + '_' + personid).modal('hide');
        window.frames["scWebEditRibbon"].contentWindow.scForm.setModified(true);
    }
}

// Toggle for multiple faculty information in each section
function toggleFD(sectionId) {
    $(document).find('.toggleFD_' + sectionId).slideToggle(null, function () {
        if ($(this).is(":hidden")) {
            $(".btnToggleFD_" + sectionId).attr("Value", "► Expend");
        } else {
            $(".btnToggleFD_" + sectionId).attr("Value", "▼ Collapse");
        }
    });
}

// Initially clear Modal popup
$(document).ready(function () {
    $('[id^="searchModal_"]').modal("hide");
    $('[id^="multipleModal_"]').modal("hide");
    //if ($('[id^="lblPersonID_"]').value != "") { this.attr("display", "block"); }
});

