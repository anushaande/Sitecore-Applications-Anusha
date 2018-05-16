<%@ Page language="c#" Codepage="65001" AutoEventWireup="true" Inherits="HealthIS.Layouts.MasterLayout" Codebehind="Master.aspx.cs" %>
<%@ OutputCache Location="None" VaryByParam="none" %>
<!DOCTYPE html>
<html lang="en-us" class="no-js">
    <head runat="server">
        <meta charset="utf-8" />
        <meta name="viewport" content="width=device-width, initial-scale=1" />

        <!-- BEGIN_REMOVE|htmlTitle  -->
        <title></title>
        <!-- END_REMOVE|htmlTitle  -->

        <asp:Placeholder ID="Placeholder1" runat="server">

        <!-- BEGIN_REMOVE|metaTags  -->
        <meta name="CODE_LANGUAGE" content="C#" />
        <meta name="vs_defaultClientScript" content="JavaScript" />
        <meta name="vs_targetSchema" content="http://schemas.microsoft.com/intellisense/ie5" />
        <%= MetaKeywords %>
        <%= MetaDescription %>
        <!-- END_REMOVE|metaTags  -->        

        <link rel="stylesheet" href="/layouts/styles/health.css">
        <link media="print" rel="stylesheet" href="/layouts/styles/print.css">

        <%= CustomCssStyle %>
        <%= TemplateStyles %>
        <%= CollegeStyles %>
        <%= DeptStyles %>
        <%= PageStyles %>

        <!-- BEGIN_REPLACE|htmlHeadContent -->
        <!-- END_REPLACE|htmlHeadContent -->

        <script src="/layouts/scripts/modernizr.js"></script>
        <!--[if lt IE 9]>
        <script type="text/javascript" src="/layouts/scripts/respond.min.js"></script>
        <![endif]-->

        <!-- BEGIN_REPLACE|AnalyticsForServer -->
        <sc:VisitorIdentification runat="server" />

        <% if (HealthIS.Apps.Util.isOnProduction() && sw.ContainsKey(deptName)) { %>
            <script type="text/javascript">
                var loc = (("https:" == document.location.protocol) ? "https://analytics." : "http://analytics.")   ;
                document.write(unescape("%3Cscript src='" + loc + "sitewit.com/sw.js'   type='text/javascript'%3E%3C/script%3E"));
            </script>
    
            <script type="text/javascript">
                var sw = new _sw_analytics();
                sw.id = '<%= sw[deptName] %>';
                sw.register_page_view();
            </script>

        <% } %>

        <% if (HealthIS.Apps.Util.isOnProduction()) { %>
            <script>
                (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
                (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
                m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
                })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

                ga('create', 'UA-4198073-1', 'auto');
                ga('require', 'displayfeatures');
                ga('send', 'pageview');
            </script>
        <% } %>
        </asp:Placeholder>
        <!-- END_REPLACE|AnalyticsForServer -->
    </head>
    <body>
        <!-- BEGIN_REPLACE|GoogleTagManager -->
        <noscript><iframe src="//www.googletagmanager.com/ns.html?id=GTM-PK7P" height="0" width="0" style="display:none;visibility:hidden"></iframe></noscript>
        <script>(function(w,d,s,l,i){w[l]=w[l]||[];w[l].push({'gtm.start': new Date().getTime(),event:'gtm.js'});var f=d.getElementsByTagName(s)[0], j=d.createElement(s),dl=l!='dataLayer'?'&l='+l:'';j.async=true;j.src= '//www.googletagmanager.com/gtm.js?id='+i+dl;f.parentNode.insertBefore(j,f); })(window,document,'script','dataLayer','GTM-PK7P');</script>
        <!-- END_REPLACE|GoogleTagManager -->

        <!-- BEGIN_REMOVE|cmsAspNetFormStart -->
        <form method="post" runat="server" id="mainform">
        <!-- END_REMOVE|cmsAspNetFormStart -->

            <sc:placeholder runat="server" key="Template"></sc:placeholder>   

            <!-- BEGIN_REPLACE|jquery -->
            <script src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
            <script>window.jQuery || document.write('<script src="/layouts/scripts/jquery.min.js"><\/script>')</script>
            <!-- END_REPLACE|jquery -->
            <script src="//netdna.bootstrapcdn.com/bootstrap/3.0.3/js/bootstrap.min.js"></script>
            <script src="/layouts/scripts/template.min.js?v=1"></script>

            <!-- BEGIN_REPLACE|customJS  -->
            <script src="/layouts/scripts/plugins.min.js?v=5"></script>

            <asp:Placeholder runat="server">
            <% if (!Sitecore.Context.PageMode.IsPageEditor) { %>
                <%= TemplateScript %>
                <%= CollegeScript %>
                <%= DeptScript %>
                <%= PageScript %>
            <% } %>
            </asp:Placeholder>

            <asp:PlaceHolder ID="JsPlaceHolder" runat="server"></asp:PlaceHolder>

            <!-- END_REPLACE|customJS -->

        <!-- BEGIN_REMOVE|cmsAspNetFormEnd -->
        </form>
        <!-- END_REMOVE|cmsAspNetFormEnd -->
    </body>
</html>
