<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="~/Sublayouts/Doctors/HeaderForMain.ascx.cs" %>

<!-- Main Page Header -->
<div id="cloud-header">
    <header role="banner" class="container block-center">
        <h1 class="doctors-wordmark"><a href="/doctors/"><strong>Doctors</strong> of USF Health</a></h1>
        <div class="logo_ribbon">
            <div id="logo">
                <a href="http://health.usf.edu" id="health-logo"><img src="/layouts/images/health_logo_gold.png" alt="" width="101" height="68" /></a>
            </div>
        </div>
        <div id="health-header">
            <div id="health-header-inner">
                <sc:placeholder ID="ph_HealthLinks" Cacheable="true" VaryByData="true" runat="server" key="HealthLinks"></sc:placeholder>
            </div>
        </div>
        <sc:placeholder ID="Placeholder2" runat="server" key="USFLinks"></sc:placeholder>
    </header>
    <div id="intro" class="clearfix">
        <sc:placeholder ID="ph_DoctorHomeSS" Cacheable="true" VaryByData="true" key="DoctorsHomeSS" runat="server"></sc:placeholder>
        <sc:placeholder ID="ph_DoctorHomePT" Cacheable="true" VaryByData="true" key="DoctorsHomePT" runat="server"></sc:placeholder>
    </div>
</div>

<sc:placeholder ID="ph_DeptNav" runat="server" key="DeptNav"></sc:placeholder>