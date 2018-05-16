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
	

    public partial class sublayoutBootStrapSlideShow : System.Web.UI.UserControl 
	{
        private const string _myDataSourceTemplatePath = "User Defined/Sublayouts/BootstrapSlideShow";
        private const string _myImageTemplatePath = "User Defined/Sublayouts/BootstrapSlideShow/BootstrapImage";
        private const string _myDataSourceTemporaryValue = "SublayoutSlideShowNewDataSource";

        public System.Collections.Generic.List<Item> itms = null;

        private void Page_Load(object sender, EventArgs e) {
          // Put user code to initialize the page here
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
                myDataSourceItem = HealthIS.Apps.Util.createDatasourceItem(_myDataSourceTemplatePath, _myDataSourceTemporaryValue, "_SS");
                if (myDataSourceItem == null)
                    return;
            }

            Sitecore.Web.UI.WebControls.Text id = new Text();
            id.Item = myDataSourceItem;
            id.Field = "htmlID";
            editID.Text = id.RenderAsText();

            Sitecore.Web.UI.WebControls.Text s = new Text();
            s.Item = myDataSourceItem;
            s.Field = "StyleClass";
            editStyleClass.Text = s.RenderAsText();

            editTransitionTime.Value = myDataSourceItem["TransitionTime"];

            if (myDataSourceItem["PauseOnHover"] != null && myDataSourceItem["PauseOnHover"] == "1")
            {
                editPauseOnHover.Attributes.Add("checked", "checked");
            }

            string fieldID = "";
            string fieldID2 = "";
            try
            {
                fieldID = myDataSourceItem.Fields["PauseOnHover"].ID.ToShortID().ToString();
                fieldID2 = myDataSourceItem.Fields["TransitionTime"].ID.ToShortID().ToString();
            }
            catch
            {
            }
            string lang = myDataSourceItem.Language.ToString();
            string version = myDataSourceItem.Version.ToString();
            string revision = myDataSourceItem[Sitecore.FieldIDs.Revision].Replace("-", string.Empty);
            string itemID = myDataSourceItem.ID.ToShortID().ToString();
            editPauseOnHover.Attributes.Add("onclick", string.Format("var val = ''; if(this.checked) val='1';  var itemURI = new Sitecore.ItemUri('{0}','{1}','{2}','{3}');Sitecore.WebEdit.setFieldValue(itemURI,'{4}',val);", itemID, lang, version, revision, fieldID, mySublayout.ClientID.ToString()));

            editTransitionTime.Attributes.Add("onchange", string.Format("var val = this.value; var itemURI = new Sitecore.ItemUri('{0}','{1}','{2}','{3}');Sitecore.WebEdit.setFieldValue(itemURI,'{4}',val);", itemID, lang, version, revision, fieldID2, mySublayout.ClientID.ToString()));

            showImageEditor();

        }

        protected void showImageEditor()
        {
            itms = new System.Collections.Generic.List<Item>();
            if (myDataSourceItem == null)
            {
                return;
            }
            
            ImagesEditingFrame.DataSource = myDataSourceItem.Paths.FullPath;

            foreach (Item img in myDataSourceItem.Children)
            {
                if (img.TemplateName == "BootstrapImage")
                {
                    itms.Add(img);
                }
            }
            editSSImages.DataSource = itms;
            editSSImages.DataBind();
        }

        protected void DeleteButtonClicked(object send, EventArgs ea)
        {

        }

        protected void setupNormalView()
        {
            if (myDataSourceItem == null)
            {
                return;
            }

            string carID = myDataSourceItem["htmlID"];
            carouselID.Text = carID;
            carouselID2.Text = carID;
            carouselID3.Text = carID;

            TransitionTime.Text = myDataSourceItem["transitionTime"];

            PauseOnHover.Text = "";
            if (myDataSourceItem["PauseOnHover"] != null && myDataSourceItem["PauseOnHover"] == "1")
            {
                PauseOnHover.Text = "hover";
            }

            int i = 0;
            foreach(Item img in myDataSourceItem.Children){
                if (img.TemplateName == "BootstrapImage")
                {
                    string active = "";
                    string active2 = "";
                    if (i == 0)
                    {
                        active = " class=\"active\"";
                        active2 = " active";
                    }

                    Sitecore.Data.Fields.ImageField imgFld = (Sitecore.Data.Fields.ImageField)img.Fields["image"];
                    if(imgFld != null && imgFld.MediaItem != null){
                    
                        Sitecore.Data.Items.MediaItem image = new Sitecore.Data.Items.MediaItem(imgFld.MediaItem);
                        string imageURL = Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(image));

                        listIndicators.Text += "<li data-target=\"#"+carID+"\" data-slide-to=\""+i+"\""+active+"></li>";
                        imageObjects.Text += "<div class=\"item" + active2 + "\">";

                        if (!string.IsNullOrEmpty(img["link"]))
                        {
                            imageObjects.Text += "<a href='" + img["link"] + "'>";
                        }
                        
                        imageObjects.Text += "<img src=\"" + imageURL + "\" alt=\"" + img["title"] + "\">";

                        if (!string.IsNullOrEmpty(img["link"]))
                        {
                            imageObjects.Text += "</a>";
                        }

                        imageObjects.Text += "<div class=\"carousel-caption\">";
                        if (!string.IsNullOrEmpty(img["title"]))
                        {
                            imageObjects.Text += "<h3>" + img["title"] + "<h3>";
                        }
                        if (!string.IsNullOrEmpty(img["description"]))
                        {
                            imageObjects.Text += "<p>" + img["description"] + "</p>";
                        }

                        imageObjects.Text += "</div></div>";
                        i++;
                    }
                }
                
            }
        }

        /*protected void addImageButton_Click(object sender, EventArgs e)
        {
            if (myDataSourceItem == null)
            {
                return;
            }
            Item myItem = Sitecore.Context.Item;
            TemplateItem template = myItem.Database.GetTemplate(_myImageTemplatePath);
            myDataSourceItem.Add((myDataSourceItem.Children.Count + 1) + "", template);
        }*/
  }
}