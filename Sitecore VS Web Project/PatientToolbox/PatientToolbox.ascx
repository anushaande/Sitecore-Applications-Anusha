<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="HealthIS.PatientToolbox.PatientToolboxSublayout" Codebehind="PatientToolbox.ascx.cs" %>
<% 
    string title1 = "<strong>Find</strong>a doctor</span>";
    string title2 = "<strong>Request</strong>an appointment online</span>";
    string title3 = "<strong>Refer</strong>a patient</span>";
    string url1 = "http://hsccf.hsc.usf.edu/clinicaldirectory/directorysearch/search.cfm";
    string url2 = "https://hsccf.hsc.usf.edu/patientappointment/requestform.cfm";
    string url3 = "http://health.usf.edu/doctors/medical-professionals.html";

    if (!String.IsNullOrEmpty(Title1)) { title1 = Title1; }
    if (!String.IsNullOrEmpty(Title2)) { title2 = Title2; }
    if (!String.IsNullOrEmpty(Title3)) { title3 = Title3; }
    if (!String.IsNullOrEmpty(URL1)) { url1 = URL1; }
    if (!String.IsNullOrEmpty(URL2)) { url2 = URL2; }
    if (!String.IsNullOrEmpty(URL3)) { url3 = URL3; }
%>

<aside class="ptbr_wrapper">
    <header class="ptbr_header">How can we help <img src="/layouts/images/ptbr_question.png" alt="?"></header>
    <div class="ptbr_panel">
        <nav class="ptbr_links clearfix">
            <header class="ptbr_footer ptbr_btn">
                <span class="ptbr_link">
                    <%--<strong><asp:Placeholder ID="Placeholder1" runat="server"><%= PatientToolboxNumber %></strong></asp:Placeholder> for all appointments--%>
                    <strong><a href="tel:<%= PatientToolboxNumber %>" onclick="_gaq.push(['_trackEvent','Phone Call Tracking','Click/Touch','Flyout']);"><%= PatientToolboxNumber %></a></strong> for all appointments
                </span>
            </header>
            <a href="<%= url1 %>" id="A1" class="ptbr_btn sky"><span class="ptbr_link"><%= title1 %></span></a>
            <a href="<%= url2 %>" id="A2" class="ptbr_btn blue"><span class="ptbr_link"><%= title2 %></span></a>
            <a href="<%= url3 %>" id="A3" class="ptbr_btn blue"><span class="ptbr_link"><%= title3 %></span></a>
        </nav>
    </div>
</aside>