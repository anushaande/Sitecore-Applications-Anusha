<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="HealthIS.Sublayouts.Components.Grid.Right_column_sublayout" %>

<div class="row <%= ContainerClass %>" id="<%= ContainerID %>">
	<div class="col-md-8 <%= ItemClass %> <%= Item1Class %>" id="<%= Item1ID %>">
		<sc:placeholder runat="server" key="MainContent"></sc:placeholder>		
	</div>
	<div class="col-md-4 rightColumn <%= ItemClass %> <%= Item2Class %>" id="<%= Item2ID %>">
		<sc:placeholder runat="server" key="RightCol"></sc:placeholder>		
	</div>
</div>