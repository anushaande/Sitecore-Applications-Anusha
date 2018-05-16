<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AwardOfExcellence.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.Forms.AwardOfExcellence" %>

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
                <asp:Panel CssClass="ContactForm" ID="pnlFormStart" runat="server">
                    <div class="aoe-form">
                        <p class="form-group">
                            <asp:Label ID="lblContact_Name" AssociatedControlID="tb_Contact_Name" runat="server" Text="Contact Name" /><br />
                            <asp:TextBox ID="tb_Contact_Name" runat="server" CssClass="form-control" placeholder="Contact Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_Contact_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblCompany_Name" AssociatedControlID="tb_Company_Name" runat="server" Text="Company Name" /><br />
                            <asp:TextBox ID="tb_Company_Name" runat="server" CssClass="form-control" placeholder="Company Name"></asp:TextBox>                            
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblAddress" AssociatedControlID="tb_Address" runat="server" Text="Address" /><br />
                            <asp:TextBox ID="tb_Address" runat="server" CssClass="form-control" placeholder="Address"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblAddress_Line_2" AssociatedControlID="tb_Address_Line_2" runat="server" Text="Address Line 2" /><br />
                            <asp:TextBox ID="tb_Address_Line_2" runat="server" CssClass="form-control" placeholder="Address Line 2"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblCity" AssociatedControlID="tb_City" runat="server" Text="City" /><br />
                            <asp:TextBox ID="tb_City" runat="server" CssClass="form-control" placeholder="City"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_City" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblState" AssociatedControlID="tb_State" runat="server" Text="State" /><br />
                            <asp:TextBox ID="tb_State" runat="server" CssClass="form-control" placeholder="State"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_State" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lbl_Zip_Code" AssociatedControlID="tb_Zip_Code" runat="server" Text="Zip Code" /><br />
                            <asp:TextBox ID="tb_Zip_Code" runat="server" CssClass="form-control" placeholder="Zip Code"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tb_Zip_Code" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblPhone_Number" AssociatedControlID="tb_Phone_Number" runat="server" Text="Phone" /><br />
                            <asp:TextBox ID="tb_Phone_Number" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="tb_Phone_Number" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Phone_Number" runat="server" ErrorMessage="" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                        </p>
                         <p class="form-group">
                            <asp:Label ID="lblFax" AssociatedControlID="tb_Fax" runat="server" Text="Fax" /><br />
                            <asp:TextBox ID="tb_Fax" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblEmail_Address" AssociatedControlID="tb_Email_Address" runat="server" Text="Email" /><br />
                            <asp:TextBox ID="tb_Email_Address" runat="server" CssClass="form-control" placeholder="someone@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblBusiness" AssociatedControlID="rbl_Business" runat="server" Text="Please select your business:" />
                            <asp:RadioButtonList ID="rbl_Business" runat="server">
                                <asp:ListItem Text="Home Health" Value="Home Health"></asp:ListItem>
                                <asp:ListItem Text="Long Term Care" Value="Long Term Care"></asp:ListItem>
                            </asp:RadioButtonList>            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="rbl_Business" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator> 
                        </p>
                        <input id="btnReset" class="btn btn-reset btn-usf-health bg-usf-rhubarb" onclick="location.reload(true)" type="button" value="Reset"/>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn-usf-health bg-usf-silvergray" Text="Submit" OnClick="btnSubmit_Click" />
                        <p class="error"><asp:Literal ID="litError" runat="server"></asp:Literal></p>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFormComplete" Visible="false" runat="server">
                    <p>Thank you for your inquiry. A member of our Education Division will contact you with further information. If you have questions, please contact us at (813) 974-4357.</p>
                    <asp:Literal ID="litComplete" runat="server"></asp:Literal>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="/resources/forms/scripts/HealthIS.Apps.Forms.js"></script>
</body>
</html>
