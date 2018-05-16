<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LibraryPurchase.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.LibraryPurchase" %>

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
                    <div class="lib-purchase-form">
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
                            <asp:Label ID="lblEmail_Address" AssociatedControlID="tb_Email_Address" runat="server" Text="Email *" /><br />
                            <asp:TextBox ID="tb_Email_Address" runat="server" CssClass="form-control" placeholder="someone@email.com"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_Email_Address" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group"> 
                            <asp:Label ID="lblPhone" AssociatedControlID="tb_Phone" runat="server" Text="Phone" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Phone" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                        </p>
                        <p class="form-group"> 
                            <asp:Label ID="lblDepartment" AssociatedControlID="tb_Department" runat="server" Text="Department" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Department" CssClass="form-control" runat="server" placeholder="Department"></asp:TextBox>
                        </p>
                        <p class="form-group"> 
                            <asp:Label ID="lblReserve_For_Course_Num" AssociatedControlID="tb_Reserve_For_Course_Num" runat="server" Text="Please place on reserve for course #" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Reserve_For_Course_Num" CssClass="form-control" runat="server" placeholder=""></asp:TextBox>
                        </p>
                        <h4>Item Information</h4>
                        <p>Please provide as much information as possible regarding the item you are recommending.</p>
                        <p class="form-group">
                            <asp:Label ID="lblFormat" AssociatedControlID="ddl_Format" runat="server" Text="Format *" />
                             <asp:DropDownList ID="ddl_Format" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="no selection made"></asp:ListItem>
                                <asp:ListItem Text="Book" Value="Book"></asp:ListItem>
                                <asp:ListItem Text="Journal Subscription" Value="Journal Subscription"></asp:ListItem>
                                <asp:ListItem Text="Database" Value="Database"></asp:ListItem>
                                <asp:ListItem Text="DVD" Value="DVD"></asp:ListItem>                 
                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="ddl_Format" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblTitle" AssociatedControlID="tb_Title" runat="server" Text="Title" /><br />
                            <asp:TextBox ID="tb_Title" runat="server" CssClass="form-control" placeholder="Title"></asp:TextBox>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblAuthor" AssociatedControlID="tb_Author" runat="server" Text="Author/Editor/Producer" /><br />
                            <asp:TextBox ID="tb_Author" runat="server" CssClass="form-control" placeholder="Author/Editor/Producer"></asp:TextBox>
                        </p> 
                        <p class="form-group">
                            <asp:Label ID="lblEdition" AssociatedControlID="tb_Edition" runat="server" Text="Edition" /><br />
                            <asp:TextBox ID="tb_Edition" runat="server" CssClass="form-control" placeholder="Edition"></asp:TextBox>
                        </p> 
                        <p class="form-group">
                            <asp:Label ID="lblPubDate" AssociatedControlID="tb_PubDate" runat="server" Text="Date of Publication" /><br />
                            <asp:TextBox ID="tb_PubDate" runat="server" CssClass="form-control" placeholder="Date of Publication"></asp:TextBox>
                        </p> 
                        <p class="form-group">
                            <asp:Label ID="lblISBN_ISSN_URL" AssociatedControlID="tb_ISBN_ISSN_URL" runat="server" Text="ISBN/ISSN/URL" /><br />
                            <asp:TextBox ID="tb_ISBN_ISSN_URL" runat="server" CssClass="form-control" placeholder="ISBN/ISSN/URL"></asp:TextBox>
                        </p> 
                        <p class="form-group">
                            <asp:Label ID="lblComments" AssociatedControlID="tb_Comments" runat="server" Text="Other information or commments" /><br />
                            <asp:TextBox ID="tb_Comments" runat="server" CssClass="form-control" placeholder="Please enter here." TextMode="MultiLine" Rows="5"></asp:TextBox>
                        </p>
                        <em><p>If the item you have recommended is purchased, you will be notified when it is available. Thank you for your suggestion!</p></em>
                        <input id="btnReset" class="btn btn-reset btn-usf-health bg-usf-rhubarb" onclick="location.reload(true)" type="button" value="Reset"/>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn-usf-health bg-usf-silvergray" Text="Submit" OnClick="btnSubmit_Click" />
                        <p class="error"><asp:Literal ID="litError" runat="server"></asp:Literal></p>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFormComplete" Visible="false" runat="server">
                    <p>Thank you. Your suggestion has been sent successfully.</p>
                    <asp:Literal ID="litComplete" runat="server"></asp:Literal>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="/resources/forms/scripts/HealthIS.Apps.Forms.js"></script>
</body>
</html>