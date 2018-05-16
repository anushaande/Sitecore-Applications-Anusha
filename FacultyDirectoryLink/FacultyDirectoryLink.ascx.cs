using System;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Data;
using System.Collections.Generic;
using Sitecore.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Data;
using Sitecore.Layouts;

namespace Layouts.Facultydirectorylink {
  
	/// <summary>
	/// Summary description for FacultydirectorylinkSublayout
	/// </summary>
  public partial class FacultydirectorylinkSublayout : System.Web.UI.UserControl 
	{
    private void Page_Load(object sender, EventArgs e) {
      // Put user code to initialize the page here
        
        if (Sitecore.Context.PageMode.IsPageEditorEditing)
        {
            mvFacultyLink.SetActiveView(viewEditor);
        }
        else
        {
            mvFacultyLink.SetActiveView(viewNormal);
        }

        tempThing.InnerText = "boom "+this.ID.ToString() +"<br />";
        foreach (var c in this.Parent.Controls)
        {
            tempThing.InnerText += "s1 " + c.GetType();
            if (c.GetType() == typeof(Sublayout))
            {
                var s1 = (Sublayout)c;
                tempThing.InnerText += "c1 " ;//s1.UniqueID.ToString();
            }
        }

    }
    protected void mvFacultyLink_ActiveViewChanged(object sender, EventArgs e)
		{
            switch (mvFacultyLink.ActiveViewIndex)
			{
				case 0:
					//anything else but PageEditor
					RenderNormalView();
					break;
				case 1:
					//PageEditor
					RenderConfigurationView();
					break;
				default:
					break;
			}
		}

    private void RenderNormalView()
    {
        myDisplay.Text = "Need to render the output";
    }
    private void RenderConfigurationView()
    {
        
    }
  }

}