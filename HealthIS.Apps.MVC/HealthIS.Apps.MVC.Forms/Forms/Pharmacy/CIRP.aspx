<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CIRP.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.CIRP" %>

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
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel CssClass="ContactForm" ID="pnlFormStart" runat="server">
                    <div class="standard-container">
                        <p>
                            Thank you for using the CIRP. Please feel out the form below, select submit, and we will provide you with a quote for services requested.
                        </p>
                    </div>
                    <div class="cirp-form">
                        <p class="form-group">
                            <span>
                                <asp:Label ID="lblEx" AssociatedControlID="rbl_CIRP_Used_Before" runat="server" Text="Have you used the CIRP before? *" />
                            </span>
                            <asp:RadioButtonList ID="rbl_CIRP_Used_Before" runat="server">
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="rbl_CIRP_Used_Before" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator> 
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblDepartment" AssociatedControlID="tb_Department" runat="server" Text="Department *" /><br />
                            <asp:TextBox ID="tb_Department" runat="server" CssClass="form-control" placeholder="Department *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Department" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblEmail_Address" AssociatedControlID="tb_Email_Address" runat="server" Text="Email Address *" /><br />
                            <asp:TextBox ID="tb_Email_Address" runat="server" CssClass="form-control" placeholder="someone@email.com *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblPhone_Number" AssociatedControlID="tb_Phone_Number" runat="server" Text="Phone Number" /><br />
                            <asp:TextBox ID="tb_Phone_Number" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                        </p>
                        <hr />
                        <p class="form-group">
                            <asp:Label ID="lblService_Request" AssociatedControlID="chb_Service_Request" runat="server" Text="Please check the box or boxes below for the service(s) you would like to request:" />
                            <asp:CheckBoxList ID="chb_Service_Request" runat="server" CssClass="checkbox">
                                <asp:ListItem Text="Sterile dose preparations in our 797 compliant cleanroom utilizing a Laminar Flow Hood or a Biological Safety Cabinet" Value="Sterile dose preparations in our 797 compliant cleanroom utilizing a Laminar Flow Hood or a Biological Safety Cabinet"></asp:ListItem>
                                <asp:ListItem Text="Order, receive, inventory and prompt secure storage of study product from all shipping sources" Value="Order, receive, inventory and prompt secure storage of study product from all shipping sources"></asp:ListItem>
                                <asp:ListItem Text="State of the art locked temperature controlled storage for refrigeration or room temperature" Value="State of the art locked temperature controlled storage for refrigeration or room temperature"></asp:ListItem>
                                <asp:ListItem Text="Creation of study related documents: dose calculation, product accountability, patient specific product accountability, and dispensation record, hand off log, product labels, monitoring visit log, product return or destruction log and randomization tables" Value="Creation of study related documents: dose calculation, product accountability, patient specific product accountability, and dispensation record, hand off log, product labels, monitoring visit log, product return or destruction log and randomization tables"></asp:ListItem>
                                <asp:ListItem Text="Placebo creation to match an Active drug for topical, oral (liquids and capsules), syringes or infusions" Value="Placebo creation to match an Active drug for topical, oral (liquids and capsules), syringes or infusions"></asp:ListItem>
                                <asp:ListItem Text="Strict maintenance of the blind with all measures taken to not reveal the identity of the active or placebo" Value="Strict maintenance of the blind with all measures taken to not reveal the identity of the active or placebo"></asp:ListItem>
                                <asp:ListItem Text="Strict HIPAA compliance utilizing white paper bags for handoff and transport of study product as well as offsite paper shredding and locked cabinets for study binder storage" Value="Strict HIPAA compliance utilizing white paper bags for handoff and transport of study product as well as offsite paper shredding and locked cabinets for study binder storage"></asp:ListItem>
                                <asp:ListItem Text="Utilization of an IVRS or IWRS system for drug dispensation" Value="Utilization of an IVRS or IWRS system for drug dispensation"></asp:ListItem>
                                <asp:ListItem Text="Monthly inventory or product accountability with onsite locked quarantine for used or expired products, offsite destruction service or to hold for product return with the monitor" Value="Monthly inventory or product accountability with onsite locked quarantine for used or expired products, offsite destruction service or to hold for product return with the monitor"></asp:ListItem>
                                <asp:ListItem Text="Order study product for a PI-initiated non-sponsored study with usually a 24 hour turnaround" Value="Order study product for a PI-initiated non-sponsored study with usually a 24 hour turnaround"></asp:ListItem>
                                <asp:ListItem Text="Site initiation visits, monitoring visits from the sponsor & close-out of study visits" Value="Site initiation visits, monitoring visits from the sponsor & close-out of study visits"></asp:ListItem>
                                <asp:ListItem Text="Convenient Monthly invoicing" Value="Convenient Monthly invoicing"></asp:ListItem>
                                <asp:ListItem Text="Flexible scheduling for dose preparations and sponsor visits through Outlook" Value="Flexible scheduling for dose preparations and sponsor visits through Outlook"></asp:ListItem>
                                <asp:ListItem Text="Maintain all forms in the study binder and all correspondence from the sponsor or study coordinator" Value="Maintain all forms in the study binder and all correspondence from the sponsor or study coordinator"></asp:ListItem>
                            </asp:CheckBoxList>
                        </p>
                        <hr />
                        <p>
                            <b>CIRP Compounding Services:</b> perfect for patients having difficulty taking medication as directed. We are equipped to work with you to creatively solve unique medication challenges on a patient by patient basis.
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblCompounding_Services" AssociatedControlID="chb_Compounding_Services" runat="server" Text="Please check the box or boxes below for the service(s) you would like to request:" />
                            <asp:CheckBoxList ID="chb_Compounding_Services" runat="server" CssClass="checkbox">
                                <asp:ListItem Text="UNGUATOR® Technology for homogenous cream, lotions and gels compounding" Value="UNGUATOR® Technology for homogenous cream, lotions and gels compounding"></asp:ListItem>
                                <asp:ListItem Text="Quality System for professional production of prescriptions" Value="Quality System for professional production of prescriptions"></asp:ListItem>
                                <asp:ListItem Text="Advanced Compounding and Industrial Pharmacy Syllabus (Class and Laboratory)" Value="Advanced Compounding and Industrial Pharmacy Syllabus (Class and Laboratory)"></asp:ListItem>
                                <asp:ListItem Text="Advanced formulations design, development and application in the real world" Value="Advanced formulations design, development and application in the real world"></asp:ListItem>
                                <asp:ListItem Text="Research Pharmacy order form for topical compounding" Value="Research Pharmacy order form for topical compounding"></asp:ListItem>
                                <asp:ListItem Text="Placebo creation to match an Active drug for topical, oral (liquids and capsules), syringes or infusions" Value="Placebo creation to match an Active drug for topical, oral (liquids and capsules), syringes or infusions"></asp:ListItem>
                                <asp:ListItem Text="Up to date with current law and regulation for pharmaceutical compounding" Value="Up to date with current law and regulation for pharmaceutical compounding"></asp:ListItem>
                            </asp:CheckBoxList>
                        </p>
                        <hr />
                        <p>
                            <b>
                                Pharmacy compounding allows the CIRP Pharmacist to find their niche as medication problem-solvers. The one size fits all nature of many mass-manufactured medications means that some patients' needs are not met by those products.
                            </b>
                        </p>
                        <hr />
                        <input id="btnReset" class="btn btn-reset btn-usf-health bg-usf-rhubarb" onclick="location.reload(true)" type="button" value="Reset"/>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn-usf-health bg-usf-silvergray" Text="Submit" OnClick="btnSubmit_Click" />
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
