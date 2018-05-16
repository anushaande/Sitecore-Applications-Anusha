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
using HealthIS.Apps;

namespace ContactForm
{

    public partial class emailParameter : System.Web.UI.UserControl
    {
        private const string _myDataSourceTemplatePath = "User Defined/Sublayouts/contactformContentArea";
        private const string _myDataSourceTemporaryValue = "SublayoutContactFormItemNewDataSource";
        public string FormBuilder;

        protected void Page_PreRender(object sender, EventArgs e)
        {
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Sitecore.Context.PageMode.IsPageEditorEditing)
            {
                mvContentArea.SetActiveView(viewPageEditor);
            }
            else
            {
                Util.AddJS(this.Page, "/sublayouts/components/scripts/contactForm.js");
                mvContentArea.SetActiveView(viewNormalMode);
            }
            if (myDataSourceItem == null)
            {
                myDataSourceItem = HealthIS.Apps.Util.createDatasourceItem(_myDataSourceTemplatePath, _myDataSourceTemporaryValue, "_CF");
                if (myDataSourceItem == null)
                    return;
            }

            Sitecore.Web.UI.WebControls.Text fb = new Text();
            fb.Item = myDataSourceItem;
            fb.Field = "FormBuilder";
            FormBuilder = fb.RenderAsText();

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
                myDataSourceItem = HealthIS.Apps.Util.createDatasourceItem(_myDataSourceTemplatePath, _myDataSourceTemporaryValue, "_CF");
                if (myDataSourceItem == null)
                    return;
            }

            Sitecore.Web.UI.WebControls.Text fa = new Text();
            fa.Item = myDataSourceItem;
            fa.Field = "FromAddress";
            myEditingArea.Text += "<b>From Address: </b>" + fa.RenderAsText() + "<br />";

            Sitecore.Web.UI.WebControls.Text ta = new Text();
            ta.Item = myDataSourceItem;
            ta.Field = "ToAddress";
            myEditingArea.Text += "<b>To Address: </b>" + ta.RenderAsText() + "<br />";

            Sitecore.Web.UI.WebControls.Text sub = new Text();
            sub.Item = myDataSourceItem;
            sub.Field = "Subject";
            myEditingArea.Text += "<b>Subject: </b>" + sub.RenderAsText() + "<br />";

            Sitecore.Web.UI.WebControls.Text st = new Text();
            st.Item = myDataSourceItem;
            st.Field = "SubmitText";
            myEditingArea.Text += "<b>Submit Text: </b>" + st.RenderAsText() + "<br />";

            Sitecore.Web.UI.WebControls.Text rt = new Text();
            rt.Item = myDataSourceItem;
            rt.Field = "ResetText";
            myEditingArea.Text += "<b>Reset Text: </b>" + rt.RenderAsText() + "<br />";

            Sitecore.Web.UI.WebControls.Text msg = new Text();
            msg.Item = myDataSourceItem;
            msg.Field = "SuccessMessage";
            myEditingArea.Text += "<b>Success Message: </b>" + msg.RenderAsText() + "<br />";

            Sitecore.Web.UI.WebControls.Text bc = new Text();
            bc.Item = myDataSourceItem;
            bc.Field = "SubmitButtonClass";
            myEditingArea.Text += "<b>Submit Button Class: </b>" + bc.RenderAsText() + "<br />";

            Sitecore.Web.UI.WebControls.Text rc = new Text();
            rc.Item = myDataSourceItem;
            rc.Field = "ResetButtonClass";
            myEditingArea.Text += "<b>Reset Button Class: </b>" + rc.RenderAsText() + "<br />";

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
            myContentArea.Text += "<br/>";
            myContentArea.Text += "<input type='hidden' name='ef_datasource' value='" + myDataSourceItem.ID.ToString() + "' />";
            myContentArea.Text += "<input type='button' name='ef_clear' class='btn btn-reset " + myDataSourceItem["ResetButtonClass"].ToString() + "' value='" + myDataSourceItem["ResetText"].ToString() + "' />";
            myContentArea.Text += "<input type='button' name='ef_submit' class='btn btn-submit " + myDataSourceItem["SubmitButtonClass"].ToString() + "' value='" + myDataSourceItem["SubmitText"].ToString() + "' />";
        }
    }
}