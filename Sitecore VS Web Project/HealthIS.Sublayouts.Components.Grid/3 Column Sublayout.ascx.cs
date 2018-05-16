using System;

namespace HealthIS.Sublayouts.Components.Grid 
{

	/// <summary>
	/// Summary description for 3_column_sublayoutSublayout
	/// </summary>
	public partial class ThreeColumnSublayout : System.Web.UI.UserControl 
	{

		public string ContainerClass;
		public string ContainerID;
		public string ItemClass;
		public string Item1Class;
		public string Item1ID;
		public string Item2Class;
		public string Item2ID;
		public string Item3Class;
		public string Item3ID;

		private void Page_Load(object sender, EventArgs e)
		{
			// Put user code to initialize the page here
			var sublayout = Parent as Sitecore.Web.UI.WebControls.Sublayout;
			var parameters = Sitecore.Web.WebUtil.ParseUrlParameters(sublayout.Parameters);

			ContainerClass = Server.UrlDecode(parameters.Get("ContainerClass"));
			ContainerID = Server.UrlDecode(parameters.Get("ContainerID"));
			ItemClass = Server.UrlDecode(parameters.Get("ItemClass"));
			Item1Class = Server.UrlDecode(parameters.Get("Item1Class"));
			Item1ID = Server.UrlDecode(parameters.Get("Item1ID"));
			Item2Class = Server.UrlDecode(parameters.Get("Item2Class"));
			Item2ID = Server.UrlDecode(parameters.Get("Item2ID"));
			Item3Class = Server.UrlDecode(parameters.Get("Item3Class"));
			Item3ID = Server.UrlDecode(parameters.Get("Item3ID"));
		}
	}
}