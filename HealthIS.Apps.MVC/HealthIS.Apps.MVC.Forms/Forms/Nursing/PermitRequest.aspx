<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PermitRequest.aspx.cs" Inherits="HealthIS.Apps.MVC.Forms.PermitRequest" %>

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
                <asp:Panel ID="pnlFormStart" runat="server">
                    <div class="ContactForm">
                        <asp:Panel runat="server" ID="pnlStudentInfo">
                            <p><em>Required fields denoted with an asterisk (*)</em></p>
                            <div class="form-group"> 
                                <asp:Label ID="lblTodays_Date" AssociatedControlID="tb_Todays_Date" runat="server" Text="Today's Date" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tb_Todays_Date" CssClass="form-control" runat="server" placeholder="Type today's date here"></asp:TextBox>                                                               
                            </div>
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
                                <asp:Label ID="lblUSF_ID" AssociatedControlID="tb_USF_ID" runat="server" Text="USF ID *" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tb_USF_ID" CssClass="form-control" runat="server" placeholder="U00000000 *"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ControlToValidate="tb_USF_ID" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ControlToValidate="tb_USF_ID" runat="server" ErrorMessage="" ValidationExpression="^[U]{1}[0-9]{8}$"></asp:RegularExpressionValidator>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblDaytime_Phone" AssociatedControlID="tb_Daytime_Phone" runat="server" Text="Daytime Phone" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tb_Daytime_Phone" CssClass="form-control" runat="server" placeholder="xxx-xxx-xxxx"></asp:TextBox>
                            </div>
                            <div class="form-group"> 
                                <asp:Label ID="lblEmail" AssociatedControlID="tb_Email" runat="server" Text="Email *" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tb_Email" CssClass="form-control" runat="server" placeholder="Email *"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ControlToValidate="tb_Email" runat="server" ErrorMessage=""></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tb_Email" runat="server" ErrorMessage="" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
                            </div>
                            <hr>
                            <p class="form-group"> 
                                <asp:Label ID="lblLevel_Name" AssociatedControlID="rbl_Level_Name" runat="server" Text="Current Program" CssClass="form-label"></asp:Label>
                                <asp:RadioButtonList ID="rbl_Level_Name" runat="server">
                                    <asp:ListItem Text="&nbsp;Pre-Nursing" Value="Pre-Nursing"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;Upper Division/Accelerated" Value="Upper Division/Accelerated"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;RN to BS/MS" Value="RN to BS/MS"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;Masters" Value="Masters"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;DNP" Value="DNP"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;Ph.D." Value="Ph.D."></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;Post-Masters Certificate" Value="Post-Masters Certificate"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;Non-Degree Seeking" Value="Non-Degree Seeking"></asp:ListItem>
                                </asp:RadioButtonList>
                            </p>
                            <hr>
                            <p class="form-group"> 
                                <asp:Label ID="lblGraduate_Concentration" AssociatedControlID="rbl_Graduate_Concentration" runat="server" Text="Graduate Concentration (ONLY select if you are a master's, post-master's certificate or doctoral student)" CssClass="form-label"></asp:Label>
                                <asp:RadioButtonList ID="rbl_Graduate_Concentration" runat="server">
                                    <asp:ListItem Text="&nbsp;Nursing Education" Value="Nursing Education"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;Adult-Gerontology Acute Care" Value="Adult-Gerontology Acute Care"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;Adult-Gerontology Primary Care" Value="Adult-Gerontology Primary Care"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;Pediatric Health" Value="Pediatric Health"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;Family Health" Value="Family Health"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;Occupational Health Nursing" Value="Occupational Health Nursing"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;Oncology" Value="Oncology"></asp:ListItem>
                                    <asp:ListItem Text="&nbsp;Nurse Anesthesia" Value="Nurse Anesthesia"></asp:ListItem>
                                </asp:RadioButtonList>
                            </p>
                            <hr>
                        </asp:Panel>                        
                        <asp:Panel runat="server" ID="pnlCourses">
                            <div class="form-group">
                                <table>
                                    <tr><td>Semester</td><td>Ref # (CRN)</td><td>Prefix</td><td>Number</td><td>Section</td></tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddl_Semester_1" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Fall 2017" Value="Fall 2017"></asp:ListItem>
                                                <asp:ListItem Text="Spring 2018" Value="Spring 2018"></asp:ListItem>
                                                <asp:ListItem Text="Summer 2018" Value="Summer 2018"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td><asp:TextBox ID="tb_Ref_1" CssClass="form-control" runat="server" placeholder="" MaxLength="5"></asp:TextBox></td>
                                        <td><asp:TextBox ID="tb_Prefix_1" CssClass="form-control" runat="server" placeholder="" MaxLength="3"></asp:TextBox></td>
                                        <td><asp:TextBox ID="tb_Number_1" CssClass="form-control" runat="server" placeholder="" MaxLength="4"></asp:TextBox></td>
                                        <td><asp:TextBox ID="tb_Section_1" CssClass="form-control" runat="server" placeholder="" MaxLength="3"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddl_Semester_2" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Fall 2017" Value="Fall 2017"></asp:ListItem>
                                                <asp:ListItem Text="Spring 2018" Value="Spring 2018"></asp:ListItem>
                                                <asp:ListItem Text="Summer 2018" Value="Summer 2018"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td><asp:TextBox ID="tb_Ref_2" CssClass="form-control" runat="server" placeholder="" MaxLength="5"></asp:TextBox></td>
                                        <td><asp:TextBox ID="tb_Prefix_2" CssClass="form-control" runat="server" placeholder="" MaxLength="3"></asp:TextBox></td>
                                        <td><asp:TextBox ID="tb_Number_2" CssClass="form-control" runat="server" placeholder="" MaxLength="4"></asp:TextBox></td>
                                        <td><asp:TextBox ID="tb_Section_2" CssClass="form-control" runat="server" placeholder="" MaxLength="3"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddl_Semester_3" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Fall 2017" Value="Fall 2017"></asp:ListItem>
                                                <asp:ListItem Text="Spring 2018" Value="Spring 2018"></asp:ListItem>
                                                <asp:ListItem Text="Summer 2018" Value="Summer 2018"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td><asp:TextBox ID="tb_Ref_3" CssClass="form-control" runat="server" placeholder="" MaxLength="5"></asp:TextBox></td>
                                        <td><asp:TextBox ID="tb_Prefix_3" CssClass="form-control" runat="server" placeholder="" MaxLength="3"></asp:TextBox></td>
                                        <td><asp:TextBox ID="tb_Number_3" CssClass="form-control" runat="server" placeholder="" MaxLength="4"></asp:TextBox></td>
                                        <td><asp:TextBox ID="tb_Section_3" CssClass="form-control" runat="server" placeholder="" MaxLength="3"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:DropDownList ID="ddl_Semester_4" runat="server" CssClass="form-control">
                                                <asp:ListItem Text="Fall 2017" Value="Fall 2017"></asp:ListItem>
                                                <asp:ListItem Text="Spring 2018" Value="Spring 2018"></asp:ListItem>
                                                <asp:ListItem Text="Summer 2018" Value="Summer 2018"></asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td><asp:TextBox ID="tb_Ref_4" CssClass="form-control" runat="server" placeholder="" MaxLength="5"></asp:TextBox></td>
                                        <td><asp:TextBox ID="tb_Prefix_4" CssClass="form-control" runat="server" placeholder="" MaxLength="3"></asp:TextBox></td>
                                        <td><asp:TextBox ID="tb_Number_4" CssClass="form-control" runat="server" placeholder="" MaxLength="4"></asp:TextBox></td>
                                        <td><asp:TextBox ID="tb_Section_4" CssClass="form-control" runat="server" placeholder="" MaxLength="3"></asp:TextBox></td>
                                    </tr>
                                    </table>
                            </div>
                        </asp:Panel>
                        <asp:Panel runat="server" ID="pnlComments">
                            <br />
                            <p>To expedite this request, please enter (copy & paste) the error message you received in OASIS when trying to register for the requested course(s).</p>
                            <br />
                            <div class="form-group">
                                <asp:Label ID="lblComments" AssociatedControlID="tb_Comments" runat="server" Text="Comments" CssClass="form-label"></asp:Label>
                                <asp:TextBox ID="tb_Comments" CssClass="form-control"  TextMode="MultiLine" Rows="5" runat="server" placeholder="For example: MAJOR RESTRICTION"></asp:TextBox>
                            </div>
                        </asp:Panel>
                        <input id="btnReset" class="btn btn-reset btn-usf-health bg-usf-rhubarb" onclick="location.reload(true)" type="button" value="Reset"/>
                        <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-submit btn-usf-health bg-usf-silvergray" Text="Submit" OnClick="btnSubmit_Click" />
                        <br /><br />
                        <p><a href="http://www.registrar.usf.edu/ssearch/search.php" target="_blank">Class Schedule Search</a></p>
                        <p>If you require further assistance, please call (813) 974-2191.</p>
                        <p class="error"><asp:Literal ID="litError" runat="server"></asp:Literal></p>
                    </div>
                </asp:Panel>
                <asp:Panel ID="pnlFormComplete" Visible="false" runat="server">
                    <p>This confirms your permit request was received.</p>
                    <ul>
                        <li>Permits typically take 1-2 business days to be issued.</li>
                        <li>If your permit is not granted, you will be notified via email or phone.</li>
                        <li>Completion of this form does not guarantee a permit will be issued. Permits will be issued based on completion of pre-requisites and admission criteria to the college.</li>
                        <li>Once a permit has been issued it is the student's responsibility to register for the course(s) on OASIS.</li>
                        <li>To see if your permits have been issued, go to OASIS and select Advising & Registration. Then select View My Registration Status.</li>
                    </ul>
                    <asp:Literal ID="litComplete" runat="server"></asp:Literal>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script type="text/javascript" src="/resources/forms/scripts/HealthIS.Apps.Forms.js"></script>
</body>
</html>