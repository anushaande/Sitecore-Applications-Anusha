using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections;

namespace Layouts.Accordion {

    public partial class AccordionSublayout : System.Web.UI.UserControl 
    {

        private const string _myDataSourceTemplatePath = "User Defined/Sublayouts/Accordion";
        private const string _myDataSourceTemporaryValue = "SublayoutAccordionItemNewDataSource";

        public System.Collections.Generic.List<Item> items = null;
        private void Page_Load(object sender, EventArgs e)
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
            if (myDataSourceItem == null)
            {
                myDataSourceItem = HealthIS.Apps.Util.createDatasourceItem(_myDataSourceTemplatePath, _myDataSourceTemporaryValue, "_AC");
                if (myDataSourceItem == null)
                    return;
            }
            Sitecore.Web.UI.WebControls.Text id = new Text();
            id.Item = myDataSourceItem;
            id.Field = "ID";
            editaccID.Text = id.RenderAsText();

            Sitecore.Web.UI.WebControls.Text cl = new Text();
            cl.Item = myDataSourceItem;
            cl.Field = "Class";
            editaccClass.Text = cl.RenderAsText();

            string lang = myDataSourceItem.Language.ToString();
            string version = myDataSourceItem.Version.ToString();
            string revision = myDataSourceItem[Sitecore.FieldIDs.Revision].Replace("-", string.Empty);
            string itemID = myDataSourceItem.ID.ToShortID().ToString();

            foreach (Item itm in myDataSourceItem.Children)
            {
                showItemEditor(itm);
            }
        }

        protected void showItemEditor(Item currentItm)
        {
            items = new System.Collections.Generic.List<Item>();
            if (myDataSourceItem == null)
            {
                return;
            }


            foreach (Item itm in myDataSourceItem.Children)
            {
                items.Add(itm);
            }
            editAccordionItms.DataSource = items;
            editAccordionItms.DataBind();
        }
       
        

        protected void setupNormalView()
        {
            if (myDataSourceItem == null)
            {
                return;
            }
            foreach (Item lnk in myDataSourceItem.Children)
            {
                accordianItems.Text += "<div class= 'panel panel-default'><div class= 'panel-heading'><h4 class='panel-title'>";
                accordianItems.Text += "<a data-toggle='collapse' data-parent='"+ '#' + myDataSourceItem.Fields["ID"] +"' href='"+'#'+ lnk["Div id"].ToString() +"' class='collapsed'>" + lnk["Header"].ToString() + "</a></h4></div>";
                accordianItems.Text += "<div id= '"+ lnk["Div id"].ToString() +"' class='panel-collapse collapse'><div class='panel-body'>" + lnk["Content"].ToString() + "</div></div></div>";
            }
        }
    }
}