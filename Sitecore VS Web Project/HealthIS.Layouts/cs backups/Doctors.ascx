<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="HealthIS.Layouts.DoctorsSublayout" Codebehind="Doctors.ascx.cs" %>

<header id="health-header">
    <div id="health-header-inner" class="container block-center">
        <nav>
            <ul class="health-links clearfix">
                <li><a href="http://health.usf.edu/news.html">USF Health News</a></li>
            </ul>
         </nav>
        <!-- Placeholder: usf links -->
        <sc:placeholder runat="server" key="USFLinks"></sc:placeholder>
        <!-- end usf links -->
    </div>
</header>

<header id="dept-header">
    <!-- BEGIN_REPLACE|header -->
    <div class="container block-center">
        <div class="logo_ribbon">
            <div id="logo">
                <a href="http://health.usf.edu" id="health-logo"><img src="/layouts/images/health_logo_gold.png" alt="" width="101" height="68" /></a>
            </div>
        </div>
        <h1 class="doctors-wordmark"><a href="/doctors/"><strong>Doctors</strong> of USF Health</a></h1>
        <h2>
            <!-- Placeholder: department title -->
            <sc:placeholder runat="server" key="pageTitle"></sc:placeholder>
            <!-- end department title -->
        </h2>
    </div>
    <!-- END_REPLACE|header -->
</header>

<!-- BEGIN_REMOVE|horizontalNavigation -->
<!-- Placeholder: department navigation -->
<sc:placeholder runat="server" key="DeptNav"></sc:placeholder>
<!-- end department navigation -->
<!-- END_REMOVE|horizontalNavigation -->

<div class="container">
    <!-- Placeholder: breadcrumbs -->
    <sc:placeholder runat="server" key="Breadcrumbs"></sc:placeholder>
    <!-- end breadcrumbs -->
</div>

<section class="container block-center site-content">
    <div id="generic_left">
        <!-- BEGIN_REPLACE|content -->
        <sc:placeholder runat="server" key="RowContainer"></sc:placeholder>
        <!-- END_REPLACE|content -->
    </div>
    <div id="generic_right">
        <sc:placeholder runat="server" key="RightColDocs"></sc:placeholder>
    </div>
</section>

<footer id="dept-footer">
    <div class="container">
        <div id="contact-panel">
            <h1 class="doctors-wordmark"><strong>Doctors</strong> of USF Health</h1>
            <h2 class="usf-subtitle">University of South Florida</h2>
            <div class="spacemaker">
                <!-- BEGIN_REPLACE|footer  -->
                <!-- vcard -->
                <!-- Custom editable footer area -->
                <sc:placeholder runat="server" key="CustomFooter"></sc:placeholder>
                <!-- END_REPLACE|footer  -->
            </div>
            <span id="modified">This page was last modified on: <asp:Literal ID="litModifiedDate" runat="server"></asp:Literal></span>
        </div>
        <!-- user control footer links -->
        <div id="footer-links">
            <nav>
                <h4>Patient Information</h4>
                <ul>
                    <li><a href="http://health.usf.edu/myhealthcare/insurance.htm">Insurance</a></li>
                    <li><a href="http://health.usf.edu/myhealthcare/billing.htm">Billing</a></li>
                    <li><a href="http://health.usf.edu/myhealthcare/pol_information.html">Patient Online</a></li>
                    <li><a href="http://health.usf.edu/myhealthcare/patient_forms.html">Forms</a></li>
                    <li><a href="http://health.usf.edu/myhealthcare/health_records.htm">Electronic Health Records</a></li>
                    <li><a href="http://health.usf.edu/myhealthcare/plan_visit.htm">Planning Your Visit</a></li>
                </ul>
            </nav>
            <nav>
                <h4>Patient Resources</h4>
                <ul>
                    <li><a href="http://health.usf.edu/myhealthcare/clinicalservices/">Clinical Services</a></li>
                    <li><a href="http://ctlsv2.emergingmed.com/Content/Partner/USF_AD/Index.htm" target="_blank">Clinical Trials</a></li>
                    <li><a href="http://health.usf.edu/nocms/myhealthcare/community_events.html">Community Events</a></li>
                    <li><a href="http://health.usf.edu/myhealthcare/family_accommodations.htm">Family Accommodations</a></li>
                    <li><a href="http://health.usf.edu/myhealthcare/patient_rights.html">Patient Rights</a></li>
                    <li><a href="http://health.usf.edu/nocms/myhealthcare/pdfs/USFNoticeofPrivacyPractices.pdf">Privacy</a></li>
                </ul>                   
            </nav>
            <nav>
                <h4>Hospital Partners</h4>
                <ul>
                    <li><a href="http://www.allkids.org" target="_blank">All Children's Hospital</a></li>
                    <li><a href="http://www.elevatinghealthcare.org" target="_blank">Florida Hospital</a></li>
                    <li><a href="http://www.hcawestflorida.com/our-services/trauma-care.dot" target="_blank">HCA Hospitals</a></li>
                    <li><a href="http://www.lrmc.com" target="_blank">Lakeland Regional<br />Medical Center</a></li>
                    <li><a href="http://www.tgh.org" target="_blank">Tampa General Hospital</a></li>
                </ul>                  
            </nav>
            <nav>
                <h4>Useful Links</h4>
                <ul>
                    <li><a href="https://webmail.health.usf.edu" target="_blank">USF Health Webmail</a></li>
                    <li><a href="https://usfpgcareersource.health.usf.edu">Jobs / Careers</a></li>
                    <li><a href="http://health.usf.edu/myhealthcare/hr/">Human Resources</a></li>
                    <li><a href="http://health.usf.edu/is/home.htm">USF Health Information Systems</a></li>
                    <li><a href="https://usfweb2.usf.edu/foundation/asp/ssl/adfdn/college.asp">Giving to USF Health</a></li>
                    <li><a href="http://www.cme.hsc.usf.edu">Continuing Professional Development</a></li>
                </ul>
            </nav>
        </div>
    </div>
</footer>

<footer id="health-footer">
    <div class="container">
        <div id="credits">
            <a class="powered" href="http://health.usf.edu/is">
                <img src="/layouts/images/health_is.png" width="62" height="62" alt="" />
                <span>Powered and Designed by <br />USF Health Information Systems</span>
            </a>
        </div>

        <nav id="docs-footer-right">
            <ul>
                <li><a href="http://health.usf.edu">USF Health</a><span class="separator">&middot;</span></li>
                <li><a href="http://health.usf.edu/research/">Research</a><span class="separator">&middot;</span></li>
                <li><a href="http://health.usf.edu/education.html">Academics</a><span class="separator">&middot;</span></li>
                <li><a href="http://health.usf.edu/doctors/">Patient Care</a></li>
            </ul>
        </nav>
    </div>
</footer>