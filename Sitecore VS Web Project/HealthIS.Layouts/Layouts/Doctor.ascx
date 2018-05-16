<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Doctor.ascx.cs" Inherits="HealthIS.Layouts.Doctor" %>

<!-- Header -->
<sc:placeholder ID="Placeholder1" key="DoctorHeaderDynamic" runat="server"></sc:placeholder>

<!-- Content Body -->
<main role="main" id="generic_wrapper" class="container block-center site-content">
    <!-- Start Only for Main Page Content Body -->
    <sc:placeholder ID="ph_RowContainerForMain" runat="server" key="RowContainerForMain"></sc:placeholder>
    <!-- End Only for Main Page Content Body --> 

    <!-- Start Only for Sub Page Content Body -->
    <div id="generic_left" class="doctors-content clearfix">
    <sc:placeholder ID="ph_RowContainerForSub" runat="server" key="RowContainerForSub"></sc:placeholder>
    </div>
    <div id="generic_right" class="clearfix">
        <sc:placeholder ID="ph_RightColDocs" runat="server" key="RightColDocs"></sc:placeholder>
        <sc:placeholder ID="ph_RightColSendary" runat="server" key="RightColSecondary"></sc:placeholder>
    </div>
    <!-- End Only for Sub Page Content Body -->
</main>

<!-- Footer -->
<footer role="contentinfo" id="dept-footer">
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
                    <li><a href="http://www.centerwatch.com/UniversityOfSouthFlorida" target="_blank">Clinical Trials</a></li>
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