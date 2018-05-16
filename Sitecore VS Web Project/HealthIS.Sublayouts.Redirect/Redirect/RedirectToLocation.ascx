<%@ Control Language="c#" AutoEventWireup="True" TargetSchema="http://schemas.microsoft.com/intellisense/ie5" Inherits="HealthIS.apps.SubLayouts.RedirectToLocation" Codebehind="RedirectToLocation.ascx.cs" %>



    <b>Default Location:</b><i>(Needs to be full URL including http)</i><br />
    <sc:FieldRenderer runat="server" FieldName="New Location" ID="loc" /><br />
    <b>Status Code:</b><br />
    <sc:FieldRenderer runat="server" FieldName="Status Code" ID="sc" /><br />
