using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Web.DynamicData;

using Sitecore.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Layouts;


namespace HealthIS.Sublayouts
{
    public partial class contentAreaSublayout : System.Web.UI.UserControl
    {
        private const string _myDataSourceTemplatePath = "User Defined/Sublayouts/sublayoutContentArea";
        private const string _myDataSourceTemporaryValue = "SublayoutContentAreaNewDataSource";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Sitecore.Context.PageMode.IsPageEditorEditing)
            {
                mvContentArea.SetActiveView(viewPageEditor);
            }
            else
            {
                mvContentArea.SetActiveView(viewNormalMode);
            }
        }

        protected void mvContentArea_ActiveViewChanged(object sender, EventArgs e)
        {
            switch (mvContentArea.ActiveViewIndex)
            {
                case 0:
                    setupEditorView();
                    break;
                case 1:
                    setupNormalView();
                    break;
                default:
                    break;
            }
        }

        protected Sublayout mySublayout
        {
            get
            {
                return ((Sublayout)this.Parent);
            }
        }
        private Item _myDataSourceItem;
        protected Item myDataSourceItem
        {
            get
            {
                if (_myDataSourceItem == null)
                {
                    if (String.IsNullOrEmpty(mySublayout.DataSource) || mySublayout.DataSource.Equals(_myDataSourceTemporaryValue))
                    {
                        _myDataSourceItem = null;
                    }
                    else
                    {
                        try
                        {
                            _myDataSourceItem = Sitecore.Context.Database.GetItem(mySublayout.DataSource);
                        }
                        catch (Exception) { _myDataSourceItem = null; }
                    }
                }
                return _myDataSourceItem;
            }
            set
            {
                _myDataSourceItem = value;
            }
        }

        protected void setupEditorView()
        {
            //Check if the sublayout has a datasource item associated with it, if not create
            if (myDataSourceItem == null)
            {
                myDataSourceItem = HealthIS.Apps.Util.createDatasourceItem(_myDataSourceTemplatePath, _myDataSourceTemporaryValue, "_CA");
                if (myDataSourceItem == null)
                    return;

            }

            //put editor into the view
            Sitecore.Web.UI.WebControls.Text t = new Text();
            t.Item = myDataSourceItem;
            t.Field = "RichTextContent";
            myEditingArea.Text += t.RenderAsText();
        }

        protected void setupNormalView()
        {
            if (myDataSourceItem == null)
            {
                return;
            }

            myContentArea.Text += myDataSourceItem["RichTextContent"];
        }

    }
}
