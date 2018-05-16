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


    public partial class BootStrapPhotoGallery : System.Web.UI.UserControl
    {

        private const string _myDataSourceTemplatePath = "User Defined/Sublayouts/BootstrapPhotoGallery";
        private const string _myImageTemplatePath = "User Defined/Sublayouts/BootstrapPhotoGallery/GalleryImage";
        private const string _myDataSourceTemporaryValue = "SublayoutPhotoGalleryNewDataSource";

        public System.Collections.Generic.List<Item> itms = null;

        private void Page_Load(object sender, EventArgs e)
        {
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
            if (myDataSourceItem == null)
            {
                myDataSourceItem = HealthIS.Apps.Util.createDatasourceItem(_myDataSourceTemplatePath, _myDataSourceTemporaryValue, "_PG");
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

            string lang = myDataSourceItem.Language.ToString();
            string version = myDataSourceItem.Version.ToString();
            string revision = myDataSourceItem[Sitecore.FieldIDs.Revision].Replace("-", string.Empty);
            string itemID = myDataSourceItem.ID.ToShortID().ToString();

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
                if (img.TemplateName == "GalleryImage")
                {
                    itms.Add(img);
                }
            }
            editPGImages.DataSource = itms;
            editPGImages.DataBind();
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
            foreach (Item img in myDataSourceItem.Children)
            {
                if (img.TemplateName == "GalleryImage")
                {

                    Sitecore.Data.Fields.ImageField imgFld = (Sitecore.Data.Fields.ImageField)img.Fields["image"];
                    if (imgFld != null && imgFld.MediaItem != null)
                    {

                        Sitecore.Data.Items.MediaItem image = new Sitecore.Data.Items.MediaItem(imgFld.MediaItem);

                        string title = "";
                        if (!string.IsNullOrEmpty(img["title"]))
                        {
                            title = img["title"].ToString();
                        }
                        imageObjects.Text += "<li><a class='photo-grid-item' data-toggle='lightbox' data-gallery='gallery' data-parent='.photo-grid' data-title= '" + title + "' title='" + title + "'href='" + Sitecore.StringUtil.EnsurePrefix('/', Sitecore.Resources.Media.MediaManager.GetMediaUrl(image)) + "'>"
                            + title + "</a></li>";
                    }
                }
            }
        }

    }
}