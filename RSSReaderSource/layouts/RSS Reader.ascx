<%@ Control Language="C#" AutoEventWireup="true" Inherits="Sitecore.RSS.RSS_Reader" Debug="true" CodeBehind="RSS Reader.ascx.cs" %>
<%if (editMode)
  {%>
<link rel="stylesheet" type="text/css" href="/Sublayouts/components/styles/hitEditing.css" id="style" visible="false" />
<%} %>

<asp:Label ID="lblError" runat="server" ForeColor="Red" />
<asp:MultiView ID="mvRSSReader" runat="server" OnActiveViewChanged="mvRSSReader_ActiveViewChanged">
    <asp:View ID="viewNormal" runat="server">
        <div class='<%# FeedSetting.FeedCSSClass %>'>
            <asp:ListView ID="lvFeedReader" runat="server">
                <LayoutTemplate>
                    <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                </LayoutTemplate>
                <ItemTemplate>


                    <% if (FeedSetting.DisplayFeedTitle) %>
                    <%{ %>
                        <% if (!string.IsNullOrEmpty(FeedSetting.InnerItem.Fields["Feed Title"].ToString()))%>
                        <%{ %>
                        <asp:HyperLink ID="hlnkFeedTitle1" runat="server" CssClass="<%# FeedSetting.FeedTitleCSSClass %>" Text='<%# FeedSetting.InnerItem.Fields["Feed Title"].ToString() %>' NavigateUrl='<%# Eval("feedLink") %>' />
                        <%} %>
                        <%else { %>
                        <asp:HyperLink ID="hlnkFeedTitle" runat="server" CssClass="<%# FeedSetting.FeedTitleCSSClass %>" Text='<%#Eval("feedTitle") %>' NavigateUrl='<%# Eval("feedLink") %>' />
                        <%} %>
                    <% } %>

                     <% if (FeedSetting.DisplayFeedDesc) %>
                        <%{ %>
                           <asp:Label ID="lblFeedDescription" runat="server" CssClass='<%#FeedSetting.FeedDescriptionCSSClass%>' Text='<%# Eval("feedDescription") %>' />
                        <% } %>

                    <asp:ListView ID="lvFeedItems" runat="server" DataSource='<%# Eval("feedItems") %>'>
                        <LayoutTemplate>
                            <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                        </LayoutTemplate>
                        <ItemTemplate>
                            <div id="divFeedItem" runat="server" class='<%#FeedSetting.FeedItemCSSClass %>'>
                                <% if (FeedSetting.DisplayTitleFirst) %>
                                <%{ %>

                                <asp:HyperLink ID="hlnkTitle" runat="server" CssClass='<%#FeedSetting.FeedItemTitleCSSClass%>' Text='<%# xEval(SafeEval(Container.DataItem, "XML"), "/item/title") %>' NavigateUrl='<%# xEval(SafeEval(Container.DataItem, "XML"), "/item/link") %>' />
                                <asp:Panel ID="pnlFeedBody" runat="server">

                                    <% if (FeedSetting.NumberOfImages > 0)
                                       { 
                                    %>
                                    <asp:Literal ID="imgPlaceHolder" runat="server" Text='<%# outputImages(SafeEval(Container.DataItem, "XML"), "/item/content:encoded") %>'></asp:Literal>
                                    <% } %>
                                    <asp:Label ID="lblFeedPublishDate" CssClass="<%# FeedSetting.FeedItemPublishDateCSSClass %>" runat="server" Text='<%# xEval(SafeEval(Container.DataItem, "XML"), "/item/pubDate") %>' />
                                    <asp:Label ID="lblFeedDesctiption" CssClass="<%# FeedSetting.FeedItemDescriptionCSSClass %>" runat="server" Text='<%# getFeedDescription(SafeEval(Container.DataItem, "XML"), "/item/description") %>' />
                                </asp:Panel>
                                <% }
                                   else
                                   { %>
                                <% if (FeedSetting.NumberOfImages > 0)
                                   { %>
                                <div class="rssfeed-thumbnail">
                                    <asp:Literal ID="imgPlaceHolder1" runat="server" Text='<%# outputImages(SafeEval(Container.DataItem, "XML"), "/item/content:encoded") %>'></asp:Literal>
                                </div>
                                <% } %>
                                <asp:HyperLink ID="hlnkTitle1" runat="server" CssClass='<%#FeedSetting.FeedItemTitleCSSClass%>' Text='<%# xEval(SafeEval(Container.DataItem, "XML"), "/item/title") %>' NavigateUrl='<%# xEval(SafeEval(Container.DataItem, "XML"), "/item/link") %>' />
                                <asp:Panel ID="pnlFeedBody1" runat="server">
                                    <asp:Label ID="lblFeedPublishDate1" CssClass="<%# FeedSetting.FeedItemPublishDateCSSClass %>" runat="server" Text='<%# xEval(SafeEval(Container.DataItem, "XML"), "/item/pubDate") %>' />
                                    <asp:Label ID="lblFeedDesctiption1" CssClass="<%# FeedSetting.FeedItemDescriptionCSSClass %>" runat="server" Text='<%# getFeedDescription(SafeEval(Container.DataItem, "XML"), "/item/description").Replace("[&#8230\u003B]", "...[<a href=\"" + xEval(SafeEval(Container.DataItem, "XML"), "/item/link") + "\">read more</a>]") %>' />
                                </asp:Panel>
                                <% } %>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </asp:View>
    <asp:View ID="viewPageEditor" runat="server">
        <p style="color: Red;">
            To choose a different feed configuration item, do the following:<br />
            <ol>
                <li>Go to layout properties for the page</li>
                <li>Find RSS Reader sublayout control and select to edit its properties</li>
                <li>Point Data Source property to a different Feed configuration item in content tree</li>
            </ol>
        </p>
        <asp:Panel ID="pnlFeedConfigurationFields" runat="server">
            <h3>Feed Configuration fields for <span style="color: green;">
                <asp:Literal ID="configItemName" runat="server"></asp:Literal></span></h3>
            <table>
                <tr>
                    <td>
                        <asp:Label ID="lblFeedUrl" runat="server" Text="Feed URL" AssociatedControlID="fldFeedUrl" CssClass="settings-label" /></td>
                    <td>
                        <sc:FieldRenderer ID="fldFeedUrl" runat="server" FieldName="Feed URL" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFeedTitle" runat="server" Text="Feed Title" AssociatedControlID="fldFeedTitle" CssClass="settings-label" /></td>
                    <td>
                        <sc:FieldRenderer ID="fldFeedTitle" runat="server" FieldName="Feed Title" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFeedCSSClass" runat="server" Text="Feed CSS Class" AssociatedControlID="fldFeedCSSClass" CssClass="settings-label" /></td>
                    <td>
                        <sc:FieldRenderer ID="fldFeedCSSClass" runat="server" FieldName="feed css class" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblFeedItemCSSClass" runat="server" Text="Feed Item CSS Class" AssociatedControlID="fldFeedItemCSSClass" CssClass="settings-label" /></td>
                    <td>
                        <sc:FieldRenderer ID="fldFeedItemCSSClass" runat="server" FieldName="feed item css class" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lblShowFeedTitle" runat="server" Text="Show Feed Title" CssClass="settings-label" />
                    </td>
                    <td>
                        <asp:CheckBox ID="cbShowFeedTitle" runat="server" Enabled="true" AutoPostBack="false"></asp:CheckBox>
                    </td>
                </tr>
                
                <tr>
                    <td>
                        <asp:Label ID="lblShowFeedDesc" runat="server" Text="Show Feed Description" CssClass="settings-label" />
                    </td>
                    <td>
                        <asp:CheckBox ID="cbShowFeedDesc" runat="server" Enabled="true" AutoPostBack="false"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblShowTitleFirst" runat="server" Text="Show Title First" CssClass="settings-label" /></td>
                    <td>
                        <asp:CheckBox ID="cbShowTitleFirst" runat="server" Enabled="true" AutoPostBack="false"></asp:CheckBox></td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblNumberOfItems" runat="server" Text="Number of Items to Show" AssociatedControlID="fldNumberOfItems" CssClass="settings-label" /></td>
                    <td>
                        <sc:FieldRenderer ID="fldNumberOfItems" runat="server" FieldName="Number of Items" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <asp:Label ID="lblNumberOfImages" runat="server" Text="Number of Images to show per Item" AssociatedControlID="fldNumberOfImages" CssClass="settings-label" /></td>
                    <td>
                        <sc:FieldRenderer ID="fldNumberOfImages" runat="server" FieldName="Number of Images" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblDescriptionCharLimit" runat="server" Text="Feed Item Description Character Limit" AssociatedControlID="fldItemDescriptionCharLimit" CssClass="settings-label" /></td>
                    <td>
                        <sc:FieldRenderer ID="fldItemDescriptionCharLimit" runat="server" FieldName="Item Description Character Limit" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </asp:View>
</asp:MultiView>