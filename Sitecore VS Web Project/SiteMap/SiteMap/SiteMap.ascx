<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SiteMap.ascx.cs" Inherits="SiteMap.SiteMap" %>
<div class="table-wrap">

    <asp:Label runat="server" ID="error" />

    <table class="confluenceTable tablesorter">
        <thead>
            <tr class="sortableHeader">
                <th class="confluenceTh sortableHeader" data-column="0">
                    <div class="tablesorter-header-inner">Display Name</div>
                </th>
                <th class="confluenceTh sortableHeader" data-column="1">
                    <div class="tablesorter-header-inner">URL</div>
                </th>
                <th colspan="1" class="confluenceTh sortableHeader" data-column="2">
                    <div class="tablesorter-header-inner">Page Type</div>
                </th>
                <th class="confluenceTh sortableHeader" data-column="3">
                    <div class="tablesorter-header-inner">Components</div>
                </th>
                <th class="confluenceTh sortableHeader" data-column="4">
                    <div class="tablesorter-header-inner">Notes</div>
                </th>
                <th class="confluenceTh sortableHeader" data-column="5">
                    <div class="tablesorter-header-inner">sitecore</div>
                </th>
                <th class="confluenceTh sortableHeader" data-column="6">
                    <div class="tablesorter-header-inner">old cms</div>
                </th>
            </tr>
        </thead>
        <tbody>
            <asp:Label runat="server" ID="lists" />
        </tbody>
    </table>
</div>
