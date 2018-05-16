<%@ Control Language="c#" AutoEventWireup="true" Inherits="Layouts.Accordion.AccordionSublayout" CodeBehind="Accordion.ascx.cs" %>
<asp:MultiView runat="server" ID="mvContentArea" OnActiveViewChanged="mvContentArea_ActiveViewChanged">
    <asp:View ID="viewPageEditor" runat="server">
        <div style="padding: 1em; border: 1px dashed #E3E3E3;">
            <b>Accordion ID: </b>
            <asp:Literal ID="editaccID" runat="server"></asp:Literal><br />
            <b>Accordion Class: </b>
            <asp:Literal ID="editaccClass" runat="server"></asp:Literal><br />
            <b>Items:</b><br />
            <sc:EditFrame runat="server" ID="LinkItemEditingFrame">
                <div style="padding: 1em; border: 1px solid #000000">
                <asp:ListView ID="editAccordionItms" runat="server">
                    <ItemTemplate>
                        <div>
                            <sc:EditFrame ID="EditLinkField" runat="server" DataSource="<%# ((Sitecore.Data.Items.Item)Container.DataItem).Paths.FullPath %>" Buttons="/sitecore/content/Applications/WebEdit/Edit Frame Buttons/BootstrapSlideshow Image Buttons">

                                <table>
                                    <tbody>
                                        <tr>
                                            <td><b>Heading: </b></td>
                                            <td>
                                                <sc:FieldRenderer ID="accItemHeader" runat="server" FieldName="Header" Item="<%# Container.DataItem %>"></sc:FieldRenderer>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><b>Div ID: </b></td>
                                            <td>
                                                <sc:FieldRenderer ID="accItemDivID" runat="server" FieldName="Div id" Item="<%# Container.DataItem %>"></sc:FieldRenderer>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td><b>Content: </b></td>
                                            <td>
                                                <sc:FieldRenderer ID="accItemContent" runat="server" FieldName="Content" Item="<%# Container.DataItem %>"></sc:FieldRenderer>
                                            </td>
                                        </tr>
                                        
                                    </tbody>
                                </table>
                                <br />
                            </sc:EditFrame>
                        </div>
                    </ItemTemplate>
                </asp:ListView>
              </div>
            </sc:EditFrame>
        </div>
    </asp:View>

    <asp:View ID="viewNormalMode" runat="server">
        <div class='panel-group <% Response.Write(myDataSourceItem.Fields["Class"]); %>' id='<% Response.Write(myDataSourceItem.Fields["ID"]); %>'>
            <asp:Literal ID="accordianItems" runat="server"></asp:Literal>
        </div>
    </asp:View>
</asp:MultiView>
