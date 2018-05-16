<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="HealthIS.Sublayouts.Components.Grid.Left_column_sublayout" %>

<div class="row <%= ContainerClass %>" id="<%= ContainerID %>">
	<div class="col-md-3 LeftColumn <%= ItemClass %> <%= Item1Class %>" id="<%= Item1ID %>">
		<sc:placeholder runat="server" key="LeftCol"></sc:placeholder>
	</div>
	<div class="col-md-9 <%= ItemClass %> <%= Item2Class %>" id="<%= Item2ID %>">
		<sc:placeholder runat="server" key="MainContent"></sc:placeholder>
	</div>
</div>