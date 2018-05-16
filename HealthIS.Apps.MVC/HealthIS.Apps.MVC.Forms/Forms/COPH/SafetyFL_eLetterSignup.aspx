<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SafetyFL_eLetterSignup.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.SafetyFL_eLetterSignup" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    <form id="form1" runat="server" defaultbutton="btnSubmit">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlFormStart" runat="server">
                    <div class="safety-fl-eletter">                      
                        <p>Please fill out this form to subscribe to the eLetter.</p>
                        <p class="form-group">
                            <asp:Label ID="lblName" AssociatedControlID="tb_Name" runat="server" Text="Name *" /><br />
                            <asp:TextBox ID="tb_Name" CssClass="form-control" runat="server" placeholder="Name" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>                        
                        <p class="form-group">
                            <asp:Label ID="lblEmail_Address" AssociatedControlID="tb_Email_Address" runat="server" Text="Email Address *" /><br />
                            <asp:TextBox ID="tb_Email_Address" runat="server" CssClass="form-control" placeholder="someone@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </p>                                              
                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-reset btn btn-usf-health bg-usf-rhubarb" Text="Reset" OnClick="btnReset_Click" CausesValidation="false" />
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn btn-usf-health bg-usf-silvergray" Text="Subscribe" OnClick="btnSubmit_Click" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFormComplete" Visible="false" runat="server">
                    <p>Thank you, your subscription has been sent.</p>
                    <asp:Literal ID="litComplete" runat="server"></asp:Literal>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="/resources/forms/scripts/HealthIS.Apps.Forms.js"></script>
</body>
</html>
