<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="emailParameter.ascx.cs" Inherits="ContactForm.emailParameter" %>


<asp:MultiView ID="mvContentArea" runat="server" OnActiveViewChanged="mvContentArea_ActiveViewChanged">

    <asp:View ID="viewPageEditor" runat="server">
        <div style="padding: 1em; border: 1px dashed #E3E3E3;">
            <asp:Literal ID="myEditingArea" runat="server" />
        </div>
    </asp:View>

    <asp:View ID="viewNormalMode" runat="server">
        <div class="ContactForm" <% if (!String.IsNullOrEmpty(FormBuilder)) { %> data-builder="<%= FormBuilder %>" <% } %> >
            <asp:Literal runat="server" ID="myContentArea" />
        </div>
    </asp:View>

</asp:MultiView>
