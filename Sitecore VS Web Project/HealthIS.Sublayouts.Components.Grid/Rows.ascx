<%@ Control Language="c#" AutoEventWireup="true" TargetSchema="http://schemas.microsoft.com/intellisense/ie5"  Inherits="HealthIS.Sublayouts.Components.Grid.RowsSublayout" %>

<div class="<%= ContainerClass %>" <% if (!String.IsNullOrEmpty(ContainerID)) { %> id="<%= ContainerID %>" <% } %> >
    <div class="<%= Row1Class %>" id="<%= Row1ID %>" >
	    <sc:placeholder runat="server" key="GridContainer1"></sc:placeholder>
    </div>
    <div class="<%= Row2Class %>" id="<%= Row2ID %>" >
	    <sc:placeholder runat="server" key="GridContainer2"></sc:placeholder>
    </div>
    <div class="<%= Row3Class %>" id="<%= Row3ID %>" >
	    <sc:placeholder runat="server" key="GridContainer3"></sc:placeholder>
    </div>
</div>