<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="FPQC_RegisterForm.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.COPH.FPQC_RegisterForm" %>

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
                            <asp:Label ID="lblFirst_Name" AssociatedControlID="tb_First_Name" runat="server" Text="First Name" /><br />
                            <asp:TextBox ID="tb_First_Name" CssClass="form-control" runat="server" placeholder="First Name *" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_First_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblLast_Name" AssociatedControlID="tb_Last_Name" runat="server" Text="Last Name" /><br />
                            <asp:TextBox ID="tb_Last_Name" CssClass="form-control" runat="server" placeholder="Last Name *" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Last_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblSpecialty" AssociatedControlID="tb_Specialty" runat="server" Text="Discipline/Specialty" /><br />
                            <asp:TextBox ID="tb_Specialty" CssClass="form-control" runat="server" placeholder="Discipline/Specialty *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_Specialty" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblEmail_Address" AssociatedControlID="tb_Email_Address" runat="server" Text="Email Address" /><br />
                            <asp:TextBox ID="tb_Email_Address" runat="server" CssClass="form-control" placeholder="Email Address *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblOrganization" AssociatedControlID="tb_Organization" runat="server" Text="Hospital/Organization" /><br />
                            <asp:TextBox ID="tb_Organization" CssClass="form-control" runat="server" placeholder="Hospital/Organization *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tb_Organization" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:CheckBoxList ID="chb_Signup" runat="server" CssClass="checkbox">
                                <asp:ListItem Text="Yes, I would like to receive future news, updates and information from the FPQC." Selected="Yes"></asp:ListItem>                              
                            </asp:CheckBoxList>
                        </div>
                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-reset btn btn-usf-health bg-usf-rhubarb" Text="Reset" OnClick="btnReset_Click" CausesValidation="false" />
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn btn-usf-health bg-usf-silvergray" Text="Submit" OnClick="btnSubmit_Click" />
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFormComplete" Visible="false" runat="server">
                    <p>Thank you, your registration has been sent.</p>
                    <asp:Literal ID="litComplete" runat="server"></asp:Literal>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="/resources/forms/scripts/HealthIS.Apps.Forms.js"></script>
</body>
</html>
