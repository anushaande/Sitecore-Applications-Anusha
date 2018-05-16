<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SendComment.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.SendComment" %>

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
                    <div class="send-comment-form">
                        <p class="form-group">
                            <asp:Label ID="lblName" AssociatedControlID="tb_Name" runat="server" Text="Name *" /><br />
                            <asp:TextBox ID="tb_Name" runat="server" CssClass="form-control" placeholder="Name"></asp:TextBox>                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblEmail_Address" AssociatedControlID="tb_Email_Address" runat="server" Text="Email *" /><br />
                            <asp:TextBox ID="tb_Email_Address" runat="server" CssClass="form-control" placeholder="someone@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblSubject" AssociatedControlID="tb_Subject" runat="server" Text="Message Subject *" /><br />
                            <asp:TextBox ID="tb_Subject" runat="server" CssClass="form-control" placeholder="Subject"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_Subject" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>                        
                        <p class="form-group">
                            <asp:Label ID="lblMessage" AssociatedControlID="tb_Message" runat="server" Text="Message *" /><br />
                            <asp:TextBox ID="tb_Message" runat="server" CssClass="form-control" placeholder="Please enter your comment here." TextMode="MultiLine" Rows="5"></asp:TextBox>                            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_Message" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <input id="btnReset" class="btn btn-reset btn-usf-health bg-usf-rhubarb" onclick="location.reload(true)" type="button" value="Reset"/>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn-usf-health bg-usf-silvergray" Text="Submit" OnClick="btnSubmit_Click" />
                        <p class="error"><asp:Literal ID="litError" runat="server"></asp:Literal></p>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFormComplete" Visible="false" runat="server">
                    <p>Thank you. Your comment has been sent successfully.</p>
                    <asp:Literal ID="litComplete" runat="server"></asp:Literal>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="/resources/forms/scripts/HealthIS.Apps.Forms.js"></script>
</body>
</html>