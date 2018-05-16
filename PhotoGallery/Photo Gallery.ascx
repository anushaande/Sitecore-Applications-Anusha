<%@ register TagPrefix="sc" Namespace="Sitecore.Web.UI.WebControls" Assembly="Sitecore.Kernel" %>
<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="Layouts.Photo_gallery.Photo_gallerySublayout" CodeFile="~/Sublayouts/components/Photo Gallery.ascx.cs" %>

<asp:repeater id="ImageGallery" runat="server">
    <HeaderTemplate>
        <ul class="photo-grid">
    </HeaderTemplate>
    <itemtemplate>
            <li>
                <a class="photo-grid-item"
                    title="<%# Eval("Fields[\"Alt\"]") %>"
                    href="<%# Sitecore.Resources.Media.MediaManager.GetMediaUrl((Sitecore.Data.Items.Item)Container.DataItem) %>"><%# Eval("Fields[\"Alt\"]") %></a>
            </li>
    </itemtemplate>
    <FooterTemplate>
        </ul>
    </FooterTemplate>
</asp:repeater>

<asp:literal runat="server" id="hello"> </asp:literal>