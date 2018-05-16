using System;

namespace HealthIS.Sublayouts.Components.Grid {

	/// <summary>
	/// Summary description for 1_column___full_pageSublayout
	/// </summary>
	public partial class FullPageSublayout : System.Web.UI.UserControl 
	{

		public string ContainerClass;
		public string ContainerID;

		private void Page_Load(object sender, EventArgs e)
		{
			// Put user code to initialize the page here
			var sublayout = Parent as Sitecore.Web.UI.WebControls.Sublayout;
			var parameters = Sitecore.Web.WebUtil.ParseUrlParameters(sublayout.Parameters);

			ContainerClass = Server.UrlDecode(parameters.Get("ContainerClass"));
			ContainerID = Server.UrlDecode(parameters.Get("ContainerID"));
		}
	}
}