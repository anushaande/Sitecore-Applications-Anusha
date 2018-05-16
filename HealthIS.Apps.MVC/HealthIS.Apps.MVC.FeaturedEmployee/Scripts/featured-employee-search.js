// Search directory
function searchFacDir(datasourceID) {
    var _fname = jQuery('#firstName').val();
    var _lname = jQuery('#lastName').val();
    var j = "";

    jQuery('#searchErr').text("");
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
                jQuery('#searchErr').text("Could not find person ('" + _fname + " " + _lname + "')");
            }
            else {
                ul = jQuery('#searchModal .searchResults').get(0);
                ul.innerHTML = '<thead><tr><th style="text-align: center">HSC ID</th><th style="text-align:center">Full Name</th></tr></thead>';

                for (i = 0; i < j.Table.length; i++) {
                    // get another ajax to call additional data
                    var _person_id = j.Table[i].PERSON_ID;
                    var _hsc_id = j.Table[i].HSCNET_ID;
                    getListField(_person_id, _hsc_id, datasourceID);
                }
                jQuery('#searchModal').modal("show");
            }
        }
    });
}

// Call ajax to get more data
function getListField(personID, hscnetID, datasourceID) {
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
                    jQuery(ul).append("<tbody><tr onclick='applyChosenSearchResult(this,\"" + datasourceID + "\")' data-personid='" + personID + "'><td>" + hscnetID + "</td><td>" + appt.Table[i].FIRST_NAME + " " + appt.Table[i].LAST_NAME + "</td></th></tbody>");
                }
            }
        }
    });
}

// Choose a facutly and update the info on page
function applyChosenSearchResult(li, datasourceID) {
    if (typeof jQuery(li) != 'undefined') {
        var personid = jQuery(li).data("personid");

        var rtcSpan = jQuery('#list span.scWebEditInput').get(0);
        var fieldID = "#" + rtcSpan.id.replace("_edit", "");
        var currentListData = jQuery(fieldID).val();

        if (!currentListData.includes(personid)) {
            if (currentListData != "") { currentListData = currentListData + ";" + personid; }
            else { currentListData = personid; }

            var isUpdated = Sitecore.PageModes.PageEditor.postRequest("item:updateitemfield(id=" + datasourceID + ", fieldName=PersonID List, fieldValue=" + currentListData + ")");
            if (isUpdated) {
                jQuery('#searchModal').modal('hide');
            }
        } else { alert("The person you've selected is already in the list. Please choose another."); }
    }
}

// initially clear modal popup
$(document).ready(function () {
    $('#searchModal').modal("hide");
});