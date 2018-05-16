<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="GECRegistration.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.GECRegistration" %>

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
        .success-message span {
            text-decoration: underline;
            font-weight: bold;
        }
        .payment {
            color: #fff;
            padding-bottom:10px;
        }
        .payment a:hover {
            color: #fff;
            text-decoration: none;
        }
    </style>
</head>
<body class="form-body">    
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel CssClass="ContactForm" ID="pnlFormStart" runat="server">
                    <div class="gec-registration-form">
                        <p class="form-group">
                            <asp:Label ID="lblName" AssociatedControlID="tb_Name" runat="server" Text="Name, Degree(s) *" /><br />
                            <asp:TextBox ID="tb_Name" runat="server" CssClass="form-control" placeholder="Name, Degree(s)"></asp:TextBox>
                            <asp:TextBox ID="tb_Name2" runat="server" CssClass="form-control" placeholder="Name, Degree(s)"></asp:TextBox>
                            <asp:TextBox ID="tb_Name3" runat="server" CssClass="form-control" placeholder="Name, Degree(s)"></asp:TextBox>
                            <asp:TextBox ID="tb_Name4" runat="server" CssClass="form-control" placeholder="Name, Degree(s)"></asp:TextBox>
                            <asp:TextBox ID="tb_Name5" runat="server" CssClass="form-control" placeholder="Name, Degree(s)"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblOrganization_Company_Name" AssociatedControlID="tb_Organization_Company_Name" runat="server" Text="Organization/Company Name *" /><br />
                            <asp:TextBox ID="tb_Organization_Company_Name" runat="server" CssClass="form-control" placeholder="Organization/Company Name"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Organization_Company_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblCompany_interested_in_sponsoring_exhibit_booth" AssociatedControlID="rbl_Company_interested_in_sponsoring_exhibit_booth" runat="server" Text="My company/organization is interested in sponsoring an exhibit booth." />
                            <asp:RadioButtonList ID="rbl_Company_interested_in_sponsoring_exhibit_booth" runat="server">
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                                <asp:ListItem Text="I Don't Know" Value="I Don't Know"></asp:ListItem>
                            </asp:RadioButtonList>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblEmail_Address" AssociatedControlID="tb_Email_Address" runat="server" Text="Email *" /><br />
                            <asp:TextBox ID="tb_Email_Address" runat="server" CssClass="form-control" placeholder="someone@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblFlorida_License_Num_for_CEUs" AssociatedControlID="tb_Florida_License_Num_for_CEUs" runat="server" Text="Florida License # for CEU's *" /><br />
                            <asp:TextBox ID="tb_Florida_License_Num_for_CEUs" runat="server" CssClass="form-control" placeholder="Enter N/A if not applicable"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_Florida_License_Num_for_CEUs" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <div class="form-group">
                            <asp:Label ID="lblPrimary_Discipline_Profession" AssociatedControlID="ddl_Primary_Discipline_Profession" runat="server" Text="Please select your Primary Discipline/Profession *" />
                             <asp:DropDownList ID="ddl_Primary_Discipline_Profession" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="no selection made"></asp:ListItem>
                                <asp:ListItem Text="ALF Administrator" Value="ALF Administrator"></asp:ListItem>
                                <asp:ListItem Text="Care Manager" Value="Care Manager"></asp:ListItem>
                                <asp:ListItem Text="Guardian" Value="Guardian"></asp:ListItem>
                                <asp:ListItem Text="Gerontologist" Value="Gerontologist"></asp:ListItem>
                                <asp:ListItem Text="Mental Health Counselor" Value="Mental Health Counselor"></asp:ListItem>
                                <asp:ListItem Text="Nurse" Value="Nurse"></asp:ListItem>
                                <asp:ListItem Text="Nursing Home Administrator" Value="Nursing Home Administrator"></asp:ListItem>
                                <asp:ListItem Text="Ombudsman" Value="Ombudsman"></asp:ListItem>
                                <asp:ListItem Text="Occupational Therapist" Value="Occupational Therapist"></asp:ListItem>
                                <asp:ListItem Text="Public Health Practitioner" Value="Public Health Practitioner"></asp:ListItem>
                                <asp:ListItem Text="Social Worker" Value="Social Worker"></asp:ListItem>
                                <asp:ListItem Text="Registered Dietitian/Nutritionist" Value="Registered Dietitian/Nutritionist"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="tb_Other_Primary_Discipline_Profession" runat="server" CssClass="form-control" placeholder="If you selected Other, please type your response here."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="ddl_Primary_Discipline_Profession" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                        <em><p>The Byrd Institute is approved by the following organizations as an accredited provider of continuing education:</p>
                        <ul>
                            <li>Florida Board of Nursing</li>
                            <li>Florida Board of Clinical Social Work, Marriage and Family Therapy, and Mental Health Counseling</li>
                            <li>Florida Board of Nursing Home Administrators</li>
                            <li>Florida Board of Occupational Therapy</li>
                            <li>Florida Council of Dietetics and Nutrition</li>
                        </ul>
                        <p>For your convenience, CE credits are entered onto the CE Broker website.</p></em>
                        <p class="form-group">
                            <asp:Label ID="lblStreet_Address" AssociatedControlID="tb_Street_Address" runat="server" Text="Street Address *" /><br />
                            <asp:TextBox ID="tb_Street_Address" runat="server" CssClass="form-control" placeholder="Street Address"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="tb_Street_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblAddress_Line_2" AssociatedControlID="tb_Address_Line_2" runat="server" Text="Address Line 2" /><br />
                            <asp:TextBox ID="tb_Address_Line_2" runat="server" CssClass="form-control" placeholder="Address Line 2"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblCity" AssociatedControlID="tb_City" runat="server" Text="City *" /><br />
                            <asp:TextBox ID="tb_City" runat="server" CssClass="form-control" placeholder="City"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="tb_City" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblState" AssociatedControlID="tb_State" runat="server" Text="State / Province / Region *" /><br />
                            <asp:TextBox ID="tb_State" runat="server" CssClass="form-control" placeholder="State / Province / Region"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="tb_State" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lbl_Zip_Code" AssociatedControlID="tb_Zip_Code" runat="server" Text="Postal/Zip Code *" /><br />
                            <asp:TextBox ID="tb_Zip_Code" runat="server" CssClass="form-control" placeholder="Postal/Zip Code"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="tb_Zip_Code" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblPhone_Number" AssociatedControlID="tb_Phone_Number" runat="server" Text="Phone Number *" /><br />
                            <asp:TextBox ID="tb_Phone_Number" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="tb_Phone_Number" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tb_Phone_Number" runat="server" ErrorMessage="" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblFax" AssociatedControlID="tb_Fax" runat="server" Text="Fax" /><br />
                            <asp:TextBox ID="tb_Fax" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                        </p>
                        <div class="form-group">
                            <asp:Label ID="lblIndividual_Registration" AssociatedControlID="ddl_Individual_Registration" runat="server" Text="Individual Registration" />
                            <p>Groups registering from the same company or with a professional association can receive a team discount of 15% off early bird and standard rates for three or more people. In order to receive a team discount, the group must submit their registration and payment at one time. Team discounts are non-transferable. Individuals registering from the same company or employer must be located at the same physical site location to receive discount.</p> 
                            <asp:DropDownList ID="ddl_Individual_Registration" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="no selection made"></asp:ListItem>
                                <asp:ListItem Text="Individual Single Day, Early Bird Rate (On or Before April 30th) -- $95" Value="Individual 1-Day Early Bird"></asp:ListItem>
                                <asp:ListItem Text="Individual 3-Day, Early Bird Rate (On or Before April 30th) -- $250" Value="Individual 3-Day Early Bird"></asp:ListItem>
                                <asp:ListItem Text="Individual Single Day -- $105" Value="Individual 1-Day"></asp:ListItem>
                                <asp:ListItem Text="Individual 3-Day -- $275" Value="Individual 3-Day"></asp:ListItem>
                                <asp:ListItem Text="Team Discount, Early Bird Daily Rate (On or Before April 30th) -- $80.75pp" Value="Team Discount 1-Day Early Bird"></asp:ListItem>
                                <asp:ListItem Text="Team Discount, Early Bird 3-Day Rate (On or Before April 30th) -- $212.50pp" Value="Team Discount 3-Day Early Bird"></asp:ListItem>
                                <asp:ListItem Text="Team Discount, Standard Daily Rate -- $89.25pp" Value="Team Discount 1-Day"></asp:ListItem>
                                <asp:ListItem Text="Team Discount, Standard 3-Day Rate -- $233.75pp" Value="Team Discount 3-Day"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblExhibitor_Registration" AssociatedControlID="ddl_Exhibitor_Registration" runat="server" Text="Exhibitor Registration" />
                            <p>Standard 3-day exhibitor rate does not allow access to conference program sessions.</p>                            
                             <asp:DropDownList ID="ddl_Exhibitor_Registration" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="no selection made"></asp:ListItem>
                                <asp:ListItem Text="Exhibitor 3-Day, Early Bird Rate (On or Before April 30th) -- $495" Value="Exhibitor 3-Day Early Bird"></asp:ListItem>
                                <asp:ListItem Text="Exhibitor 3-Day -- $545" Value="Exhibitor 3-Day"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <p class="form-group">
                            <asp:Label ID="lblDays_of_conference_attendance" AssociatedControlID="chb_Days_of_conference_attendance" runat="server" Text="Select day(s) of conference attendance *" />
                            <asp:CheckBoxList ID="chb_Days_of_conference_attendance" runat="server" CssClass="checkbox">
                                <asp:ListItem Text="June 13" Value="June 13"></asp:ListItem>
                                <asp:ListItem Text="June 14" Value="June 14"></asp:ListItem>
                                <asp:ListItem Text="June 15" Value="June 15"></asp:ListItem>                                
                            </asp:CheckBoxList>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblBooth_Representatives" AssociatedControlID="tb_Booth_Representative1" runat="server" Text="Name of Exhibit Booth Representative(s)" /><br />
                            <p>Exhibitor rate does not allow access to conference program sessions and are non-transferable. Exhibitor registration fee includes a 6-foot table and morning/afternoon refreshments for two individuals and (2) lunch tickets.</p>
                            <asp:TextBox ID="tb_Booth_Representative1" CssClass="form-control" runat="server" placeholder="Booth representative 1 (Please enter title, first/last name)"></asp:TextBox>
                            <asp:TextBox ID="tb_Booth_Representative2" CssClass="form-control" runat="server" placeholder="Booth representative 2 (Please enter title, first/last name)"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lbl_Booth_reps_want_to_attend_education_program_sessions" AssociatedControlID="rbl_Booth_reps_want_to_attend_education_program_sessions" runat="server" Text="Does the booth representative(s) want to attend the education program sessions?" />
                            <asp:RadioButtonList ID="rbl_Booth_reps_want_to_attend_education_program_sessions" runat="server">
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            </asp:RadioButtonList>
                        </p>
                        <div class="form-group">
                            <asp:Label ID="lblExhibitor_plus_Conference_Attendee" AssociatedControlID="ddl_Exhibitor_plus_Conference_Attendee" runat="server" Text="Exhibitor + Conference Attendee Rates" />
                            <p>Rates apply per person. Includes access to program sessions, lunch and morning/afternoon refreshments.</p>
                             <asp:DropDownList ID="ddl_Exhibitor_plus_Conference_Attendee" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="no selection made"></asp:ListItem>
                                <asp:ListItem Text="Sole Exhibitor + Conference Attendee -- $250 per day" Value="Sole Exhibitor plus Conference Attendee"></asp:ListItem>
                                <asp:ListItem Text="Additional Exhibitor + Conference Attendee -- $105 per day" Value="Additional Exhibitor plus Conference Attendee"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label ID="lblMethod_of_payment" AssociatedControlID="ddl_Method_of_payment" runat="server" Text="Method of Payment *" />
                            <p>Rates apply per person.</p>
                             <asp:DropDownList ID="ddl_Method_of_payment" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="no selection made"></asp:ListItem>
                                <asp:ListItem Text="Credit Card (Online through USF payment system)" Value="Credit Card"></asp:ListItem>
                                <asp:ListItem Text="Check (Online through USF payment system)" Value="Check"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ControlToValidate="ddl_Method_of_payment" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                        <p class="form-group">
                            Thank you for supporting the USF Health Byrd Alzheimer's Institute, 2017 Geriatric Health Care Conference!<br />
                            Additional comments can be noted below or you can call (813) 396-0659 with any questions.<br />                            
                            <asp:TextBox ID="tb_Comments" runat="server" CssClass="form-control" placeholder="" TextMode="MultiLine" Rows="3"></asp:TextBox>                            
                        </p>
                        <p class="form-group">
                           All payments must be made online. For your convenience we accept credit card, debit card, and check online through the USF payment system. Cash payments will not be accepted.
                        </p>
                        <p class="form-group">
                            <span id ="lblCancellation_Policy">Cancellation Policy</span><br /><br />Early bird rates end April 30, 2017. All cancellations must be submitted in writing to the USF Health Byrd Institute. Cancellations received by April 30, 2017 will be granted a 100% refund. Cancellations received by May 31, 2017 will be granted a 50% refund. NO REFUNDS OR CREDITS will be issued after May 31. 
                        </p>
                        <input id="btnReset" class="btn btn-reset btn-usf-health bg-usf-rhubarb" onclick="location.reload(true)" type="button" value="Reset"/>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn-usf-health bg-usf-silvergray" Text="Submit Registration" OnClick="btnSubmit_Click" />
                        <p class="error"><asp:Literal ID="litError" runat="server"></asp:Literal></p>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFormComplete" Visible="false" runat="server">
                    <h5 class="success-message">Conference registration sent successfully.<br />NOTE: Registration will <span>NOT</span> be accepted without payment.</h5>
                    <p class="payment"><a class="btn btn-usf-health btn-icon bg-usf-teal" role="button" target="_blank" href="http://hsccf.hsc.usf.edu/conference/viewSchedule.cfm?CFID=18063825&CFTOKEN=10d3138a99e1c665-38D7A95D-5056-AC57-CF6762D95C191C5A">Continue to Payment<span class="icon-chevron-right"></span></a></p>
                    <asp:Literal ID="litComplete" runat="server"></asp:Literal>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="/resources/forms/scripts/HealthIS.Apps.Forms.js"></script>
</body>
</html>