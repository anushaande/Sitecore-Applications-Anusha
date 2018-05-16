<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="HealthIS.Layouts.CorporateSublayout" Codebehind="Corporate.ascx.cs" %>

<header id="corp-header">
    <div class="container block-center">

        <h1><span class="welcome">Welcome</span> to USF Health at the <br /><span class="university">University of South Florida,</span> Tampa</h1>

        <a href="http://health.usf.edu" id="corp-logo"><img src="/layouts/images/corp-logo.png" alt="USF Health" height="174" width="247" /></a>

        <!-- Placeholder: health links -->
        <sc:placeholder runat="server" key="HealthLinks"></sc:placeholder>
        <!-- end health links -->

        <!-- Placeholder: usf links -->
        <sc:placeholder runat="server" key="USFLinks"></sc:placeholder>
        <!-- end usf links -->
    </div>
</header>

<!-- BEGIN_REPLACE|breadCrumbs -->
<section id="breadcrumbs" class="container block-center">
    <!-- Placeholder: breadcrumbs -->
    <sc:placeholder runat="server" key="Breadcrumbs"></sc:placeholder>        
    <!-- end breadcrumbs -->
</section>
<!-- END_REPLACE|breadCrumbs -->

<!-- BEGIN_REMOVE|horizontalNavigation -->
<!-- Placeholder: department navigation -->
<sc:placeholder runat="server" key="DeptNav"></sc:placeholder>
<!-- end department navigation -->
<!-- END_REMOVE|horizontalNavigation -->

<section class="container block-center site-content clearfix tapered-underline">
    <!-- BEGIN_REPLACE|content -->
    <sc:placeholder runat="server" key="RowContainer"></sc:placeholder>
    <!-- END_REPLACE|content -->
</section>

<nav id="social-media" class="container block-center">
    <h3>Engage:</h3>
    <ul>
        <li><a href="http://www.facebook.com/usfhealth" class="facebook">Facebook</a></li>
        <li><a href="https://twitter.com/#!/USFHealth" class="twitter">Twitter</a></li>
        <li><a href="http://health.usf.edu/social_media.html" class="connect">More ways to connect</a></li>
        <li><a href="http://health.usf.edu/giving/" class="giving">Giving to USF Health</a></li>
    </ul>
</nav>

<footer id="corp-footer">
    <div class="container block-center">
        <div class="left">
            <img src="/layouts/images/usf-corp-wordmark.png" alt="University of South Florida" width="296" height="84" class="usf-wordmark" />
            <!-- BEGIN_REPLACE|footer  -->
            <sc:placeholder runat="server" key="vCard"></sc:placeholder>
            <!-- END_REPLACE|footer  -->
        </div>
        <div class="site-map">
            <nav>
                <h2>USF Health</h2>
                <ul>
                    <li><a href="http://health.usf.edu/about.html">About Us</a></li>
                    <li><a href="http://health.usf.edu/VP/leadership/">Leadership</a></li>
                    <li><a href="http://health.usf.edu/doctors/">Patient Care</a></li>
                    <li><a href="http://health.usf.edu/strategic_initiatives.html">Strategic Initiatives</a></li>
                    <li><a href="http://hsc.usf.edu/publicaffairs/index.html">Communications</a></li>
                    <li><a href="http://usfweb2.usf.edu/human-resources/">Careers (Academic)</a></li>
                    <li><a href="https://usfpgcareersource.health.usf.edu/ ">Careers (Healthcare)</a></li>
                    <li><a href="http://health.usf.edu/maps_directions/index.htm">Directions / Maps</a></li>
                </ul>
            </nav>
            <nav>
                <h2>Degrees</h2>
                <ul>
                    <li><a href="http://health.usf.edu/medicine/">Medicine</a></li>
                    <li><a href="http://health.usf.edu/nursing/">Nursing</a></li>
                    <li><a href="http://health.usf.edu/publichealth/">Public Health</a></li>
                    <li><a href="http://health.usf.edu/pharmacy/">Pharmacy</a></li>
                    <li><a href="http://health.usf.edu/medicine/dpt/">Physical Therapy</a></li>
                    <li><a href="http://gradaffairs.health.usf.edu/">Biomedical Sciences</a></li>
                </ul>
            </nav>
            <nav>
                <h2>For You</h2>
                <ul>
                    <li><a href="http://health.usf.edu/doctors/">Patient Care</a></li>
                    <li><a href="http://health.usf.edu/students.html">Students</a></li>
                    <li><a href="http://health.usf.edu/faculty_staff.html">Faculty &amp; Staff</a></li>
                    <li><a href="http://www.usfhealthalumni.net/">Alumni</a></li>
                    <li><a href="http://health.usf.edu/newscenter.html">Media</a></li>
                    <li><a href="http://ctlsv2.emergingmed.com/Content/Partner/USF_AD/Index.htm" target="_blank">Clinical Trials</a></li>
                    <li><a href="http://health.usf.edu/publicaffairs/logos_templates.html">Logos &amp; Templates</a></li>
                    <li><a href="http://health.usf.edu/nocms/publicaffairs/USFHealthFactSheet.pdf">USF Health Fact Sheet</a></li>
                    <li><a href="http://library.hsc.usf.edu/">Shimberg Library</a></li>
                    <li><a href="http://webmail.health.usf.edu">Web Email</a></li>
                </ul>
            </nav>
        </div>
        <span id="modified">This page was last modified on: <asp:Literal ID="litModifiedDate" runat="server"></asp:Literal></span>
        <div id="corporate-info">
            <div class="credits">
                <a href="http://health.usf.edu/is"><img src="/layouts/images/health_is.png" width="62" height="62" alt=""/></a>
                <a href="http://health.usf.edu/is" id="powered">Powered by<br />
                USF Health Information Systems</a>
            </div>
            <div class="copyright">Copyright &copy; 2011, University of South Florida. All rights reserved.</div>
            <div class="contact">12901 Bruce B. Downs Blvd, Tampa, Florida, USA<br><a href="http://hsccf.hsc.usf.edu/hscanswers/contact_form.cfm" target="_blank">Ask USF Health</a></div>
        </div>
    </div>
</footer>
<footer id="health-tagline"><strong>USF Health:</strong> To envision and implement the future of health.</footer>