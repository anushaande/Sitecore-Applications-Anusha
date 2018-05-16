<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ScheduleLibClass.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.ScheduleLibClass" %>

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
            $("#tb_Date_Preference_1").datepicker();
            $("#tb_Date_Preference_2").datepicker();
        });
    </script>
    <!-- end datetime picker -->
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
                    <div class="schedule-class-form">
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
                            <asp:Label ID="lblPhone" AssociatedControlID="tb_Phone" runat="server" Text="Phone *" /><br />
                            <asp:TextBox ID="tb_Phone" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_Phone" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tb_Phone" runat="server" ErrorMessage="" ValidationExpression="\(?\d{3}\)?-? *\d{3}-? *-?\d{4}"></asp:RegularExpressionValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblPreferred_Contact_Method" AssociatedControlID="ddl_Preferred_Contact_Method" runat="server" Text="Preferred method of contact to schedule the class *" />
                             <asp:DropDownList ID="ddl_Preferred_Contact_Method" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="no selection made"></asp:ListItem>
                                <asp:ListItem Text="Email" Value="Email"></asp:ListItem>
                                <asp:ListItem Text="Phone" Value="Phone"></asp:ListItem>                                
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ControlToValidate="ddl_Preferred_Contact_Method" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblUSF_Status" AssociatedControlID="ddl_USF_Status" runat="server" Text="USF Status *" />
                             <asp:DropDownList ID="ddl_USF_Status" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="no selection made"></asp:ListItem>
                                <asp:ListItem Text="Student" Value="Student"></asp:ListItem>
                                <asp:ListItem Text="Staff" Value="Staff"></asp:ListItem>
                                <asp:ListItem Text="Faculty" Value="Faculty"></asp:ListItem>
                                <asp:ListItem Text="Resident/Fellow" Value="Resident/Fellow"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ControlToValidate="ddl_USF_Status" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblAffiliation" AssociatedControlID="ddl_Affiliation" runat="server" Text="Affiliation *" />
                             <asp:DropDownList ID="ddl_Affiliation" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="no selection made"></asp:ListItem>
                                <asp:ListItem Text="College of Medicine" Value="College of Medicine"></asp:ListItem>
                                <asp:ListItem Text="College of Nursing" Value="College of Nursing"></asp:ListItem>
                                <asp:ListItem Text="College of Pharmacy" Value="College of Pharmacy"></asp:ListItem>
                                <asp:ListItem Text="College of Public Health" Value="College of Public Health"></asp:ListItem>
                                <asp:ListItem Text="School of Physical Therapy" Value="School of Physical Therapy"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ControlToValidate="ddl_Affiliation" runat="server" ErrorMessage="" InitialValue=""></asp:RequiredFieldValidator>
                        </p>
                        <p class="form-group">
                            <asp:Label ID="lblClass" AssociatedControlID="ddl_Class" runat="server" Text="Class" />
                             <asp:DropDownList ID="ddl_Class" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select" Value="no selection made"></asp:ListItem>
                                <asp:ListItem Text="CINAHL" Value="CINAHL"></asp:ListItem>
                                <asp:ListItem Text="EndNote" Value="EndNote"></asp:ListItem>
                                <asp:ListItem Text="PubMed" Value="PubMed"></asp:ListItem>
                                <asp:ListItem Text="Shimberg Library 101" Value="Shimberg Library 101"></asp:ListItem>
                                <asp:ListItem Text="Web of Science" Value="Web of Science"></asp:ListItem>
                                <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                            </asp:DropDownList>
                        </p>
                        <p class="form-group"> 
                            <asp:Label ID="lblNum_of_Students" AssociatedControlID="tb_Num_of_Students" runat="server" Text="Number of Students" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Num_of_Students" CssClass="form-control" runat="server" placeholder="(minimum 3)"></asp:TextBox>
                        </p>
                        <p><strong>Note:</strong> Date/Time availability is Monday - Friday between the hours of 9:00 AM - 4:00 PM.</p>
                        <p class="form-group"> 
                            <asp:Label ID="lblDateTime_Pref_1" AssociatedControlID="tb_Date_Preference_1" runat="server" Text="Date/Time 1st Preference" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Date_Preference_1" runat="server" CssClass="form-control" placeholder="Click here to select date"></asp:TextBox>
                            <asp:DropDownList ID="ddl_Time_Preference_1" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select time" Value="not specified"></asp:ListItem>
                                <asp:ListItem Text="9 AM" Value="9 AM"></asp:ListItem>
                                <asp:ListItem Text="9:30 AM" Value="9:30 AM"></asp:ListItem>
                                <asp:ListItem Text="10 AM" Value="10 AM"></asp:ListItem>
                                <asp:ListItem Text="10:30 AM" Value="10:30 AM"></asp:ListItem>
                                <asp:ListItem Text="11 AM" Value="11 AM"></asp:ListItem>
                                <asp:ListItem Text="11:30 AM" Value="11:30 AM"></asp:ListItem>
                                <asp:ListItem Text="12 PM" Value="12 PM"></asp:ListItem>
                                <asp:ListItem Text="12:30 PM" Value="12:30 PM"></asp:ListItem>
                                <asp:ListItem Text="1 PM" Value="1 PM"></asp:ListItem>
                                <asp:ListItem Text="1:30 PM" Value="1:30 PM"></asp:ListItem>
                                <asp:ListItem Text="2 PM" Value="2 PM"></asp:ListItem>
                                <asp:ListItem Text="2:30 PM" Value="2:30 PM"></asp:ListItem>
                                <asp:ListItem Text="3 PM" Value="3 PM"></asp:ListItem>
                                <asp:ListItem Text="3:30 PM" Value="3:30 PM"></asp:ListItem>
                            </asp:DropDownList>
                        </p>
                        <p class="form-group"> 
                            <asp:Label ID="lblDateTime_Pref_2" AssociatedControlID="tb_Date_Preference_2" runat="server" Text="Date/Time 2nd Preference" CssClass="form-label"></asp:Label>
                            <asp:TextBox ID="tb_Date_Preference_2" runat="server" CssClass="form-control" placeholder="Click here to select date"></asp:TextBox>
                            <asp:DropDownList ID="ddl_Time_Preference_2" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Select time" Value="not specified"></asp:ListItem>
                                <asp:ListItem Text="9 AM" Value="9 AM"></asp:ListItem>
                                <asp:ListItem Text="9:30 AM" Value="9:30 AM"></asp:ListItem>
                                <asp:ListItem Text="10 AM" Value="10 AM"></asp:ListItem>
                                <asp:ListItem Text="10:30 AM" Value="10:30 AM"></asp:ListItem>
                                <asp:ListItem Text="11 AM" Value="11 AM"></asp:ListItem>
                                <asp:ListItem Text="11:30 AM" Value="11:30 AM"></asp:ListItem>
                                <asp:ListItem Text="12 PM" Value="12 PM"></asp:ListItem>
                                <asp:ListItem Text="12:30 PM" Value="12:30 PM"></asp:ListItem>
                                <asp:ListItem Text="1 PM" Value="1 PM"></asp:ListItem>
                                <asp:ListItem Text="1:30 PM" Value="1:30 PM"></asp:ListItem>
                                <asp:ListItem Text="2 PM" Value="2 PM"></asp:ListItem>
                                <asp:ListItem Text="2:30 PM" Value="2:30 PM"></asp:ListItem>
                                <asp:ListItem Text="3 PM" Value="3 PM"></asp:ListItem>
                                <asp:ListItem Text="3:30 PM" Value="3:30 PM"></asp:ListItem>
                            </asp:DropDownList>
                        </p>
                        <p>We'll get back to you within two (2) business days.</p><br />
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