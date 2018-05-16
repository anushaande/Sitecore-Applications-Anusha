<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="HealthIS.Layouts.AcademicSublayout" Codebehind="Academic.ascx.cs" %>

<header role="banner" id="health-header">
  <div id="health-header-inner" class="container block-center">

    <!-- Placeholder: health links -->
    <sc:placeholder runat="server" key="HealthLinks"></sc:placeholder>
    <!-- end health links -->

    <!-- Placeholder: usf links -->
    <sc:placeholder runat="server" key="USFLinks"></sc:placeholder>
    <!-- end usf links -->

  </div>
</header>

<header role="banner" id="dept-header" class="container block-center">
  <a href="http://health.usf.edu" class="text-hide" id="health-logo">USF Health</a>

  <!-- BEGIN_REPLACE|header -->
  <!-- Placeholder: breadcrumbs -->
  <sc:placeholder runat="server" key="Breadcrumbs"></sc:placeholder>
  <!-- end breadcrumbs -->

  <!-- Placeholder: department title -->
  <h1>
      <sc:placeholder runat="server" key="pageTitle"></sc:placeholder>
  </h1>
  <!-- end department title -->
    
  <div class="template-options">
    <sc:placeholder runat="server" key="TemplateOptions"></sc:placeholder>
  </div>

  <!-- END_REPLACE|header -->
</header>

<!-- BEGIN_REMOVE|horizontalNavigation -->
<!-- Placeholder: department navigation -->
<sc:placeholder runat="server" key="DeptNav"></sc:placeholder>
<!-- end department navigation -->
<!-- END_REMOVE|horizontalNavigation -->

<main role="main" class="container block-center site-content clearfix">
  <!-- BEGIN_REPLACE|content -->
  <sc:placeholder runat="server" key="RowContainer"></sc:placeholder>
  <!-- END_REPLACE|content -->
</main>

