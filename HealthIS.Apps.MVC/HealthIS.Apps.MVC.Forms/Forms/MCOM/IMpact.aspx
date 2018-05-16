<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="IMpact.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.IMpact" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>IMpact</title>
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
                <p><b>Disclaimer:</b> IMpact's aim is to assist authors in the preparation of scientific manuscripts to make them suitable for submission to the peer-reviewed journal of their choice. IMpact does not assume responsibility for confirming IRB approval or author contribution. A positive review from our editors is not a guarantee that a manuscript will be published in a peer-reviewed journal.</p>
                <p>After submitting, please contact Jennifer Newcomb at <a href="mailto:jnewcomb@health.usf.edu">jnewcomb@health.usf.edu</a> or <a href="tel:(813) 495-2945" onclick="_gaq.push(['_trackEvent','Phone Call Tracking','Click/Touch','Flyout']);">(813) 495-2945</a> to confirm your submission was received.</p>
                <hr />
                <asp:Panel ID="pnlFormStart" runat="server">
                    <div class="ContactForm">
                        <h4><asp:Label runat="server" CssClass="form-label" Text="Author Information" AssociatedControlID="pnlAuth"></asp:Label></h4>
                            <asp:Panel runat="server" ID="pnlAuth">
                            <div class="form-group">
                                <asp:Label ID="lblAuth_Type" AssociatedControlID="ddlAuth_Type" runat="server" Text="I am *" CssClass="form-label"></asp:Label>
                                <asp:DropDownList ID="ddlAuth_Type" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Faculty" Value="Faculty"></asp:ListItem>
                                    <asp:ListItem Text="Fellow" Value="Fellow"></asp:ListItem>
                                    <asp:ListItem Text="Medical student" Value="Medical student"></asp:ListItem>
                                    <asp:ListItem Text="PhD candidate" Value="PhD candidate"></asp:ListItem>
                                    <asp:ListItem Text="Post Doc" Value="Post Doc"></asp:ListItem>
                                    <asp:ListItem Text="Resident" Value="Resident"></asp:ListItem>
                                    <asp:ListItem Text="Staff" Value="Staff"></asp:ListItem>
                                </asp:DropDownList>                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="ddlAuth_Type" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblAuth_Name" AssociatedControlID="tbAuth_Name" runat="server" Text="Name *" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tbAuth_Name" CssClass="form-control" runat="server" placeholder="Name *"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tbAuth_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblAuth_Degree" AssociatedControlID="tbAuth_Degree" runat="server" Text="Degree *" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tbAuth_Degree" CssClass="form-control" runat="server" placeholder="Degree *"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tbAuth_Degree" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblAuth_Email" AssociatedControlID="tbAuth_Email" runat="server" Text="Email *" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tbAuth_Email" CssClass="form-control" runat="server" placeholder="Email *"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tbAuth_Email" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tbAuth_Email" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblAuth_Phone" AssociatedControlID="tbAuth_Phone" runat="server" Text="Phone *" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tbAuth_Phone" CssClass="form-control" runat="server" placeholder="Phone *"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="tbAuth_Phone" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tbAuth_Phone" runat="server" ErrorMessage="" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblAuth_Alt_Phone" AssociatedControlID="tbAuth_Alt_Phone" runat="server" Text="Alternate Phone" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tbAuth_Alt_Phone" CssClass="form-control" runat="server" placeholder="Alternate Phone"></asp:TextBox>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblAuth_Campus_Address" AssociatedControlID="tbAuth_Campus_Address" runat="server" Text="Campus Address" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tbAuth_Campus_Address" TextMode="MultiLine" Rows="4" CssClass="form-control" runat="server" placeholder="Campus Address"></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <asp:Label ID="lblAuth_Dept_Affiliation" AssociatedControlID="ddlAuth_Dept_Affiliation" runat="server" Text="What department are you affiliated with? *" CssClass="form-label"></asp:Label>
                                <asp:DropDownList ID="ddlAuth_Dept_Affiliation" runat="server" CssClass="form-control">
                                    <asp:ListItem Text="" Value=""></asp:ListItem>
                                    <asp:ListItem Text="Cardiology" Value="Cardiology"></asp:ListItem>
                                    <asp:ListItem Text="Dermatology" Value="Dermatology"></asp:ListItem>
                                    <asp:ListItem Text="Family Medicine" Value="Family Medicine"></asp:ListItem>
                                    <asp:ListItem Text="Internal Medicine" Value="Internal Medicine"></asp:ListItem>
                                    <asp:ListItem Text="Molecular Medicine" Value="Molecular Medicine"></asp:ListItem>
                                    <asp:ListItem Text="Molecular Pharmacology & Physiology" Value="Molecular Pharmacology & Physiology"></asp:ListItem>
                                    <asp:ListItem Text="Neurology" Value="Neurology"></asp:ListItem>
                                    <asp:ListItem Text="Neurosurgery & Brain Repair" Value="Neurosurgery & Brain Repair"></asp:ListItem>
                                    <asp:ListItem Text="Obstetrics & Gynecology" Value="Obstetrics & Gynecology"></asp:ListItem>
                                    <asp:ListItem Text="Oncological Sciences" Value="Oncological Sciences"></asp:ListItem>
                                    <asp:ListItem Text="Ophthalmology" Value="Ophthalmology"></asp:ListItem>
                                    <asp:ListItem Text="Orthopaedics & Sports Medicine" Value="Orthopaedics & Sports Medicine"></asp:ListItem>
                                    <asp:ListItem Text="Otolaryngology – Head & Neck Surgery" Value="Otolaryngology – Head & Neck Surgery"></asp:ListItem>
                                    <asp:ListItem Text="Pathology & Cell Biology" Value="Pathology & Cell Biology"></asp:ListItem>
                                    <asp:ListItem Text="Pediatrics" Value="Pediatrics"></asp:ListItem>
                                    <asp:ListItem Text="Psychiatry & Behavioral Neuroscience" Value="Psychiatry & Behavioral Neuroscience"></asp:ListItem>
                                    <asp:ListItem Text="Radiology" Value="Radiology"></asp:ListItem>
                                    <asp:ListItem Text="Surgery" Value="Surgery"></asp:ListItem>
                                    <asp:ListItem Text="Urology" Value="Urology"></asp:ListItem>
                                </asp:DropDownList>                
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="ddlAuth_Dept_Affiliation" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator><br />
                                <asp:Label ID="lblSubspecialty_Divison" AssociatedControlID="tbAuth_Subspecialty_Division" runat="server" Text="Subspecialty or Division" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tbAuth_Subspecialty_Division" CssClass="form-control" runat="server" placeholder="If applicable, please specify a Subspecialty or Division here."></asp:TextBox>
                            </div>
                            <div class="form-group">
                                <p><b>What journal do you plan to submit your manuscript to?</b></p>
                                <p>Please follow each peer-reviewed journal's submission process and instructions. Our editors will not review the criteria for submission.</p>
                                <asp:Label ID="lblAuth_Journal_First_Choice" AssociatedControlID="tbAuth_Journal_First_Choice" runat="server" Text="First Choice" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tbAuth_Journal_First_Choice" CssClass="form-control" runat="server" placeholder="First Choice"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblAuth_Journal_Second_Choice" AssociatedControlID="tbAuth_Journal_Second_Choice" runat="server" Text="Second Choice" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tbAuth_Journal_Second_Choice" CssClass="form-control" runat="server" placeholder="Second Choice"></asp:TextBox>
                                <br />
                                <asp:Label ID="lblAuth_Journal_Third_Choice" AssociatedControlID="tbAuth_Journal_Third_Choice" runat="server" Text="Third Choice" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tbAuth_Journal_Third_Choice" CssClass="form-control" runat="server" placeholder="Third Choice"></asp:TextBox>
                                <br />
                            </div>
                        </asp:Panel>
                        <hr />
                        <div class="form-group">
                            <h4><asp:Label ID="lbl_Manuscript_Type" AssociatedControlID="ddl_Manuscript_Type" runat="server" Text="Manuscript Type *" CssClass="form-label"></asp:Label></h4>
                            <asp:DropDownList ID="ddl_Manuscript_Type" runat="server" CssClass="form-control">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Abstract" Value="Abstract"></asp:ListItem>
                                <asp:ListItem Text="Book Chapter" Value="Book Chapter"></asp:ListItem>
                                <asp:ListItem Text="Brief Communication" Value="Brief Communication"></asp:ListItem>
                                <asp:ListItem Text="Clinical Case Report" Value="Clinical Case Report"></asp:ListItem>
                                <asp:ListItem Text="Original investigation" Value="Original investigation"></asp:ListItem>
                                <asp:ListItem Text="Scientific Review or Meta-analysis" Value="Scientific Review or Meta-analysis"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                            </asp:DropDownList>                
                            <asp:TextBox ID="tb_Other_Manuscript_Type" runat="server" CssClass="form-control" placeholder="If you selected Other, please type your Manuscript Type here."></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="ddl_Manuscript_Type" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator>
                        </div>
                        <hr />
                        <div class="form-group"> 
                            <h4><asp:Label ID="lbl_Title" AssociatedControlID="tb_Title" runat="server" Text="Title *" CssClass="form-label"></asp:Label></h4>
                            <asp:TextBox ID="tb_Title" CssClass="form-control" runat="server" placeholder="Title *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="tb_Title" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </div>
                        <hr />
                        <div class="form-group"> 
                            <h4><asp:Label ID="lbl_Abstract" AssociatedControlID="tb_Abstract" runat="server" Text="Abstract" CssClass="form-label"></asp:Label></h4>
                            <asp:TextBox ID="tb_Abstract" CssClass="form-control"  TextMode="MultiLine" Rows="5" runat="server" placeholder="Abstract"></asp:TextBox>
                        </div>
                        <hr />
                        <div class="form-group">
                            <h4><asp:Label ID="lbl_IRB_Approved" AssociatedControlID="ddl_IRB_Approved" runat="server" Text="Is this work IRB approved?" CssClass="form-label"></asp:Label></h4>
                            <asp:DropDownList ID="ddl_IRB_Approved" runat="server" CssClass="form-control">
                                <asp:ListItem Text="" Value=""></asp:ListItem>
                                <asp:ListItem Text="Yes" Value="Yes"></asp:ListItem>
                                <asp:ListItem Text="No" Value="No"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:Label ID="lbl_Not_IRB_Approved" AssociatedControlID="tb_Not_IRB_Approved" runat="server" Text="If No; Explain" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Not_IRB_Approved" CssClass="form-control"  TextMode="MultiLine" Rows="5" runat="server" placeholder="Explanation"></asp:TextBox>                
                        </div>
                        <hr />
                        <div class="form-group">
                            <h4><asp:Label runat="server" CssClass="form-label" Text="Authors and Institutions" AssociatedControlID="pnlAuthors"></asp:Label></h4>
                            <asp:UpdatePanel ID="udpAuthors" runat="server" UpdateMode="Conditional" EnableViewState="true">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlAuthors" runat="server">
                                        <asp:TextBox ID="tb_Author_Institution_1" CssClass="form-control" runat="server" placeholder="Author/Institution"></asp:TextBox>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel><br />
                            <asp:Button ID="btnAddAuthors" type="button" runat="server" Text="Add another Author/Institution" OnClick="btnAddAuthors_Click" CssClass="btn btn-usf-health bg-usf-silvergray" CausesValidation="false" />
                        </div>
                        <hr />
                        <div class="form-group">
                            <h4><asp:Label runat="server" CssClass="form-label" Text="Attach Files" AssociatedControlID="fu_File"></asp:Label></h4>
                            <asp:UpdatePanel ID="udpFiles" runat="server" UpdateMode="Conditional" EnableViewState="true">
                                <ContentTemplate>
                                    <asp:Panel ID="pnlFiles" runat="server">
                                            <asp:Label ID="lblFiles" runat="server" Text=""></asp:Label><br />
                                            <asp:FileUpload ID="fu_File" CssClass="form-control" AllowMultiple="true" runat="server" />
                                    </asp:Panel><br />
                                    <asp:Button ID="btnFU_Upload" runat="server" Text="Upload Files" CssClass="btn btn-usf-health bg-usf-silvergray" OnClick="btnFU_Upload_Click" CausesValidation="false" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:PostBackTrigger ControlID="btnFU_Upload" />
                                </Triggers>
                            </asp:UpdatePanel>
                        </div><br />
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
