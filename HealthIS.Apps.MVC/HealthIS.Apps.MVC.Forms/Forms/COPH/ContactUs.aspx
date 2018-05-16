<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ContactUs.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.ContactUs" %>

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
                <p>Thank you for your interest in the USF College of Public Health.<br />
                    Please share your contact info with us and an advisor will follow-up with you soon.
                </p>
                <hr />
                <asp:Panel ID="pnlFormStart" runat="server">
                    <div class="ContactForm">
                        <div class="form-group"> 
                            <asp:TextBox ID="tbST_Program" CssClass="form-control" runat="server" placeholder="Type of Program Interested In *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tbST_Program" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:DropDownList ID="ddlST_Country" CssClass="form-control" runat="server">
                                <asp:ListItem Text="Country" Value="Country"></asp:ListItem>
                                <asp:ListItem Text="United States of America" Value="United States of America"></asp:ListItem>
                                <asp:ListItem Text="Afghanistan" Value="Afghanistan"></asp:ListItem>
                                <asp:ListItem Text="Albania" Value="Albania"></asp:ListItem>
                                <asp:ListItem Text="Algeria" Value="Algeria"></asp:ListItem>
                                <asp:ListItem Text="American Samoa" Value="American Samoa"></asp:ListItem>
                                <asp:ListItem Text="Andorra" Value="Andorra"></asp:ListItem>
                                <asp:ListItem Text="Angola" Value="Angola"></asp:ListItem>
                                <asp:ListItem Text="Anguilla" Value="Anguilla"></asp:ListItem>
                                <asp:ListItem Text="Antigua & Barbuda" Value="Antigua & Barbuda"></asp:ListItem>
                                <asp:ListItem Text="Argentina" Value="Argentina"></asp:ListItem>
                                <asp:ListItem Text="Armenia" Value="Armenia"></asp:ListItem>
                                <asp:ListItem Text="Aruba" Value="Aruba"></asp:ListItem>
                                <asp:ListItem Text="Australia" Value="Australia"></asp:ListItem>
                                <asp:ListItem Text="Austria" Value="Austria"></asp:ListItem>
                                <asp:ListItem Text="Azerbaijan" Value="Azerbaijan"></asp:ListItem>
                                <asp:ListItem Text="Bahamas" Value="Bahamas"></asp:ListItem>
                                <asp:ListItem Text="Bahrain" Value="Bahrain"></asp:ListItem>
                                <asp:ListItem Text="Bangladesh" Value="Bangladesh"></asp:ListItem>
                                <asp:ListItem Text="Barbados" Value="Barbados"></asp:ListItem>
                                <asp:ListItem Text="Belarus" Value="Belarus"></asp:ListItem>
                                <asp:ListItem Text="Belgium" Value="Belgium"></asp:ListItem>
                                <asp:ListItem Text="Belize" Value="Belize"></asp:ListItem>
                                <asp:ListItem Text="Benin" Value="Benin"></asp:ListItem>
                                <asp:ListItem Text="Bermuda" Value="Bermuda"></asp:ListItem>
                                <asp:ListItem Text="Bhutan" Value="Bhutan"></asp:ListItem>
                                <asp:ListItem Text="Bolivia" Value="Bolivia"></asp:ListItem>
                                <asp:ListItem Text="Bonaire" Value="Bonaire"></asp:ListItem>
                                <asp:ListItem Text="Bosnia & Herzegovina" Value="Bosnia & Herzegovina"></asp:ListItem>
                                <asp:ListItem Text="Botswana" Value="Botswana"></asp:ListItem>
                                <asp:ListItem Text="Brazil" Value="Brazil"></asp:ListItem>
                                <asp:ListItem Text="British Indian Ocean Ter" Value="British Indian Ocean Ter"></asp:ListItem>
                                <asp:ListItem Text="Brunei" Value="Brunei"></asp:ListItem>
                                <asp:ListItem Text="Bulgaria" Value="Bulgaria"></asp:ListItem>
                                <asp:ListItem Text="Burkina Faso" Value="Burkina Faso"></asp:ListItem>
                                <asp:ListItem Text="Burundi" Value="Burundi"></asp:ListItem>
                                <asp:ListItem Text="Cambodia" Value="Cambodia"></asp:ListItem>
                                <asp:ListItem Text="Cameroon" Value="Cameroon"></asp:ListItem>
                                <asp:ListItem Text="Canada" Value="Canada"></asp:ListItem>
                                <asp:ListItem Text="Canary Islands" Value="Canary Islands"></asp:ListItem>
                                <asp:ListItem Text="Cape Verde" Value="Cape Verde"></asp:ListItem>
                                <asp:ListItem Text="Cayman Islands" Value="Cayman Islands"></asp:ListItem>
                                <asp:ListItem Text="Central African Republic" Value="Central African Republic"></asp:ListItem>
                                <asp:ListItem Text="Chad" Value="Chad"></asp:ListItem>
                                <asp:ListItem Text="Channel Islands" Value="Channel Islands"></asp:ListItem>
                                <asp:ListItem Text="Chile" Value="Chile"></asp:ListItem>
                                <asp:ListItem Text="China" Value="China"></asp:ListItem>
                                <asp:ListItem Text="Christmas Island" Value="Christmas Island"></asp:ListItem>
                                <asp:ListItem Text="Cocos Island" Value="Cocos Island"></asp:ListItem>
                                <asp:ListItem Text="Colombia" Value="Colombia"></asp:ListItem>
                                <asp:ListItem Text="Comoros" Value="Comoros"></asp:ListItem>
                                <asp:ListItem Text="Congo" Value="Congo"></asp:ListItem>
                                <asp:ListItem Text="Cook Islands" Value="Cook Islands"></asp:ListItem>
                                <asp:ListItem Text="Costa Rica" Value="Costa Rica"></asp:ListItem>
                                <asp:ListItem Text="Cote D'Ivoire" Value="Cote D'Ivoire"></asp:ListItem>
                                <asp:ListItem Text="Croatia" Value="Croatia"></asp:ListItem>
                                <asp:ListItem Text="Cuba" Value="Cuba"></asp:ListItem>
                                <asp:ListItem Text="Curacao" Value="Curacao"></asp:ListItem>
                                <asp:ListItem Text="Cyprus" Value="Cyprus"></asp:ListItem>
                                <asp:ListItem Text="Czech Republic" Value="Czech Republic"></asp:ListItem>
                                <asp:ListItem Text="Denmark" Value="Denmark"></asp:ListItem>
                                <asp:ListItem Text="Djibouti" Value="Djibouti"></asp:ListItem>
                                <asp:ListItem Text="Dominica" Value="Dominica"></asp:ListItem>
                                <asp:ListItem Text="Dominican Republic" Value="Dominican Republic"></asp:ListItem>
                                <asp:ListItem Text="East Timor" Value="East Timor"></asp:ListItem>
                                <asp:ListItem Text="Ecuador" Value="Ecuador"></asp:ListItem>
                                <asp:ListItem Text="Egypt" Value="Egypt"></asp:ListItem>
                                <asp:ListItem Text="El Salvador" Value="El Salvador"></asp:ListItem>
                                <asp:ListItem Text="Equatorial Guinea" Value="Equatorial Guinea"></asp:ListItem>
                                <asp:ListItem Text="Eritrea" Value="Eritrea"></asp:ListItem>
                                <asp:ListItem Text="Estonia" Value="Estonia"></asp:ListItem>
                                <asp:ListItem Text="Ethiopia" Value="Ethiopia"></asp:ListItem>
                                <asp:ListItem Text="Falkland Islands" Value="Falkland Islands"></asp:ListItem>
                                <asp:ListItem Text="Faroe Islands" Value="Faroe Islands"></asp:ListItem>
                                <asp:ListItem Text="Fiji" Value="Fiji"></asp:ListItem>
                                <asp:ListItem Text="Finland" Value="Finland"></asp:ListItem>
                                <asp:ListItem Text="France" Value="France"></asp:ListItem>
                                <asp:ListItem Text="French Guiana" Value="French Guiana"></asp:ListItem>
                                <asp:ListItem Text="French Polynesia" Value="French Polynesia"></asp:ListItem>
                                <asp:ListItem Text="French Southern Ter" Value="French Southern Ter"></asp:ListItem>
                                <asp:ListItem Text="Gabon" Value="Gabon"></asp:ListItem>
                                <asp:ListItem Text="Gambia" Value="Gambia"></asp:ListItem>
                                <asp:ListItem Text="Georgia" Value="Georgia"></asp:ListItem>
                                <asp:ListItem Text="Germany" Value="Germany"></asp:ListItem>
                                <asp:ListItem Text="Ghana" Value="Ghana"></asp:ListItem>
                                <asp:ListItem Text="Gibraltar" Value="Gibraltar"></asp:ListItem>
                                <asp:ListItem Text="Great Britain" Value="Great Britain"></asp:ListItem>
                                <asp:ListItem Text="Greece" Value="Greece"></asp:ListItem>
                                <asp:ListItem Text="Greenland" Value="Greenland"></asp:ListItem>
                                <asp:ListItem Text="Grenada" Value="Grenada"></asp:ListItem>
                                <asp:ListItem Text="Guadeloupe" Value="Guadeloupe"></asp:ListItem>
                                <asp:ListItem Text="Guam" Value="Guam"></asp:ListItem>
                                <asp:ListItem Text="Guatemala" Value="Guatemala"></asp:ListItem>
                                <asp:ListItem Text="Guinea" Value="Guinea"></asp:ListItem>
                                <asp:ListItem Text="Guyana" Value="Guyana"></asp:ListItem>
                                <asp:ListItem Text="Haiti" Value="Haiti"></asp:ListItem>
                                <asp:ListItem Text="Hawaii" Value="Hawaii"></asp:ListItem>
                                <asp:ListItem Text="Honduras" Value="Honduras"></asp:ListItem>
                                <asp:ListItem Text="Hong Kong" Value="Hong Kong"></asp:ListItem>
                                <asp:ListItem Text="Hungary" Value="Hungary"></asp:ListItem>
                                <asp:ListItem Text="Iceland" Value="Iceland"></asp:ListItem>
                                <asp:ListItem Text="India" Value="India"></asp:ListItem>
                                <asp:ListItem Text="Indonesia" Value="Indonesia"></asp:ListItem>
                                <asp:ListItem Text="Iran" Value="Iran"></asp:ListItem>
                                <asp:ListItem Text="Iraq" Value="Iraq"></asp:ListItem>
                                <asp:ListItem Text="Ireland" Value="Ireland"></asp:ListItem>
                                <asp:ListItem Text="Isle of Man" Value="Isle of Man"></asp:ListItem>
                                <asp:ListItem Text="Israel" Value="Israel"></asp:ListItem>
                                <asp:ListItem Text="Italy" Value="Italy"></asp:ListItem>
                                <asp:ListItem Text="Jamaica" Value="Jamaica"></asp:ListItem>
                                <asp:ListItem Text="Japan" Value="Japan"></asp:ListItem>
                                <asp:ListItem Text="Jordan" Value="Jordan"></asp:ListItem>
                                <asp:ListItem Text="Kazakhstan" Value="Kazakhstan"></asp:ListItem>
                                <asp:ListItem Text="Kenya" Value="Kenya"></asp:ListItem>
                                <asp:ListItem Text="Kiribati" Value="Kiribati"></asp:ListItem>
                                <asp:ListItem Text="Korea North" Value="Korea North"></asp:ListItem>
                                <asp:ListItem Text="Korea South" Value="Korea South"></asp:ListItem>
                                <asp:ListItem Text="Kuwait" Value="Kuwait"></asp:ListItem>
                                <asp:ListItem Text="Kyrgyzstan" Value="Kyrgyzstan"></asp:ListItem>
                                <asp:ListItem Text="Laos" Value="Laos"></asp:ListItem>
                                <asp:ListItem Text="Latvia" Value="Latvia"></asp:ListItem>
                                <asp:ListItem Text="Lebanon" Value="Lebanon"></asp:ListItem>
                                <asp:ListItem Text="Lesotho" Value="Lesotho"></asp:ListItem>
                                <asp:ListItem Text="Liberia" Value="Liberia"></asp:ListItem>
                                <asp:ListItem Text="Libya" Value="Libya"></asp:ListItem>
                                <asp:ListItem Text="Liechtenstein" Value="Liechtenstein"></asp:ListItem>
                                <asp:ListItem Text="Lithuania" Value="Lithuania"></asp:ListItem>
                                <asp:ListItem Text="Luxembourg" Value="Luxembourg"></asp:ListItem>
                                <asp:ListItem Text="Macau" Value="Macau"></asp:ListItem>
                                <asp:ListItem Text="Macedonia" Value="Macedonia"></asp:ListItem>
                                <asp:ListItem Text="Madagascar" Value="Madagascar"></asp:ListItem>
                                <asp:ListItem Text="Malaysia" Value="Malaysia"></asp:ListItem>
                                <asp:ListItem Text="Malawi" Value="Malawi"></asp:ListItem>
                                <asp:ListItem Text="Maldives" Value="Maldives"></asp:ListItem>
                                <asp:ListItem Text="Mali" Value="Mali"></asp:ListItem>
                                <asp:ListItem Text="Malta" Value="Malta"></asp:ListItem>
                                <asp:ListItem Text="Marshall Islands" Value="Marshall Islands"></asp:ListItem>
                                <asp:ListItem Text="Martinique" Value="Martinique"></asp:ListItem>
                                <asp:ListItem Text="Mauritania" Value="Mauritania"></asp:ListItem>
                                <asp:ListItem Text="Mauritius" Value="Mauritius"></asp:ListItem>
                                <asp:ListItem Text="Mayotte" Value="Mayotte"></asp:ListItem>
                                <asp:ListItem Text="Mexico" Value="Mexico"></asp:ListItem>
                                <asp:ListItem Text="Midway Islands" Value="Midway Islands"></asp:ListItem>
                                <asp:ListItem Text="Moldova" Value="Moldova"></asp:ListItem>
                                <asp:ListItem Text="Monaco" Value="Monaco"></asp:ListItem>
                                <asp:ListItem Text="Mongolia" Value="Mongolia"></asp:ListItem>
                                <asp:ListItem Text="Montserrat" Value="Montserrat"></asp:ListItem>
                                <asp:ListItem Text="Morocco" Value="Morocco"></asp:ListItem>
                                <asp:ListItem Text="Mozambique" Value="Mozambique"></asp:ListItem>
                                <asp:ListItem Text="Myanmar" Value="Myanmar"></asp:ListItem>
                                <asp:ListItem Text="Nambia" Value="Nambia"></asp:ListItem>
                                <asp:ListItem Text="Nauru" Value="Nauru"></asp:ListItem>
                                <asp:ListItem Text="Nepal" Value="Nepal"></asp:ListItem>
                                <asp:ListItem Text="Netherland Antilles" Value="Netherland Antilles"></asp:ListItem>
                                <asp:ListItem Text="Netherlands (Holland, Europe)" Value="Netherlands (Holland, Europe)"></asp:ListItem>
                                <asp:ListItem Text="Nevis" Value="Nevis"></asp:ListItem>
                                <asp:ListItem Text="New Caledonia" Value="New Caledonia"></asp:ListItem>
                                <asp:ListItem Text="New Zealand" Value="New Zealand"></asp:ListItem>
                                <asp:ListItem Text="Nicaragua" Value="Nicaragua"></asp:ListItem>
                                <asp:ListItem Text="Niger" Value="Niger"></asp:ListItem>
                                <asp:ListItem Text="Nigeria" Value="Nigeria"></asp:ListItem>
                                <asp:ListItem Text="Niue" Value="Niue"></asp:ListItem>
                                <asp:ListItem Text="Norfolk Island" Value="Norfolk Island"></asp:ListItem>
                                <asp:ListItem Text="Norway" Value="Norway"></asp:ListItem>
                                <asp:ListItem Text="Oman" Value="Oman"></asp:ListItem>
                                <asp:ListItem Text="Pakistan" Value="Pakistan"></asp:ListItem>
                                <asp:ListItem Text="Palau Island" Value="Palau Island"></asp:ListItem>
                                <asp:ListItem Text="Palestine" Value="Palestine"></asp:ListItem>
                                <asp:ListItem Text="Panama" Value="Panama"></asp:ListItem>
                                <asp:ListItem Text="Papua New Guinea" Value="Papua New Guinea"></asp:ListItem>
                                <asp:ListItem Text="Paraguay" Value="Paraguay"></asp:ListItem>
                                <asp:ListItem Text="Peru" Value="Peru"></asp:ListItem>
                                <asp:ListItem Text="Philippines" Value="Philippines"></asp:ListItem>
                                <asp:ListItem Text="Pitcairn Island" Value="Pitcairn Island"></asp:ListItem>
                                <asp:ListItem Text="Poland" Value="Poland"></asp:ListItem>
                                <asp:ListItem Text="Portugal" Value="Portugal"></asp:ListItem>
                                <asp:ListItem Text="Puerto Rico" Value="Puerto Rico"></asp:ListItem>
                                <asp:ListItem Text="Qatar" Value="Qatar"></asp:ListItem>
                                <asp:ListItem Text="Republic of Montenegro" Value="Republic of Montenegro"></asp:ListItem>
                                <asp:ListItem Text="Republic of Serbia" Value="Republic of Serbia"></asp:ListItem>
                                <asp:ListItem Text="Reunion" Value="Reunion"></asp:ListItem>
                                <asp:ListItem Text="Romania" Value="Romania"></asp:ListItem>
                                <asp:ListItem Text="Russia" Value="Russia"></asp:ListItem>
                                <asp:ListItem Text="Rwanda" Value="Rwanda"></asp:ListItem>
                                <asp:ListItem Text="St Barthelemy" Value="St Barthelemy"></asp:ListItem>
                                <asp:ListItem Text="St Eustatius" Value="St Eustatius"></asp:ListItem>
                                <asp:ListItem Text="St Helena" Value="St Helena"></asp:ListItem>
                                <asp:ListItem Text="St Kitts-Nevis" Value="St Kitts-Nevis"></asp:ListItem>
                                <asp:ListItem Text="St Lucia" Value="St Lucia"></asp:ListItem>
                                <asp:ListItem Text="St Maarten" Value="St Maarten"></asp:ListItem>
                                <asp:ListItem Text="St Pierre & Miquelon" Value="St Pierre & Miquelon"></asp:ListItem>
                                <asp:ListItem Text="St Vincent & Grenadines" Value="St Vincent & Grenadines"></asp:ListItem>
                                <asp:ListItem Text="Saipan" Value="Saipan"></asp:ListItem>
                                <asp:ListItem Text="Samoa" Value="Samoa"></asp:ListItem>
                                <asp:ListItem Text="Samoa American" Value="Samoa American"></asp:ListItem>
                                <asp:ListItem Text="San Marino" Value="San Marino"></asp:ListItem>
                                <asp:ListItem Text="Sao Tome & Principe" Value="Sao Tome & Principe"></asp:ListItem>
                                <asp:ListItem Text="Saudi Arabia" Value="Saudi Arabia"></asp:ListItem>
                                <asp:ListItem Text="Senegal" Value="Senegal"></asp:ListItem>
                                <asp:ListItem Text="Serbia" Value="Serbia"></asp:ListItem>
                                <asp:ListItem Text="Seychelles" Value="Seychelles"></asp:ListItem>
                                <asp:ListItem Text="Sierra Leone" Value="Sierra Leone"></asp:ListItem>
                                <asp:ListItem Text="Singapore" Value="Singapore"></asp:ListItem>
                                <asp:ListItem Text="Slovakia" Value="Slovakia"></asp:ListItem>
                                <asp:ListItem Text="Slovenia" Value="Slovenia"></asp:ListItem>
                                <asp:ListItem Text="Solomon Islands" Value="Solomon Islands"></asp:ListItem>
                                <asp:ListItem Text="Somalia" Value="Somalia"></asp:ListItem>
                                <asp:ListItem Text="South Africa" Value="South Africa"></asp:ListItem>
                                <asp:ListItem Text="Spain" Value="Spain"></asp:ListItem>
                                <asp:ListItem Text="Sri Lanka" Value="Sri Lanka"></asp:ListItem>
                                <asp:ListItem Text="Sudan" Value="Sudan"></asp:ListItem>
                                <asp:ListItem Text="Suriname" Value="Suriname"></asp:ListItem>
                                <asp:ListItem Text="Swaziland" Value="Swaziland"></asp:ListItem>
                                <asp:ListItem Text="Sweden" Value="Sweden"></asp:ListItem>
                                <asp:ListItem Text="Switzerland" Value="Switzerland"></asp:ListItem>
                                <asp:ListItem Text="Syria" Value="Syria"></asp:ListItem>
                                <asp:ListItem Text="Tahiti" Value="Tahiti"></asp:ListItem>
                                <asp:ListItem Text="Taiwan" Value="Taiwan"></asp:ListItem>
                                <asp:ListItem Text="Tajikistan" Value="Tajikistan"></asp:ListItem>
                                <asp:ListItem Text="Tanzania" Value="Tanzania"></asp:ListItem>
                                <asp:ListItem Text="Thailand" Value="Thailand"></asp:ListItem>
                                <asp:ListItem Text="Togo" Value="Togo"></asp:ListItem>
                                <asp:ListItem Text="Tokelau" Value="Tokelau"></asp:ListItem>
                                <asp:ListItem Text="Tonga" Value="Tonga"></asp:ListItem>
                                <asp:ListItem Text="Trinidad & Tobago" Value="Trinidad & Tobago"></asp:ListItem>
                                <asp:ListItem Text="Tunisia" Value="Tunisia"></asp:ListItem>
                                <asp:ListItem Text="Turkey" Value="Turkey"></asp:ListItem>
                                <asp:ListItem Text="Turkmenistan" Value="Turkmenistan"></asp:ListItem>
                                <asp:ListItem Text="Turks & Caicos Is" Value="Turks & Caicos Is"></asp:ListItem>
                                <asp:ListItem Text="Tuvalu" Value="Tuvalu"></asp:ListItem>
                                <asp:ListItem Text="Uganda" Value="Uganda"></asp:ListItem>
                                <asp:ListItem Text="Ukraine" Value="Ukraine"></asp:ListItem>
                                <asp:ListItem Text="United Arab Emirates" Value="United Arab Emirates"></asp:ListItem>
                                <asp:ListItem Text="United Kingdom" Value="United Kingdom"></asp:ListItem>
                                <asp:ListItem Text="Uruguay" Value="Uruguay"></asp:ListItem>
                                <asp:ListItem Text="Uzbekistan" Value="Uzbekistan"></asp:ListItem>
                                <asp:ListItem Text="Vanuatu" Value="Vanuatu"></asp:ListItem>
                                <asp:ListItem Text="Vatican City State" Value="Vatican City State"></asp:ListItem>
                                <asp:ListItem Text="Venezuela" Value="Venezuela"></asp:ListItem>
                                <asp:ListItem Text="Vietnam" Value="Vietnam"></asp:ListItem>
                                <asp:ListItem Text="Virgin Islands (Brit)" Value="Virgin Islands (Brit)"></asp:ListItem>
                                <asp:ListItem Text="Virgin Islands (USA)" Value="Virgin Islands (USA)"></asp:ListItem>
                                <asp:ListItem Text="Wake Island" Value="Wake Island"></asp:ListItem>
                                <asp:ListItem Text="Wallis & Futana Is" Value="Wallis & Futana Is"></asp:ListItem>
                                <asp:ListItem Text="Yemen" Value="Yemen"></asp:ListItem>
                                <asp:ListItem Text="Zaire" Value="Zaire"></asp:ListItem>
                                <asp:ListItem Text="Zambia" Value="Zambia"></asp:ListItem>
                                <asp:ListItem Text="Zimbabwe" Value="Zimbabwe"></asp:ListItem>

                            </asp:DropDownList>
                        </div>

                        <div class="form-group">
                            <asp:DropDownList ID="ddlST_Education" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Highest Level of Education" Value="Highest Level of Education"></asp:ListItem>
                                <asp:ListItem Text="High School" Value="High School"></asp:ListItem>
                                <asp:ListItem Text="1 Year of College" Value="1 Year of College"></asp:ListItem>
                                <asp:ListItem Text="2 Years of College" Value="2 Years of College"></asp:ListItem>
                                <asp:ListItem Text="3 Years of College" Value="3 Years of College"></asp:ListItem>
                                <asp:ListItem Text="Associates" Value="Associates"></asp:ListItem>
                                <asp:ListItem Text="BSN (Nursing)" Value="BSN (Nursing)"></asp:ListItem>
                                <asp:ListItem Text="Bachelors" Value="Bachelors"></asp:ListItem>
                                <asp:ListItem Text="RN (Diploma Nurse)" Value="RN (Diploma Nurse)"></asp:ListItem>
                                <asp:ListItem Text="RN (ADN)" Value="RN (ADN)"></asp:ListItem>
                                <asp:ListItem Text="MSN (Nursing)" Value="MSN (Nursing)"></asp:ListItem>
                                <asp:ListItem Text="Some Graduate" Value="Some Graduate"></asp:ListItem>
                                <asp:ListItem Text="Masters" Value="Masters"></asp:ListItem>
                                <asp:ListItem Text="Doctorate or Equivalent" Value="Doctorate or Equivalent"></asp:ListItem>
                            </asp:DropDownList> 
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="tbST_First_Name" CssClass="form-control" runat="server" placeholder="First Name *" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tbST_First_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="tbST_Last_Name" CssClass="form-control" runat="server" placeholder="Last Name *" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tbST_Last_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="tbST_Phone" CssClass="form-control" runat="server" placeholder="Phone Number *" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tbST_Phone" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tbST_Phone" runat="server" ErrorMessage="" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="tbST_Email" CssClass="form-control" runat="server" placeholder="Email Address *" ></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="tbST_Email" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tbST_Email" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </div>
                        <div class="form-group">
                            <asp:TextBox ID="tbST_Question" runat="server" CssClass="form-control"  placeholder="Question *" TextMode="MultiLine" Rows="5"></asp:TextBox> 
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tbST_Question" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <p>By completing this form, I consent to receiving calls and/or emails from University Alliance and/or U.S. News University Connection. I understand calls may be generated using an automated technology.</p>
                        <br/>
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
