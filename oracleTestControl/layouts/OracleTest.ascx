<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OracleTest.ascx.cs" Inherits="HealthIS.Apps.Debug.OracleTest" debug="true" %>

<style type="text/css">
    div.rbContainer
    {
        padding:1em;
        border:1px dashed #000000;
    }
</style>

<div class="rbContainer">
    <asp:Literal ID="myTestResults" runat="server" ></asp:Literal>
</div>
<br /><br />
<div class="rbContainer">
    <asp:Literal ID="mySystemPath" runat="server"></asp:Literal>
</div>

