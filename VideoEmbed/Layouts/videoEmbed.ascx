<%@ Control Language="c#" AutoEventWireup="True" Inherits="Sitecore.videoEmbed.VideoEmbedSublayout" CodeBehind="videoEmbed.ascx.cs" debug="false"%>

<asp:label runat="server" id="lblError" forecolor="Red" />
<asp:multiview runat="server" id="mvVideoEmbed" onactiveviewchanged="mvVideoEmbed_ActiveViewChanged">
	<asp:view id="viewNormal" runat="server">
        
            <span class="vidEmbed"><asp:Label runat="server" ID="VideoEmbedTitle"></asp:Label></span>
		    <a ID="VideoUrl" runat="server">
            <img id="videoImg" runat="server" class="vidEmbed"></img>
            </a>
	
	</asp:view>
	<asp:view id="viewPageEditor" runat="server">

		<asp:panel id="pnlFeedConfigurationFields" runat="server">
			<h3>
				Configuration fields for <span style="color:green;"><asp:literal id="configItemName" runat="server"></asp:literal></span></h3>
            
			<fieldset class="settings-fieldset">
               <legend></legend>
                <div class="editHeader">
                    <span>
                        <asp:label id="lblVidTitle" runat="server" text="Video Title" cssclass="settings-label"></asp:label> | 
                        <asp:label id="lblVidLightboxTitle" runat="server" text="Video Lightbox Title" cssclass="settings-label"></asp:label> | 
                        <asp:label id="lblVidUrl" runat="server" text="Video URL"  cssclass="settings-label"></asp:label> | 
				        <asp:label id="lblCSSClass" runat="server" text="Video Container CSS Class" cssclass="settings-label"></asp:label>
                    </span>
                </div>
                <div class="editLink">
                    <sc:fieldrenderer id="vidTitle" runat="server" fieldname="Title"></sc:fieldrenderer> | 
                    <sc:fieldrenderer id="vidLightboxTitle" runat="server" fieldname="LightboxTitle"></sc:fieldrenderer> | 
                    <sc:fieldrenderer id="vidURL" runat="server" fieldname="URL"></sc:fieldrenderer> | 
                    <sc:fieldrenderer id="vidcssClass" runat="server" fieldname="css"></sc:fieldrenderer>
                </div>
             
	        </fieldset>

		</asp:panel>

	</asp:view>

</asp:multiview>