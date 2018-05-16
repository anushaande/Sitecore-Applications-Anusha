<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WHOCC_Contact.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.WHOCC_Contact" %>

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
                    <div class="whocc-contact">                      
                        <p class="form-group">
                            <asp:Label ID="lblName" AssociatedControlID="tb_Name" runat="server" Text="Name *" /><br />
                            <asp:TextBox ID="tb_Name" CssClass="form-control" runat="server" placeholder="Name" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblTitle" AssociatedControlID="tb_Title" runat="server" Text="Title *" /><br />
                            <asp:TextBox ID="tb_Title" CssClass="form-control" runat="server" placeholder="Title" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Title" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>                        
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblInstitution" AssociatedControlID="tb_Institution" runat="server" Text="Institution *" /><br />
                            <asp:TextBox ID="tb_Institution" CssClass="form-control" runat="server" placeholder="Institution"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_Institution" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblEmail_Address" AssociatedControlID="tb_Email_Address" runat="server" Text="Email Address *" /><br />
                            <asp:TextBox ID="tb_Email_Address" runat="server" CssClass="form-control" placeholder="someone@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblPhone_Number" AssociatedControlID="tb_Phone_Number" runat="server" Text="Phone" /><br />
                            <asp:TextBox ID="tb_Phone_Number" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tb_Phone_Number" runat="server" ErrorMessage="" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">                                               
                            <asp:Label ID="lblMessage" AssociatedControlID="tb_Message" runat="server" Text="Message *" /><br />                            
                            <asp:TextBox ID="tb_Message" runat="server" CssClass="form-control" placeholder="Enter your message here." TextMode="MultiLine" Rows="5"></asp:TextBox>                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tb_Message" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>                            
                        </p>                        
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
