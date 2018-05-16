<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MemoryScreening.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.MemoryScreening" %>

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
                    <div class="ms-form">
                        <p class="form-group">
                            <asp:Label ID="lblName" AssociatedControlID="tb_Name" runat="server" Text="Name" /><br />
                            <asp:TextBox ID="tb_Name" runat="server" CssClass="form-control" placeholder="Name (person who will be screened)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblAge" AssociatedControlID="tb_Age" runat="server" Text="Age" /><br />
                            <asp:TextBox ID="tb_Age" runat="server" CssClass="form-control" placeholder="Age (memory screening recommended for persons aged 65 years and older)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Age" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblPhone_Number" AssociatedControlID="tb_Phone_Number" runat="server" Text="Phone" /><br />
                            <asp:TextBox ID="tb_Phone_Number" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_Phone_Number" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Phone_Number" runat="server" ErrorMessage="" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblEmail_Address" AssociatedControlID="tb_Email_Address" runat="server" Text="Email" /><br />
                            <asp:TextBox ID="tb_Email_Address" runat="server" CssClass="form-control" placeholder="someone@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblSeeing_physician_at_Byrd" AssociatedControlID="rbl_Seeing_physician_at_Byrd" runat="server" Text="Are you (or your loved one) currently seeing a physician at the Byrd Alzheimer's Institute?" />
                            <asp:RadioButtonList ID="rbl_Seeing_physician_at_Byrd" runat="server">
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="rbl_Seeing_physician_at_Byrd" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator> 
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblBeing_treated_for_a_memory_disorder_by_any_physician" AssociatedControlID="rbl_Being_treated_for_a_memory_disorder_by_any_physician" runat="server" Text="Are you (or your loved one) being treated for dementia or some other memory disorder by any physician or medical provider?" />
                            <asp:RadioButtonList ID="rbl_Being_treated_for_a_memory_disorder_by_any_physician" runat="server">
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="rbl_Being_treated_for_a_memory_disorder_by_any_physician" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator> 
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblWhy_you_feel_screening_is_needed" AssociatedControlID="chb_Why_you_feel_screening_is_needed" runat="server" Text="Why do you feel you need a memory screening? (please check all that apply)" />
                            <asp:CheckBoxList ID="chb_Why_you_feel_screening_is_needed" runat="server" CssClass="checkbox">
                                <asp:ListItem Text="I am experiencing memory problems" Value="I am experiencing memory problems"></asp:ListItem>
                                <asp:ListItem Text="I have Alzheimer's disease in my family" Value="I have Alzheimer's disease in my family"></asp:ListItem>
                                <asp:ListItem Text="My family or friends have encouraged me to get screened" Value="My family or friends have encouraged me to get screened"></asp:ListItem>
                                <asp:ListItem Text="My primary healthcare provider encouraged me to get screened" Value="My primary healthcare provider encouraged me to get screened"></asp:ListItem>
                                <asp:ListItem Text="I want to see how I will do and obtain a score for future comparison" Value="I want to see how I will do and obtain a score for future comparison"></asp:ListItem>
                                <asp:ListItem Text="I have received a diagnosis of mild cognitive impairment" Value="I have received a diagnosis of mild cognitive impairment"></asp:ListItem>
                            </asp:CheckBoxList>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblInterest_in_clinical_research_opportunities" AssociatedControlID="rbl_Interest_in_clinical_research_opportunities" runat="server" Text="Do you have an interest in learning more about clinical research opportunities?" />
                            <asp:RadioButtonList ID="rbl_Interest_in_clinical_research_opportunities" runat="server">
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="rbl_Interest_in_clinical_research_opportunities" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator> 
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblHow_did_you_hear_about_Byrd_Institute" AssociatedControlID="chb_How_did_you_hear_about_Byrd_Institute" runat="server" Text="How did you hear about the Byrd Institute?" />
                            <asp:CheckBoxList ID="chb_How_did_you_hear_about_Byrd_Institute" runat="server" CssClass="checkbox">
                                <asp:ListItem Text="Self-Referral" Value="Self-Referral"></asp:ListItem>
                                <asp:ListItem Text="Spouse" Value="Spouse"></asp:ListItem>
                                <asp:ListItem Text="Child or Family Member" Value="Child or Family Member"></asp:ListItem>
                                <asp:ListItem Text="Clinician" Value="Clinician"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                            </asp:CheckBoxList>
                            <asp:TextBox ID="tb_Other_referral" runat="server" CssClass="form-control" placeholder="If other, please specify here."></asp:TextBox>
                        </p>
                        <div class="form-group">
                            <asp:Label ID="lblSubmitting_appointment_request_for" AssociatedControlID="ddl_Submitting_appointment_request_for" runat="server" Text="I am submitting this appointment request for:" />
                             <asp:DropDownList ID="ddl_Submitting_appointment_request_for" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="no selection made"></asp:ListItem>
                                 <asp:ListItem Text="Myself" Value="Myself"></asp:ListItem>
                                <asp:ListItem Text="Spouse/Partner" Value="Spouse/Partner"></asp:ListItem>
                                <asp:ListItem Text="Parent" Value="Parent"></asp:ListItem>
                                <asp:ListItem Text="Relative" Value="Relative"></asp:ListItem>
                                <asp:ListItem Text="Friend" Value="Friend"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="ddl_Submitting_appointment_request_for" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                        <p class="form-group">
                            <asp:Label ID="lblContact_Name" AssociatedControlID="tb_Contact_Name" runat="server" Text="Contact Name" /><br />
                            <asp:TextBox ID="tb_Contact_Name" runat="server" CssClass="form-control" placeholder="Contact Name (person submitting request)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="tb_Contact_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblPrimary_language_english" AssociatedControlID="rbl_Primary_language_english" runat="server" Text="Is your primary language English?" />
                            <asp:RadioButtonList ID="rbl_Primary_language_english" runat="server">
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="rbl_Primary_language_english" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator> 
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblAble_to_communicate_in_English" AssociatedControlID="rbl_Able_to_communicate_in_English" runat="server" Text="If no, are you able to communicate in English?" />
                            <asp:RadioButtonList ID="rbl_Able_to_communicate_in_English" runat="server">
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>                            
                        </p>
                        <input id="btnReset" class="btn btn-reset btn-usf-health bg-usf-rhubarb" onclick="location.reload(true)" type="button" value="Reset"/>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn-usf-health bg-usf-silvergray" Text="Submit" OnClick="btnSubmit_Click" />
                        <p class="error"><asp:Literal ID="litError" runat="server"></asp:Literal></p>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFormComplete" Visible="false" runat="server">
                    <p>Thank you for your request. A member of our clinical staff will contact you to confirm an appointment date/time.</p>
                    <asp:Literal ID="litComplete" runat="server"></asp:Literal>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="/resources/forms/scripts/HealthIS.Apps.Forms.js"></script>
</body>
</html>