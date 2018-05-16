<%@ Control Language="c#" AutoEventWireup="True" Inherits="HealthIS.Sublayouts.BootStrapPhotoGallery" CodeBehind="BootstrapPhotoGallery.ascx.cs" %><asp:multiview runat="server" id="mvContentArea" onactiveviewchanged="mvContentArea_ActiveViewChanged">

    <asp:view id="viewPageEditor" runat="server">
        <div style="padding: 1em; border: 1px dashed #E3E3E3;">
            <asp:literal id="myEditingArea" runat="server"></asp:literal>
            <b>ID: </b>
            <asp:literal id="editID" runat="server"></asp:literal><br />
            <b>StyleClass: </b>
            <asp:literal id="editStyleClass" runat="server"></asp:literal><br />
            <b>Images:</b><br />

            <sc:editframe runat="server" id="ImagesEditingFrame" buttons="/sitecore/content/Applications/WebEdit/Edit Frame Buttons/Bootstrap SlideShow Insert Image Frame">
                <div style="padding: 1em; border: 1px solid #000000">
                    <asp:literal id="editImages" runat="server"></asp:literal>
                    <asp:listview id="editPGImages" runat="server">
                        <itemtemplate>
                            <div>
                                <sc:editframe id="EditField" runat="server" datasource="&lt;%# ((Sitecore.Data.Items.Item)Container.DataItem).Paths.FullPath %&gt;" buttons="/sitecore/content/Applications/WebEdit/Edit Frame Buttons/BootstrapSlideshow Image Buttons">
                                    <table>
                                        <thead></thead>
                                        <tbody>
                                            <tr>
                                                <td>
                                                    <sc:image runat="server" item="&lt;%# Container.DataItem %&gt;" field="image" maxheight="50" maxwidth="50"></sc:image>
                                                </td>
                                                <td>
                                                    <b>Title: </b>
                                                    <sc:fieldrenderer id="navLinkTitle" runat="server" fieldname="title" item="&lt;%# Container.DataItem %&gt;"></sc:fieldrenderer>
                                                    <br />
                                                </td>
                                            </tr>
                                        </tbody>
                                    </table>
                                </sc:editframe>
                            </div>
                        </itemtemplate>
                    </asp:listview>
                </div>
            </sc:editframe>

        </div>
    </asp:view>

    &lt;%--  -------------View Mode ---------------- --%&gt;
    <asp:view id="viewNormalMode" runat="server">

        <asp:literal runat="server" id="myContentArea"></asp:literal>
        <ul class="photo-grid">
            <asp:literal id="imageObjects" runat="server"></asp:literal>
        </ul>

    </asp:view>

</asp:multiview>