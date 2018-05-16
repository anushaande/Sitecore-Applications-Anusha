<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="HealthIS.Sublayouts.Components.Grid.Half_and_half_sublayout" %>

<div class="row two-col-container <%= ContainerClass %>" id="<%= ContainerID %>">
	<div class="col-md-6 <%= ItemClass %> <%= Item1Class %>" id="<%= Item1ID %>">
		<sc:placeholder runat="server" key="LeftHalf"></sc:placeholder>
	</div>
	<div class="col-md-6 <%= ItemClass %> <%= Item2Class %>" id="<%= Item2ID %>">
		<sc:placeholder runat="server" key="RightHalf"></sc:placeholder>
	</div>
</div>