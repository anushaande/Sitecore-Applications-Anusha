<%@ Control Language="C#" AutoEventWireup="True" Inherits="Sitecore.VerticalNav.VerticalNavSublayout" CodeBehind="VerticalNav.ascx.cs" debug="false" %>
<style type="text/css">
    .navTabs 
    {
        border:1px solid black;
        display:block;
        padding:8px 10px;
        margin:10px 10px 10px;
        width:150px;
        border-radius: 5px;
        text-align:center;
        font: bold 14px sans-serif;
   }
    div.navLinkContainer {
    background:green; 
    min-height:100px;
    }
    div.editNavLinks {
        border:solid #666 dashed;
        background-color:#E3E3E3;
        padding-left:.5em;
    }
    div.editNavLinks > span {
        font-weight:bold;
    }
    div.editNavLink {
        display:block;
        margin-top:1em;
        margin-left:1em;
    }
    .settings-label {
        font-weight:bold;
    }
</style>

<asp:label runat="server" id="lblError" forecolor="Red">
    </asp:label>
<asp:multiview runat="server" id="mvVerticalNav" onactiveviewchanged="mvVerticalNav_ActiveViewChanged">
	<asp:view id="viewNormal" runat="server">
		<div runat="server" id="vertNavOutDiv" class='<%# MyItem.InnerItem.Fields["css"].Value%>'>
            <asp:Label runat="server" ID="VertNavTitle" Text='<%# MyItem.InnerItem.Fields["NavTitle"].Value %>'></asp:Label>
			<asp:listview id="lvFeedReader" runat="server">
				<itemtemplate>
		            <asp:HyperLink runat="server" CssClass='<%# Eval("Fields[class]") %>' Text='<%# Eval("Fields[Title].Value") %>' NavigateUrl='<%# ProperUrl(Eval("Fields[url].Value").ToString()) %>' Target="_blank" />
                </itemtemplate>
			</asp:listview>
		</div>
	</asp:view>
	<asp:view id="viewPageEditor" runat="server">

		<asp:panel id="pnlFeedConfigurationFields" runat="server">
			<h3>
				Feed Configuration fields for <span style="color:green;"><asp:literal id="configItemName" runat="server"></asp:literal></span></h3>
			<fieldset class="settings-fieldset">
                <legend></legend>

                

				<asp:label id="lblCSSClass" runat="server" text="Vertical Nav CSS Class" associatedcontrolid="cssClass" cssclass="settings-label"></asp:label>
                    <div>
                        <sc:fieldrenderer id="cssClass" runat="server" fieldname="css"></sc:fieldrenderer>
                    </div>
                <asp:label id="lblNavTitle" runat="server" text="Vertical Nav Title" associatedcontrolid="NavTitle" cssclass="settings-label"></asp:label>
                    <div>
                        <sc:fieldrenderer id="NavTitle" runat="server" fieldname="NavTitle"></sc:fieldrenderer>
                    </div>

                <div class="editNavLinks">
                    <span>URL Links ( Title  | URL  | CSS)</span>
               
               <asp:listview id="editLinks" runat="server">
				<itemtemplate>
                    <div class="editNavLink">
                        <sc:EditFrame ID="EditField" runat="server" DataSource="<%# ((Sitecore.Data.Items.Item)Container.DataItem).Paths.FullPath %>" Buttons="/sitecore/content/Applications/WebEdit/Edit Frame Buttons/Nav Link Buttons">
                            <sc:fieldrenderer id="navLinkTitle" runat="server" fieldName="title" item='<%# Container.DataItem %>'></sc:fieldrenderer> |
                            <sc:fieldrenderer id="navLinkURL" runat="server" fieldName="url" item='<%# Container.DataItem %>'></sc:fieldrenderer> |
                            <sc:fieldrenderer id="navLinkCSS" runat="server" fieldName="class" item='<%# Container.DataItem %>'></sc:fieldrenderer>
                        </sc:EditFrame>
                    </div>
                </itemtemplate>
               </asp:listview>
              </div>
	        </fieldset>

		</asp:panel>

	</asp:view>

</asp:multiview>