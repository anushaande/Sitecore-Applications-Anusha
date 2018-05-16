<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RSS Reader.ascx.cs" Inherits="Sitecore.RSS.RSS_Reader" %>
<style type="text/css">
	.settings-fieldset
	{
		border: none;
	}
	.settings-label
	{
		float: left;
		font-weight: bold;
	}
	.settings-field
	{
		float: none;
		clear: both;
	}
	.feed
	{
		padding: 15px 0 15px 0;
	}
	.feed-item
	{
		margin-top: 10px;
		padding-top: 10px;
		border-top: solid 1px gray;
	}
	.feed-title
	{
		display: block;
		font-weight: bold;
		color: Black;
	}
	.feed-item-title
	{
		font-weight: bold;
	}
	.feed-item-publish-date
	{
		font-style: italic;
		color: Gray;
	}
	.feed-item-description
	{
		display: block;
		padding: 3px;
	}
</style>
<asp:Label ID="lblError" runat="server" ForeColor="Red" />
<asp:MultiView ID="mvRSSReader" runat="server" OnActiveViewChanged="mvRSSReader_ActiveViewChanged">
	<asp:View ID="viewNormal" runat="server">
		<div class='<%# FeedSetting.FeedCSSClass %>'>
			<asp:ListView ID="lvFeedReader" runat="server">
				<LayoutTemplate>
					<asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
				</LayoutTemplate>
				<ItemTemplate>
					<asp:HyperLink ID="hlnkFeedTitle" runat="server" CssClass="<%# FeedSetting.FeedTitleCSSClass %>" Text='<%# Eval("feedTitle") %>' NavigateUrl='<%# Eval("feedLink") %>' />
					<asp:Label ID="lblFeedDescription" runat="server" CssClass='<%#FeedSetting.FeedDescriptionCSSClass%>' Text='<%# Eval("feedDescription") %>' />
					<asp:ListView ID="lvFeedItems" runat="server" DataSource='<%# Eval("feedItems") %>'>
						<LayoutTemplate>
							<asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
						</LayoutTemplate>
						<ItemTemplate>
							<div id="divFeedItem" runat="server" class='<%#FeedSetting.FeedItemCSSClass %>'>
								<asp:HyperLink ID="hlnkTitle" runat="server" CssClass='<%#FeedSetting.FeedItemTitleCSSClass%>' Text='<%# xEval(Eval("XML"), "/item/title") %>' NavigateUrl='<%# xEval(Eval("XML"), "/item/link") %>' />
								<asp:Panel ID="pnlFeedBody" runat="server">
									<asp:Label ID="lblFeedPublishDate" CssClass="<%# FeedSetting.FeedItemPublishDateCSSClass %>" runat="server" Text='<%# xEval(Eval("XML"), "/item/pubDate") %>' />
									<asp:Label ID="lblFeedDesctiption" CssClass="<%# FeedSetting.FeedItemDescriptionCSSClass %>" runat="server" Text='<%# xEval(Eval("XML"), "/item/description") %>' />
								</asp:Panel>
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
				<li>Close Edit mode</li>
				<li>Open Design mode</li>
				<li>Find RSS Reader sublayout control and select to edit its properties</li>
				<li>Point Data Source property to a different Feed configuration item</li>
			</ol>
		</p>
		<asp:Panel ID="pnlFeedConfigurationFields" runat="server">
			<h3>
				Feed Configuration fields</h3>
			<fieldset class="settings-fieldset">
				<asp:Label ID="lblFeedUrl" runat="server" Text="Feed URL" AssociatedControlID="fldFeedUrl" CssClass="settings-label" />
				<div class="settings-field">
					<sc:FieldRenderer ID="fldFeedUrl" runat="server" FieldName="feed url" />
				</div>
				<br />
				<asp:Label ID="lblFeedCSSClass" runat="server" Text="Feed CSS Class" AssociatedControlID="fldFeedCSSClass" CssClass="settings-label" />
				<div class="settings-field">
					<sc:FieldRenderer ID="fldFeedCSSClass" runat="server" FieldName="feed css class" />
				</div>
				<br />
				<asp:Label ID="lblFeedItemCSSClass" runat="server" Text="Feed Item CSS Class" AssociatedControlID="fldFeedItemCSSClass" CssClass="settings-label" />
				<div class="settings-field">
					<sc:FieldRenderer ID="fldFeedItemCSSClass" runat="server" FieldName="feed item css class" />
				</div>
			</fieldset>
		</asp:Panel>
	</asp:View>
</asp:MultiView>
