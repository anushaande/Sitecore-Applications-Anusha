<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DocumentDelivery.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.DocumentDelivery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="/layouts/styles/health.css"/>
    <script type="text/javascript" src="//ajax.googleapis.com/ajax/libs/jquery/1.10.2/jquery.min.js"></script>
    <script type="text/javascript" src="/resources/scripts/HealthIS.Apps.Util.js"></script>
    <!-- start datetime picker -->
    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css"/>
    <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>
    <script>
        $(function () {
            $("#tb_PubDate").datepicker();
        });
    </script>
    <!-- end datetime picker -->
    <style>
        .form-control-invalid {
            border-color:red;
        }
        .warning {
            color:red;
            font-weight:bold;
        }
    </style>
</head>
<body class="form-body">    
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:Panel CssClass="ContactForm" ID="pnlFormStart" runat="server">
                    <div class="doc-delivery-form">
                        <p class="form-group"> 
                            <asp:Label ID="lblFirst_Name" AssociatedControlID="tb_First_Name" runat="server" Text="First Name *" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_First_Name" CssClass="form-control" runat="server" placeholder="First Name *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="tb_First_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group"> 
                            <asp:Label ID="lblLast_Name" AssociatedControlID="tb_Last_Name" runat="server" Text="Last Name *" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Last_Name" CssClass="form-control" runat="server" placeholder="Last Name *"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ControlToValidate="tb_Last_Name" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblStreet_Address" AssociatedControlID="tb_Street_Address" runat="server" Text="Street Address *" /><br />
                            <asp:TextBox ID="tb_Street_Address" runat="server" CssClass="form-control" placeholder="Street Address"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_Street_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblAddress_Line_2" AssociatedControlID="tb_Address_Line_2" runat="server" Text="Address Line 2" /><br />
                            <asp:TextBox ID="tb_Address_Line_2" runat="server" CssClass="form-control" placeholder="Address Line 2"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblCity" AssociatedControlID="tb_City" runat="server" Text="City *" /><br />
                            <asp:TextBox ID="tb_City" runat="server" CssClass="form-control" placeholder="City"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_City" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblState" AssociatedControlID="tb_State" runat="server" Text="State / Province / Region *" /><br />
                            <asp:TextBox ID="tb_State" runat="server" CssClass="form-control" placeholder="State / Province / Region"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="tb_State" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lbl_Zip_Code" AssociatedControlID="tb_Zip_Code" runat="server" Text="Postal/Zip Code *" /><br />
                            <asp:TextBox ID="tb_Zip_Code" runat="server" CssClass="form-control" placeholder="Postal/Zip Code"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="tb_Zip_Code" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblEmail_Address" AssociatedControlID="tb_Email_Address" runat="server" Text="Email *" /><br />
                            <asp:TextBox ID="tb_Email_Address" runat="server" CssClass="form-control" placeholder="someone@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </p>      
                        <p class="form-group">
                            <asp:Label ID="lblPhone" AssociatedControlID="tb_Phone" runat="server" Text="Phone *" /><br />
                            <asp:TextBox ID="tb_Phone" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ControlToValidate="tb_Phone" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tb_Phone" runat="server" ErrorMessage="" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblFax" AssociatedControlID="tb_Fax" runat="server" Text="Fax" /><br />
                            <asp:TextBox ID="tb_Fax" runat="server" CssClass="form-control" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                        </p>
                        <div class="form-group">
                            <asp:Label ID="lblDelivery_Method" AssociatedControlID="ddl_Delivery_Method" runat="server" Text="Delivery Method" />
                             <asp:DropDownList ID="ddl_Delivery_Method" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="no selection made"></asp:ListItem>
                                <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                                <asp:ListItem Text="Mail" Value="Mail"></asp:ListItem>
                                <asp:ListItem Text="Pickup in person" Value="Pickup in person"></asp:ListItem>
                                <asp:ListItem Text="Fax (Poor Quality)" Value="Fax"></asp:ListItem>
                                <asp:ListItem Text="Call when ready" Value="Call when ready"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <p class="form-group">
                            <asp:Label ID="lblAuthorized_By" AssociatedControlID="tb_Authorized_By" runat="server" Text="Authorized By" /><br />
                            <asp:TextBox ID="tb_Authorized_By" runat="server" CssClass="form-control" placeholder="Name"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblJournal_Book_Title" AssociatedControlID="tb_Journal_Book_Title" runat="server" Text="Title of journal or book" /><br />
                            <asp:TextBox ID="tb_Journal_Book_Title" runat="server" CssClass="form-control" placeholder="Title"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblVol_Num" AssociatedControlID="tb_Vol_Num" runat="server" Text="Volume number" /><br />
                            <asp:TextBox ID="tb_Vol_Num" runat="server" CssClass="form-control" placeholder="Volume number"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblIssue_Num" AssociatedControlID="tb_Issue_Num" runat="server" Text="Issue number" /><br />
                            <asp:TextBox ID="tb_Issue_Num" runat="server" CssClass="form-control" placeholder="Issue number"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblPage_Num" AssociatedControlID="tb_Page_Num" runat="server" Text="Page number(s)" /><br />
                            <asp:TextBox ID="tb_Page_Num" runat="server" CssClass="form-control" placeholder="Page number(s)"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblPubDate" AssociatedControlID="tb_PubDate" runat="server" Text="Date of publication" /><br />
                            <asp:TextBox ID="tb_PubDate" runat="server" CssClass="form-control" placeholder=""></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblAuthor" AssociatedControlID="tb_Author" runat="server" Text="Author(s) of book or chapter" /><br />
                            <asp:TextBox ID="tb_Author" runat="server" CssClass="form-control" placeholder="Author(s)"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblArticle_Chapter_Title" AssociatedControlID="tb_Article_Chapter_Title" runat="server" Text="Title of article or book chapter" /><br />
                            <asp:TextBox ID="tb_Article_Chapter_Title" runat="server" CssClass="form-control" placeholder="Title"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblUI" AssociatedControlID="tb_UI" runat="server" Text="Unique Identifier/UI" /><br />
                            <asp:TextBox ID="tb_UI" runat="server" CssClass="form-control" placeholder="Unique Identifier/UI"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblComments" AssociatedControlID="tb_Comments" runat="server" Text="Additional information or commments *" /><br />
                            <asp:TextBox ID="tb_Comments" runat="server" CssClass="form-control" placeholder="If none, type N/A." TextMode="MultiLine" Rows="5"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ControlToValidate="tb_Comments" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>                        
                        </p>
                        <p>** Note that we cannot provide entire books, only selected pages (at most, one chapter). **</p>
                        <p><strong>NOTICE:</strong> COPYRIGHT RESTRICTIONS<br />This reproduction requested is to be for study, scholarship, criticism, or research and will not be sold, further reproduced, or used for any other purpose.</p>
                        <p class="form-group">
                            <asp:Label ID="lblCopyright_Compliance" AssociatedControlID="rbl_Copyright_Compliance" runat="server" Text="Request complies with 108(g)(2)Guidelines (CCG) or other provisions of copyright law (CCL)" />
                            <asp:RadioButtonList ID="rbl_Copyright_Compliance" runat="server">
                                <asp:ListItem Text="CCG" Value="CCG"></asp:ListItem>
                                <asp:ListItem Text="CCL" Value="CCL"></asp:ListItem>
                            </asp:RadioButtonList>            
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ControlToValidate="rbl_Copyright_Compliance" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator> 
                        </p>
                        <h4>Fees</h4>
                        <p>$11.00 per article (+tax)<br />$3.00 surcharge for fax or rush requests</p>
                        <p><em>By clicking on the Submit button below, you agree to accept the terms of copyright law as stated above.</em></p>
                        <p><span class="warning">Warning:</span> There may be charges associated with your request. Make sure that cookies are enabled on your machine.</p>
                        <input id="btnReset" class="btn btn-reset btn-usf-health bg-usf-rhubarb" onclick="location.reload(true)" type="button" value="Reset"/>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn-usf-health bg-usf-silvergray" Text="Submit" OnClick="btnSubmit_Click" />
                        <p class="error"><asp:Literal ID="litError" runat="server"></asp:Literal></p>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFormComplete" Visible="false" runat="server">
                    <p>Thank you, your request has been sent successfully.</p>
                    <asp:Literal ID="litComplete" runat="server"></asp:Literal>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="/resources/forms/scripts/HealthIS.Apps.Forms.js"></script>
</body>
</html>