<footer role="contentinfo" id="dept-footer">
    <div class="container block-center" id="ctl3">
        <div class="left" id="ctl4">
            <section class="vCard">
                <img src="/layouts/images/usf-wordmark.png" alt="University of South Florida" />
                <!-- BEGIN_REPLACE|footer  -->
                <sc:placeholder runat="server" key="vCard"></sc:placeholder>
                <!-- END_REPLACE|footer  -->
            </section>
            <span id="modified">This page was last modified on: <asp:Literal ID="litModifiedDate" runat="server"></asp:Literal></span>
            <div id="credits">
                <a href="http://health.usf.edu/is"><img src="http://health.usf.edu/cms_author/docs/masterpages/health_corp/images/health_is.png?w=62&amp;h=62&amp;as=1" width="62" height="62" alt="" border="0" /></a>
                <a href="http://health.usf.edu/is" id="powered">Powered By <br />USF Health Information Systems</a>
            </div>
        </div>
        <nav>
            <h1>USF Health</h1>
            <ul>
                <li><a href="http://health.usf.edu/about.html">About</a></li>
                <li><a href="http://health.usf.edu/VP/leadership/">Health Leadership</a></li>
                <li><a href="http://generalcounsel.usf.edu/regulations/">USF Systems</a></li>
                <li><a href="http://hscweb3.hsc.usf.edu/health/now/?p=6237">Comm. Engagement</a></li>
                <li><a href="http://health.usf.edu/intprog/home.html">International Programs</a></li>
                <li><a href="http://hsc.usf.edu/publicaffairs/index.html">Media/Communications</a></li>
                <li><a href="http://health.usf.edu/maps_directions/index.htm">Maps/Directions</a></li>
                <li><a href="http://health.usf.edu/nocms/giving/">Giving</a></li>
                <li><a href="http://hscweb3.hsc.usf.edu/calendar/index.php?calendar=1&amp;v=m">Calendars</a></li>
                <li><a href="http://health.usf.edu/publicaffairs/logos_templates.html">Logos &amp; Templates</a></li>
                <li><a href="http://webmail.health.usf.edu">Web E-mail</a></li>
            </ul>
        </nav>
        <nav>
            <h1>Education</h1>
            <h2>Colleges</h2>
            <ul>
                <li><a href="http://health.usf.edu/medicine/">Medicine</a></li>
                <li><a href="http://health.usf.edu/nocms/nursing/">Nursing</a></li>
                <li><a href="http://health.usf.edu/publichealth/index.htm">Public Health</a></li>
                <li><a href="http://health.usf.edu/nocms/pharmacy/">Pharmacy</a></li>
            </ul>
            <h2>Schools</h2>
            <ul>
                <li><a href="http://health.usf.edu/medicine/dpt/index.htm">Physical Therapy</a></li>
                <li><a href="http://gradaffairs.health.usf.edu/">Biomedical Sciences</a></li>
            </ul>
            <h2>More</h2>
            <ul>
                <li><a href="http://library.hsc.usf.edu/">Shimberg Library</a></li>
                <li><a href="http://www.usfhealthalumni.net/">Alumni</a></li>
                <li><a href="http://hscweb3.hsc.usf.edu/health/now/?p=334">iTunes U</a></li>
                <li><a href="http://www.cme.hsc.usf.edu/">Cont. Prof. Development</a></li>
                <li><a href="http://health.usf.edu/ahec/index.htm">AHEC</a></li>
            </ul>
        </nav>
        <nav>
            <h1>Research</h1>
            <ul>
                <li><a href="http://health.usf.edu/research/home.html">USF Health</a></li>
                <li><a href="http://health.usf.edu/medicine/research/home.html">Medicine</a></li>
                <li><a href="http://health.usf.edu/nocms/nursing/ResearchCenters.html">Nursing</a></li>
                <li><a href="http://health.usf.edu/publichealth/officeresearch/index.htm">Public Health</a></li>
                <li><a href="http://health.usf.edu/nocms/pharmacy/">Pharmacy</a></li>
            </ul>
            <h2>More</h2>
            <ul>
                <li><a href="http://health.usf.edu/research/ocr/index.htm">Clinical Research</a></li>
                <li><a href="http://health.usf.edu/research/researchcenters.html">Centers of Excellence</a></li>
                <li><a href="http://hsccf.hsc.usf.edu/researchdirectory/directorysearch/search.cfm">Research Directory</a></li>
                <li><a href="http://ctlsv2.emergingmed.com/Content/Partner/USF_AD/Index.htm" target="_blank">Clinical Studies</a></li>
            </ul>
        </nav>
        <nav>
            <h1>Healthcare</h1>
            <h2>Healthcare</h2>
            <ul>
                <li><a href="http://health.usf.edu/doctors/">Doctors of USF Health</a></li>
            </ul>
            <h2>Service</h2>
            <ul>
                <li><a href="http://health.usf.edu/admin/hr/">Human Resources</a></li>
                <li><a href="http://health.usf.edu/is/home.htm">Information Systems</a></li>
                <li><a href="http://health.usf.edu/pio/index.htm">Professional Integrity</a></li>
                <li><a href="http://health.usf.edu/facultyaffairs/index.htm">Faculty Affairs</a></li>
                <li><a href="http://health.usf.edu/ofm/index.htm">Facilities Management</a></li>
                <li><a href="http://www.paperfreeflorida.org/">PaperFree Florida</a></li>
            </ul>
        </nav>
    </div>
</footer>
<footer id="health-footer">
    <div class="container block-center" id="ctl5">
        <h3>USF Health:</h3>
        <nav>
            <ul>
                <li><a href="http://health.usf.edu">Home</a></li>
                <li><a href="http://health.usf.edu/about.html">About</a></li>
                <li><a href="http://health.usf.edu/news.html">News</a></li>
                <li><a href="http://health.usf.edu/social_media.html">Social Media</a></li>
            </ul>
        </nav>
        <h3>For You:</h3>
        <nav>
            <ul>
                <li><a href="http://health.usf.edu/nocms/myhealthcare/">Patients</a></li>
                <li><a href="http://health.usf.edu/students.html">Students</a></li>
                <li><a href="http://health.usf.edu/faculty_staff.html">Faculty &amp; Staff</a></li>
                <li><a href="http://www.usfhealthalumni.net/">Alumni</a></li>
                <li><a href="http://health.usf.edu/newscenter.html">Media</a></li>
            </ul>
        </nav>
    </div>
</footer>