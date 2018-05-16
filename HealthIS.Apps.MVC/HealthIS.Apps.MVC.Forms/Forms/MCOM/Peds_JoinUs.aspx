<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Peds_JoinUs.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.Peds_JoinUs" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Join Us</title>
    <link rel="stylesheet" href="/layouts/styles/health.css"/>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <script type="text/javascript" src="/resources/scripts/HealthIS.Apps.Util.js"></script>
    <style>
        .btn-success {
            margin-top: 10px;
        }
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
                <div>
                    <a href="https://www.facebook.com/groups/988049841227292/" target="_blank"><img alt="" src="http://health.usf.edu/~/media/Images/Medicine/Internal%20Medicine/Infectious%20Disease%20and%20International%20Medicine/HIV%20Clinical%20Research/finduson_facebook_nqfz.jpg" style="float: right; max-width: 170px;" /></a>
                </div>
                <p>Note: All information is not required; <em>required fields are indicated by an asterisk (*).</em><br>
                Any information given is for the Alumni group only and will not be shared.</p>
                <hr />
                <asp:Panel ID="pnlFormStart" runat="server">
                    <div class="ContactForm">
                        <div class="form-group">
                            <asp:Label ID="lblFirst_Name" AssociatedControlID="tb_First_Name" runat="server" Text="First Name *" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_First_Name" CssClass="form-control" runat="server" placeholder="First Name *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_First_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblLast_Name" AssociatedControlID="tb_Last_Name" runat="server" Text="Last Name *" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Last_Name" CssClass="form-control" runat="server" placeholder="Last Name *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Last_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblPostnomial" AssociatedControlID="ddl_Postnomial" runat="server" Text="MD/DO *" CssClass="form-label"></asp:Label>
                            <asp:DropDownList ID="ddl_Postnomial" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="No Selection"></asp:ListItem>
                                <asp:ListItem Text="MD" Value="MD"></asp:ListItem>
                                <asp:ListItem Text="DO" Value="DO"></asp:ListItem>
                            </asp:DropDownList>  
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="ddl_Postnomial" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblDate_of_Birth" AssociatedControlID="tb_Date_of_Birth" runat="server" Text="Date of Birth" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Date_of_Birth" CssClass="form-control" runat="server" placeholder="MM/DD/YYYY"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblResidency_Grad_Year" AssociatedControlID="tb_Residency_Grad_Year" runat="server" Text="Residency Graduation Year *" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Residency_Grad_Year" CssClass="form-control" runat="server" placeholder="Residency Graduation Year *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_Residency_Grad_Year" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <hr />
                        <h4><asp:Label runat="server" CssClass="form-label" Text="Home Address"></asp:Label></h4>
                        <div class="form-group">
                            <asp:Label ID="lblHome_Address" AssociatedControlID="tb_Home_Address" runat="server" Text="Street Address *" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Home_Address" CssClass="form-control" runat="server" placeholder="Street Address *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tb_Home_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblHome_Address2" AssociatedControlID="tb_Home_Address2" runat="server" Text="Address Line 2" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Home_Address2" CssClass="form-control" runat="server" placeholder="optional"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblHome_City" AssociatedControlID="tb_Home_City" runat="server" Text="City *" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Home_City" CssClass="form-control" runat="server" placeholder="City *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="tb_Home_City" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblHome_State" AssociatedControlID="tb_Home_State" runat="server" Text="State *" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Home_State" CssClass="form-control" runat="server" placeholder="State *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="tb_Home_State" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblHome_Zip" AssociatedControlID="tb_Home_Zip" runat="server" Text="Zip Code *" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Home_Zip" CssClass="form-control" runat="server" placeholder="Zip Code *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="tb_Home_Zip" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <hr />
                        <h4><asp:Label runat="server" CssClass="form-label" Text="Office Address"></asp:Label></h4>
                        <div class="form-group">
                            <asp:Label ID="lblOffice_Name" AssociatedControlID="tb_Office_Name" runat="server" Text="Office Name *" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Office_Name" CssClass="form-control" runat="server" placeholder="Office Name *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="tb_Office_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>                        
                        <div class="form-group">
                            <asp:Label ID="lblOffice_Address" AssociatedControlID="tb_Office_Address" runat="server" Text="Street Address" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Office_Address" CssClass="form-control" runat="server" placeholder="Street Address"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblOffice_Address2" AssociatedControlID="tb_Office_Address2" runat="server" Text="Address Line 2" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Office_Address2" CssClass="form-control" runat="server" placeholder="optional"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblOffice_City" AssociatedControlID="tb_Office_City" runat="server" Text="City" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Office_City" CssClass="form-control" runat="server" placeholder="City"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblOffice_State" AssociatedControlID="tb_Office_State" runat="server" Text="State" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Office_State" CssClass="form-control" runat="server" placeholder="State"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblOffice_Zip" AssociatedControlID="tb_Office_Zip" runat="server" Text="Zip Code" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Office_Zip" CssClass="form-control" runat="server" placeholder="Zip Code"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblOffice_Telephone" AssociatedControlID="tb_Office_Telephone" runat="server" Text="Office Telephone" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Office_Telephone" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Office_Telephone" runat="server" ErrorMessage="" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                        </div>
                        <hr />
                        <div class="form-group">
                            <asp:Label ID="lblPrimary_Email" AssociatedControlID="tb_Primary_Email" runat="server" Text="Primary Email *" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Primary_Email" CssClass="form-control" runat="server" placeholder="someone@email.com *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="tb_Primary_Email" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tb_Primary_Email" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblSecondary_Email" AssociatedControlID="tb_Secondary_Email" runat="server" Text="Secondary Email" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Secondary_Email" CssClass="form-control" runat="server" placeholder="someone@email.com"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator3" ControlToValidate="tb_Secondary_Email" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblCell_Number" AssociatedControlID="tb_Cell_Number" runat="server" Text="Cell Phone" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Cell_Number" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator4" ControlToValidate="tb_Cell_Number" runat="server" ErrorMessage="" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblSpouse_Partner" AssociatedControlID="tb_Spouse_Partner" runat="server" Text="Spouse/Partner" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Spouse_Partner" CssClass="form-control" runat="server" placeholder="Spouse/Partner"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblSpecial_Interests" AssociatedControlID="tb_Special_Interests" runat="server" Text="Special Interests" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Special_Interests" TextMode="MultiLine" Rows="5" CssClass="form-control" runat="server" placeholder="Please list your special interests here."></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblMembership_Level" AssociatedControlID="ddl_Membership_Level" runat="server" Text="Membership Level" CssClass="form-label"></asp:Label>
                            <asp:DropDownList ID="ddl_Membership_Level" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="No Selection"></asp:ListItem>
                                <asp:ListItem Text="Alumni Annual Member: $50" Value="Alumni Annual Member: $50"></asp:ListItem>
                                <asp:ListItem Text="Lifetime Membership: $1000" Value="Lifetime Membership: $1000"></asp:ListItem>                                   
                            </asp:DropDownList>                
                        </div>
                        <input id="btnReset" class="btn btn-reset btn-usf-health bg-usf-rhubarb" onclick="location.reload(true)" type="button" value="Reset">
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn-usf-health bg-usf-green" Text="Submit Registration" OnClick="btnSubmit_Click" />
                        <p class="error"><asp:Literal ID="litError" runat="server"></asp:Literal></p>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFormComplete" Visible="false" runat="server">
                    <p><b>If you prefer to mail in a check</b>, please make checks payable to "USF Foundation, Inc."<br> Please note in memo section of check "USF Pediatric Residency Alumni Fund".</p>
                    <p>The USF Foundation is a 501(c) 3 tax-exempt organization. Contributions to the USF Foundation are tax-deductible to the extent allowed by law.</p>
                    <p>Please return this completed form with your gift payment to:<br>USF Health Development Office<br>12901 Bruce B. Downs Blvd.<br>MDC 70<br>Tampa, FL 33612-9986<br><br>OR FAX this form to 813-974-2936 (Attn: Kara Steiner)</p>
                    <div class="btn payment-btn btn-success"><a target="_blank" href="https://usffdn.usf.edu/ua-giving/default.aspx?search=250279&Activity=FUNDS&tab=All&origin=&comments=&solcode=&a=" style="color: white">Continue to Payment</a></div>
                    <asp:Literal ID="litComplete" runat="server"></asp:Literal>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="/resources/forms/scripts/HealthIS.Apps.Forms.js"></script>
</body>
</html>
