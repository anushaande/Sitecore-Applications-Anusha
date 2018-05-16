<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SIPAIID_MemberInfo.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.SIPAIID_MemberInfo" %>

<!DOCTYPE html>

<head id="Head1" runat="server">
    <title>SIPAIID Member Information</title>
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
                    <div class="ContactForm">                        
                            <div class="form-group"> 
                                <asp:Label ID="lblFirst_Name" AssociatedControlID="tb_First_Name" runat="server" Text="First Name *" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tb_First_Name" CssClass="form-control" runat="server" placeholder="First Name *"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_First_Name" runat="server"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblLast_Name" AssociatedControlID="tb_Last_Name" runat="server" Text="Last Name *" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tb_Last_Name" CssClass="form-control" runat="server" placeholder="Last Name *"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Last_Name" runat="server"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblAffiliation" AssociatedControlID="tb_Affiliation" runat="server" Text="Title/Affliation *" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tb_Affiliation" CssClass="form-control" runat="server" placeholder="Title/Affliation *"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_Affiliation" runat="server"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblDepartment" AssociatedControlID="tb_Department" runat="server" Text="Department *" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tb_Department" CssClass="form-control" runat="server" placeholder="Department *"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_Department" runat="server"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblPhone_Number" AssociatedControlID="tb_Phone_Number" runat="server" Text="Phone Number *" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tb_Phone_Number" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tb_Phone_Number" runat="server"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Phone_Number" runat="server" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblEmail" AssociatedControlID="tb_Email" runat="server" Text="Email Address *" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tb_Email" CssClass="form-control" runat="server" placeholder="someone@email.com"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="tb_Email" runat="server"></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tb_Email" runat="server" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblRole" AssociatedControlID="rbl_Role" runat="server" Text="Please select one of the following *" />
                                <asp:RadioButtonList ID="rbl_Role" runat="server">
                                    <asp:ListItem Text="Faculty" Value="Faculty"></asp:ListItem>
                                    <asp:ListItem Text="Staff" Value="Staff"></asp:ListItem>
                                    <asp:ListItem Text="Student" Value="Student"></asp:ListItem>
                                    <asp:ListItem Text="Other" Value="Other"></asp:ListItem>                                    
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="rbl_Role" runat="server"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblBasic_or_Clinical" AssociatedControlID="rbl_Basic_or_Clinical" runat="server" Text="Please select one of the following *" />
                                <asp:RadioButtonList ID="rbl_Basic_or_Clinical" runat="server">
                                    <asp:ListItem Text="Basic" Value="Basic"></asp:ListItem>
                                    <asp:ListItem Text="Clinical" Value="Clinical"></asp:ListItem>
                                </asp:RadioButtonList>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="rbl_Basic_or_Clinical" runat="server"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group">
                                Please give a brief description of your Research Area (Concentration) *<br />
                                <asp:TextBox ID="tb_Research_Area" runat="server" CssClass="form-control" placeholder="" TextMode="MultiLine" Rows="8"></asp:TextBox> 
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="tb_Research_Area" runat="server"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group"> 
                                Please give 5 words that describe your Expertise *<br />
                                <asp:TextBox ID="tb_Expertise" runat="server" CssClass="form-control" placeholder="" TextMode="MultiLine" Rows="8"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="tb_Expertise" runat="server"></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group"> 
                                Please give 5 words that describe your Research *<br />
                                <asp:TextBox ID="tb_Describe_Research" runat="server" CssClass="form-control" placeholder="" TextMode="MultiLine" Rows="8"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ControlToValidate="tb_Describe_Research" runat="server"></asp:RequiredFieldValidator>
                            </div>                       
                       <br />
                        <asp:Button ID="btnReset" runat="server" CssClass="btn btn-reset btn-usf-health bg-usf-rhubarb" Text="Reset" OnClick="btnReset_Click" CausesValidation="false" />
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn-usf-health bg-usf-green" Text="Submit" OnClick="btnSubmit_Click" />
                        <p class="error"><asp:Literal ID="litError" runat="server"></asp:Literal></p>
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