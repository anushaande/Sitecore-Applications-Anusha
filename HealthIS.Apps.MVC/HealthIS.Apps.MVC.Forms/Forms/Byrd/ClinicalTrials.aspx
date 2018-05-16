<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClinicalTrials.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.ClinicalTrials" %>

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
                    <div class="ct-form">
                        <p class="form-group">
                            <asp:Label ID="lblPatient_Name" AssociatedControlID="tb_Patient_Name" runat="server" Text="Patient Name" /><br />
                            <asp:TextBox ID="tb_Patient_Name" runat="server" CssClass="form-control" placeholder="Patient Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_Patient_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblCaregiver_Legal_Guardian" AssociatedControlID="tb_Caregiver_Legal_Guardian" runat="server" Text="Caregiver/Legal Guardian" /><br />
                            <asp:TextBox ID="tb_Caregiver_Legal_Guardian" runat="server" CssClass="form-control" placeholder="Caregiver/Legal Guardian"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Caregiver_Legal_Guardian" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lbl_Patient_Age" AssociatedControlID="tb_Patient_Age" runat="server" Text="Patient Age" /><br />
                            <asp:TextBox ID="tb_Patient_Age" runat="server" CssClass="form-control" placeholder="Patient Age"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_Patient_Age" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblPhone_Number" AssociatedControlID="tb_Phone_Number" runat="server" Text="Phone" /><br />
                            <asp:TextBox ID="tb_Phone_Number" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_Phone_Number" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Phone_Number" runat="server" ErrorMessage="" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblEmail_Address" AssociatedControlID="tb_Email_Address" runat="server" Text="Email" /><br />
                            <asp:TextBox ID="tb_Email_Address" runat="server" CssClass="form-control" placeholder="someone@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblPatient_Diagnosis" AssociatedControlID="rbl_Patient_Diagnosis" runat="server" Text="Please select your patient's diagnosis:" />
                            <asp:RadioButtonList ID="rbl_Patient_Diagnosis" runat="server">
                                <asp:ListItem Text="Normal" Value="Normal"></asp:ListItem>
                                <asp:ListItem Text="Mild Cognitive Impairment" Value="Mild Cognitive Impairment"></asp:ListItem>
                                <asp:ListItem Text="Mild Alzheimer's Disease" Value="Mild Alzheimer's Disease"></asp:ListItem>
                                <asp:ListItem Text="Moderate Alzheimer's Disease" Value="Moderate Alzheimer's Disease"></asp:ListItem>
                                <asp:ListItem Text="I Don't Know" Value="I Don't Know"></asp:ListItem>
                            </asp:RadioButtonList>            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="rbl_Patient_Diagnosis" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator> 
                        </p>                        
                        <input id="btnReset" class="btn btn-reset btn-usf-health bg-usf-rhubarb" onclick="location.reload(true)" type="button" value="Reset"/>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn-usf-health bg-usf-silvergray" Text="Submit" OnClick="btnSubmit_Click" />
                        <p class="error"><asp:Literal ID="litError" runat="server"></asp:Literal></p>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFormComplete" Visible="false" runat="server">
                    <p>Thank you for your inquiry. If you have questions about a particular clinical study, please contact us at (813) 974-4355.</p>
                    <asp:Literal ID="litComplete" runat="server"></asp:Literal>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="/resources/forms/scripts/HealthIS.Apps.Forms.js"></script>
</body>
</html>
