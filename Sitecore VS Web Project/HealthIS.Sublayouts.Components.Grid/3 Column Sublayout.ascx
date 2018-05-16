<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="HealthIS.Sublayouts.Components.Grid.ThreeColumnSublayout" %>
<div class="row <%= ContainerClass %>" id="<%= ContainerID %>">
	<div class="col-md-3 leftColumn <%= ItemClass %> <%= Item1Class %>" id="<%= Item1ID %>">
		<sc:placeholder runat="server" key="LeftCol"></sc:placeholder>	
	</div>
	<div class="col-md-6 <%= ItemClass %> <%= Item2Class %>" id="<%= Item2ID %>">
		<sc:placeholder runat="server" key="MainContent"></sc:placeholder>	
	</div>
	<div class="col-md-3 rightColumn <%= ItemClass %> <%= Item3Class %>" id="<%= Item3ID %>">
		<sc:placeholder runat="server" key="RightCol"></sc:placeholder>	
	</div>
</div>