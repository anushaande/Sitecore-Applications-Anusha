using System;
using Sitecore;

namespace Layouts.Photo_gallery {
  
	/// <summary>
	/// Summary description for Photo_gallerySublayout
	/// </summary>

 	public partial class Photo_gallerySublayout : System.Web.UI.UserControl 
	{
		// public Sitecore.Collections.ChildList images;
		private void Page_Load(object sender, EventArgs e) {
			ImageGallery.DataSource = Sitecore.Context.Database.Items["/sitecore/media library/Images/Medicine/DPT/Alumni"].GetChildren();
			ImageGallery.DataBind();

			// hello.Text = Sitecore.Context.Database.Items["/sitecore/media library/Images/Medicine/DPT/Alumni"].ID.ToString();
			// images = Sitecore.Context.Database.Items["/sitecore/media library/Images/Medicine/DPT/Alumni"].GetChildren();
	    }
    }
}