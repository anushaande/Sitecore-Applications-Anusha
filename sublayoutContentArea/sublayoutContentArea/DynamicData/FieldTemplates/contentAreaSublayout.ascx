<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="contentAreaSublayout.ascx.cs" Inherits="HealthIS.Sublayouts.contentAreaSublayout" Debug="true" %>


<asp:MultiView ID="mvContentArea" runat="server" OnActiveViewChanged="mvContentArea_ActiveViewChanged">
    
    <asp:View ID="viewPageEditor" runat="server">
        <div style="padding:1em; border:1px dashed #E3E3E3;">
                <asp:Literal ID="myEditingArea" runat="server" />
        </div>
    </asp:View>
    
    <%--  -------------View Mode ---------------- --%>
    <asp:View ID="viewNormalMode" runat="server">
        <asp:Literal runat="server" ID="myContentArea" />
    </asp:View>
</asp:MultiView>
