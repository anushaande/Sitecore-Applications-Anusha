<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/Sublayouts/Doctors/HeaderForSub.ascx.cs" %>

<!-- Sub Page Header -->
<header role="banner" id="health-header">
    <div id="health-header-inner" class="container block-center">
        <nav class="health-links clearfix">
            <ul>
                <li><a href="http://health.usf.edu/news.html">USF Health News</a></li>
            </ul>
         </nav>
        <sc:placeholder ID="ph_USFLinks" Cacheable="true" VaryByData="true" runat="server" key="USFLinks"></sc:placeholder>
    </div>
</header>

<header role="banner" id="dept-header">
    <div class="container block-center">
        <div class="logo_ribbon">
            <div id="logo">
                <a href="http://health.usf.edu" id="health-logo"><img src="/layouts/images/health_logo_gold.png" alt="" width="101" height="68" /></a>
            </div>
        </div>
        <h1 class="doctors-wordmark"><a href="/doctors/"><strong>Doctors</strong> of USF Health</a></h1>
        <h2>
            <sc:placeholder ID="ph_pageTitle" Cacheable="true" VaryByData="true" runat="server" key="pageTitle"></sc:placeholder>
        </h2>
    </div>
</header>
<sc:placeholder ID="ph_DeptNav" runat="server" key="DeptNav"></sc:placeholder>
<div class="container">
    <sc:placeholder ID="ph_Breadcrumbs" Cacheable="true" VaryByData="true" runat="server" key="Breadcrumbs"></sc:placeholder>
</div>