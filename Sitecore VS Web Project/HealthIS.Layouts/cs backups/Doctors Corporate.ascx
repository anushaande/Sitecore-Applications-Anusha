<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="HealthIS.Layouts.Doctors_corporateSublayout" Codebehind="Doctors Corporate.ascx.cs" %>

<section id="cloud-header">
    <header class="container block-center">
        <h1 class="doctors-wordmark"><a href="/doctors/"><strong>Doctors</strong> of USF Health</a></h1>
        <div class="logo_ribbon">
            <div id="logo">
                <a href="http://health.usf.edu" id="health-logo"><img src="/layouts/images/health_logo_gold.png" alt="" width="101" height="68" /></a>
            </div>
        </div>

        <!-- Placeholder: health links -->
        <div id="health-header">
            <div id="health-header-inner">
                <sc:placeholder runat="server" key="HealthLinks"></sc:placeholder>
            </div>
        </div>
        <!-- end health links -->

        <!-- Placeholder: usf links -->
        <sc:placeholder runat="server" key="USFLinks"></sc:placeholder>
        <!-- end usf links -->
    </header>
    <div id="intro" class="clearfix">
        
        <!-- Patient toolbox goes here -->
    </div>
</section>

<!-- THIS IS BAD. THIS SHOULD BE A USER FRIENDLY COMPONENT. -->
<nav class="navtacular navtacular-theme-simple container block-center">
    <h1 class="navtacular-label"><i class="icon-reorder"></i>Doctors Navigation</h1>
    <ul class="navtacular-list">
        <li class="navtacular-item"><a class="navtacular-link" href="http://health.usf.edu/doctors/locations.html">View our<strong>Locations</strong></a></li>
        <li class="navtacular-item"><a class="navtacular-link" href="http://health.usf.edu/doctors/specialties.html">List of<strong>Specialties</strong></a></li>
        <li class="navtacular-item"><a class="navtacular-link" href="http://health.usf.edu/doctors/patient-information.html">Patient<strong>Information</strong></a></li>
        <li class="navtacular-item"><a class="navtacular-link" href="http://health.usf.edu/doctors/medical-professionals.html">For Medical<strong>Professionals</strong></a></li>
        <li class="navtacular-item"><a class="navtacular-link" href="http://health.usf.edu/doctors/why-usf-health.html">Why<strong>USF Health</strong></a></li>
    </ul>
</nav>

<section class="container block-center site-content">
    <!-- BEGIN_REPLACE|content -->
    <sc:placeholder runat="server" key="RowContainer"></sc:placeholder>
    <!-- END_REPLACE|content -->
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