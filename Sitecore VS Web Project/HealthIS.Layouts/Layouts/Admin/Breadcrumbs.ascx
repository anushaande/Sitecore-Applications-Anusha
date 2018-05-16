<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="HealthIS.Layouts.BreadcrumbsSublayout" Codebehind="Breadcrumbs.ascx.cs" %>

<% if (CrumbList.Count > 1) { %>
    <nav class="crumbtacular three-deep">
        <ul>            
            <li><a href="/"><i class="icon-home"></i><span class="sr-only">USF Health Home</span></a></li>
            <li><i class="icon-caret-right"></i><a href="/<%= CrumbList[0].Name %>"><%= CrumbList[0].DisplayName %></a></li>
            <li><i class="icon-caret-right"></i><a href="/<%= CrumbList[0].Name %>/<%= CrumbList[1].Name %>"><%= CrumbList[1].DisplayName %></a></li>
        </ul>
    </nav>
<% } else { %>
    <nav class="crumbtacular">
        <ul>
            <li><a href="/"><i class="icon-home"></i>USF Home</a></li>
            <% if (!String.IsNullOrEmpty(CrumbList[0].DisplayName)) { %>
                <li><i class="icon-caret-right"></i><a href="/<%= CrumbList[0].Name %>"><%= CrumbList[0].DisplayName %></a></li>
            <% } %>
        </ul>
    </nav>
<% } %>