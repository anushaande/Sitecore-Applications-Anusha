<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DiagnosticLab_ContactUs.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.Doctors.DiagnosticLab_ContactUs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="/layouts/styles/health.css"/>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <script type="text/javascript" src="/resources/scripts/HealthIS.Apps.Util.js"></script>
    <style>
    .form-control-invalid {
        border-color:red;
    }
    </style>
</head>
<body class="form-body">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlFormStart" runat="server">
                    <div class="RegistrationForm">
                        <div class="form-group">
                            <asp:Label ID="lblName" AssociatedControlID="tb_Name" runat="server" Text="Name" /><br />
                            <asp:TextBox ID="tb_Name" CssClass="form-control" runat="server" placeholder="Name *" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblEmail" AssociatedControlID="tb_Email" runat="server" Text="Email" /><br />
                            <asp:TextBox ID="tb_Email" runat="server" CssClass="form-control" placeholder="Email *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Email" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Email" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblPhone" AssociatedControlID="tb_Phone" runat="server" Text="Phone" /><br />
                            <asp:TextBox ID="tb_Phone" CssClass="form-control" runat="server" placeholder="Phone *" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_Phone" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tb_Phone" runat="server" ErrorMessage="" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group">
                        <asp:Label ID="lblQuestion" AssociatedControlID="tb_Question" runat="server" Text="Question" /><br />
                            <asp:TextBox ID="tb_Question" CssClass="form-control" TextMode="MultiLine" Rows="5" runat="server" placeholder="Question *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_Question" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-reset btn btn-usf-health bg-usf-rhubarb" Text="Reset" OnClick="btnReset_Click" CausesValidation="false" />
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn btn-usf-health bg-usf-silvergray" Text="Submit" OnClick="btnSubmit_Click" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFormComplete" Visible="false" runat="server">
                    <p>Thank you, your message has been sent.</p>
                    <asp:Literal ID="litComplete" runat="server"></asp:Literal>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="/resources/forms/scripts/HealthIS.Apps.Forms.js"></script>
</body>
</html>