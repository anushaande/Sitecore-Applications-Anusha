<%@ Control Language="C#" AutoEventWireup="True" Inherits="HealthIS.apps.SubLayouts.Redirect_for_profileSublayout" debug="true" Codebehind="redirect for cosmetics profile.ascx.cs" %>



    <b>Default Location:</b><i>(Needs to be full URL including http)</i><br />
    <sc:FieldRenderer runat="server" FieldName="New Default Location" ID="loc" /><br />
    <b>Status Code:</b><br />
    <sc:FieldRenderer runat="server" FieldName="Status Code" id="sc" /><br />

    <b>Query String Variable:</b><br />
    <sc:FieldRenderer runat="server" FieldName="queryStringVariable" ID="QueryStringField" /><br />
    <b>New Base Location:</b><br />
    <sc:FieldRenderer runat="server" FieldName="New Base Location" id="NewBaseLocation" /><br />