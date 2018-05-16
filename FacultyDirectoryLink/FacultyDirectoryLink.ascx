<%@ Control Language="c#" AutoEventWireup="True" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="Layouts.Facultydirectorylink.FacultydirectorylinkSublayout" Codebehind="FacultyDirectoryLink.ascx.cs" %>

<div runat="server" id="tempThing"></div>

<div style="border:1px dashed #666; padding:1em; background:#e3e3e3;">
  <asp:MultiView ID="mvFacultyLink" runat="server" OnActiveViewChanged="mvFacultyLink_ActiveViewChanged">
	<asp:View ID="viewNormal" runat="server">
        <asp:Literal runat="server" ID="myDisplay"></asp:Literal>
    </asp:View>
    <asp:View ID="viewEditor" runat="server">
      <div id='<%# ((Sitecore.Web.UI.WebControls.Sublayout)this.Parent).UniqueID.ToString() %>'>
        <form id="form1" runat="server">
        <input type="checkbox" ID="clinicalResearchSwitch" onchange="hit_facultySearchTypeChanged(this)" checked="checked"/><Label for="clinicalResearchSwitch">Is Clinical</Label>
        <div runat="server" id="clinicalSearch">
            <asp:Label ID="ClinicalSearchLabel" AssociatedControlID="doctorLookup_fn" runat="server"><b>Search Doctor by lastname, first</b></asp:Label><br />
            <asp:TextBox runat="server" ID="doctorLookup_fn" Columns="20"></asp:TextBox>
            <asp:TextBox runat="server" ID="doctorLookup_ln" Columns="20"></asp:TextBox>
            <asp:TextBox runat="server" ID="doctorLookup_hscid" Columns="20"></asp:TextBox>
        </div>
        <div runat="server" id="researchSearch" class="hit_hide">
            <asp:Label ID="ResearchSearchLabel" AssociatedControlID="researchLookup_term" runat="server"><b>Search Person By lastname, first or HSCID</b></asp:Label>
            <br />
            <asp:textbox runat="server" ID="researchLookup_term" Columns="40"></asp:textbox>
        </div>
        <input type="hidden" value="" name="faculty_pid" />
        </form>
        <script type="text/javascript" src="/layouts/scripts/jquery.min.js"></script>
        <script type="text/javascript" src="/layouts/scripts/jquery-ui-1.10.0.min.js"></script>
        <link type="text/css" rel="stylesheet" href="/layouts/styles/jquery-ui.min.css" />

        <style>
            .hit_hide
            {
                display:none;
            }
        </style>

        <script type="text/javascript">
            function hit_facultySearchTypeChanged(itm) {
                if ($(itm).prop("checked")) {
                    $('div[id$=clinicalSearch]').show();
                    $('div[id$=researchSearch]').hide();
                } else {
                    $('div[id$=clinicalSearch]').hide();
                    $('div[id$=researchSearch]').show();
                }
            }
            function ACResearchNameSource(request, response) {
                var term = $('input[name$=researchLookup_term]').val();
                $.ajax({
                    url: "/Sublayouts/components/FacultyDirectory.aspx",
                    type: "POST",
                    dataType: "json",
                    data: { method: "ajaxacResearchName", term: term },
                    error: function () { alert('error'); },
                    success: function (data) {
                        response(data);
                    }
                });
            }
            function ACResearchNameSelect(event, ui) {
                $('input[name$=faculty_pid]').val(ui.item.person_id);
                $('input[name$=researchLookup_term]').val(ui.item.label);
                return false;
            }
            function ACFacultyNameSource(request, response) {
                var fname = $('input[name$=doctorLookup_fn]').val();
                var lname = $('input[name$=doctorLookup_ln]').val();
                var hscid = $('input[name$=doctorLookup_hscid]').val();
                $.ajax({
                    url: "/Sublayouts/components/FacultyDirectory.aspx",
                    type: "POST",
                    dataType: "json",
                    data: { method: "ajaxacfacultyName", fname: fname, lname: lname, hscid: hscid },
                    error: function () { alert('error'); },
                    success: function (data) {
                        response(data);
                    }
                });
            }
            function ACFacultyNameSelect(event, ui) {
                $('input[name$=doctorLookup_fn]').val(ui.item.first_name);
                $('input[name$=doctorLookup_ln]').val(ui.item.last_name);
                $('input[name$=doctorLookup_hscid]').val(ui.item.hscid);
                $('input[name$=faculty_pid]').val(ui.item.person_id);
                return false;
            }
            $(function () {
                $('input[name$=doctorLookup_fn]').autocomplete({
                    source: ACFacultyNameSource,
                    select: ACFacultyNameSelect
                });

                $('input[name$=researchLookup_term]').autocomplete({
                    source: ACResearchNameSource,
                    select: ACResearchNameSelect
                });

            });
        </script>
      </div>
    </asp:View>
  </asp:MultiView>
</div>
