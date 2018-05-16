<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Tree.aspx.cs" Inherits="HealthIS.Apps.MVC.TreeDisplay.Tree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnShowTree" runat="server" Text="Generate Tree" OnClick="btnShowTree_Click" /><br />
        <asp:ScriptManager ID="scriptMng" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="udpTree" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <ul>
                    <asp:Literal ID="litTree" runat="server"></asp:Literal>
                    <asp:Panel ID="pnlErr" runat="server" Visible="false">
                        <li>
                            <h2 style="color:red;">Error:</h2>
                            <p>
                                <asp:Literal ID="litErr" runat="server"></asp:Literal>
                            </p>
                        </li>
                    </asp:Panel>
                </ul>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
