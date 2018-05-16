<%@ Control Language="c#" AutoEventWireup="True"  Inherits="HealthIS.Sublayouts.sublayoutBootStrapSlideShow" CodeBehind="BootstrapSlideShow.ascx.cs" %>


<asp:MultiView ID="mvContentArea" runat="server" OnActiveViewChanged="mvContentArea_ActiveViewChanged">
    
    <asp:View ID="viewPageEditor" runat="server">
        <div style="padding:1em; border:1px dashed #E3E3E3;">
            <asp:Literal ID="myEditingArea" runat="server" />
            <b>ID: </b><asp:Literal ID="editID" runat="server" /><br />
            <%-- <b>Title: </b><asp:Literal ID="editTitle" runat="server" /><br /> --%>
            <b>StyleClass: </b><asp:Literal ID="editStyleClass" runat="server" /><br />
            <b>Transition Time:</b><input type="text" runat="server" id="editTransitionTime" /><br />
            <b>Pause on hover: </b><asp:CheckBox runat="server" id="editPauseOnHover" Enabled="true" AutoPostBack="false"></asp:CheckBox><br />
            <b>Images:</b><br />
            <sc:EditFrame runat="server" ID="ImagesEditingFrame" Buttons="/sitecore/content/Applications/WebEdit/Edit Frame Buttons/Bootstrap SlideShow Insert Image Frame">
                <div style="padding:1em; border:1px solid #000000">
                    <asp:Literal ID="editImages" runat="server" />
                    <asp:ListView ID="editSSImages" runat="server">
                        <ItemTemplate>
                            <div>
                                <sc:EditFrame ID="EditField" runat="server" datasource="<%# ((Sitecore.Data.Items.Item)Container.DataItem).Paths.FullPath %>" buttons="/sitecore/content/Applications/WebEdit/Edit Frame Buttons/BootstrapSlideshow Image Buttons">
                                    <table>
                                        <thead></thead>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <sc:Image runat="server" Item='<%# Container.DataItem %>' field="image" maxHeight="50" maxWidth="50"></sc:Image>
                                                </td>
                                                <td>
                                                    <b>Title: </b><sc:fieldrenderer id="navLinkTitle" runat="server" fieldName="title" item='<%# Container.DataItem %>'></sc:fieldrenderer> <br />
                                                    <b>Description: </b><sc:fieldrenderer id="navLinkURL" runat="server" fieldName="description" item='<%# Container.DataItem %>'></sc:fieldrenderer> <br />
                                                    <b>Link: </b><sc:fieldrenderer id="navLinkCSS" runat="server" fieldName="link" item='<%# Container.DataItem %>'></sc:fieldrenderer> 
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </sc:EditFrame>
                            </div>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </sc:EditFrame>
        </div>
    </asp:View>
    
    <%--  -------------View Mode ---------------- --%>
    <asp:View ID="viewNormalMode" runat="server">
        <asp:Literal runat="server" ID="myContentArea" />
          <div id="<asp:Literal id="carouselID" runat="server"></asp:Literal>" class="carousel slide" data-ride="carousel" data-interval="<asp:Literal id="TransitionTime" runat="server"></asp:Literal>" data-pause="<asp:Literal id="PauseOnHover" runat="server"></asp:Literal>" >
          <!-- Indicators -->
          <ol class="carousel-indicators">
            <asp:Literal ID="listIndicators" runat="server"></asp:Literal>
          </ol>

          <!-- Wrapper for slides -->
          <div class="carousel-inner">
            <asp:Literal ID="imageObjects" runat="server"></asp:Literal>
          </div>

          <!-- Controls -->
          <a class="left carousel-control" href="#<asp:Literal id="carouselID2" runat="server"></asp:Literal>" data-slide="prev">
            <span class="glyphicon glyphicon-chevron-left"></span>
          </a>
          <a class="right carousel-control" href="#<asp:Literal id="carouselID3" runat="server"></asp:Literal>" data-slide="next">
            <span class="glyphicon glyphicon-chevron-right"></span>
          </a>
        </div>
    </asp:View>
</asp:MultiView>
