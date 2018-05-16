using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Web;
using System.Collections.Generic;
using Sitecore.SecurityModel;
using Sitecore.Security.Accounts;
using Sitecore.Data;
using Sitecore.Web.UI.WebControls;
using Sitecore.Data.Fields;
using System.Linq;

namespace HealthIS.Apps.MVC.Models
{
    public class HorizontalNavigation
    {
        // NavBarHorizontal Template
        public string H_NavBarClass { get; set; }
        public string H_LabelH1Class { get; set; }
        public string H_LabelIClass { get; set; }
        public string H_NavBarLabel { get; set; }
        public string H_UlClass { get; set; }
        public int ListCount { get; set; }
        public bool isDatasourceSet { get; set; }
        public Item datasourceItem { get; set; }
        public string NewTab { get; set; }
        public List<HtmlString> LiList { get; set; }
        public HtmlString HsTag { get; set; }
        public Sitecore.Mvc.Presentation.Rendering Rendering { get; set; }
        public Item Item { get; set; }
        public Item PageItem { get; set; }
        public Database masterDb { get; set; }
        public Item newAddedItem { get; set; }
        public string newAddedItemPopUp { get; set; }

        // Open pop-up to update item's properties
        public HtmlString addEditing(Sitecore.Data.ID itemId, string text = "Edit", string newStyle = "")
        {
            return new HtmlString("<span style='float: right; padding-right: 15px; " + newStyle + "'><a name='edit_" + itemId + "' href='#' class='pe-button pe-yellow' onclick='Sitecore.PageModes.PageEditor.postRequest(\"webedit:fieldeditor(command={11111111-1111-1111-1111-111111111111}, fields=Nav Bar Class|Nav Bar Label|Label h1 Class|Label i Class|ul Class|Title|Template Type|URL|openInNewTab|li Class|a Class|Inner Div Class|Outer Div Class|Menu Class|Menu Group Class|li Id, id=" + itemId + ")\");' id='edit_" + itemId + "'>" + text + "</a></span>");
        }

        // delete item
        public HtmlString deleteItem(Sitecore.Data.ID itemId)
        {
            return new HtmlString("<span style='float: right; padding-right: 15px;'><a name='edit_" + itemId + "' href='#delete' class='pe-button pe-red' onclick='Sitecore.PageModes.PageEditor.postRequest(\"webedit:delete(id=" + itemId.ToString() + ")\"); alert(\"Once you confirm item deletion, please refresh the page.\");' id='edit_" + itemId + "'>&#120;</a></span>");
        }

        // Sort items
        public HtmlString sortingItems(Sitecore.Data.ID itemId, string text)
        {
            return new HtmlString("<span style='float: right; padding-right: 15px;'><a href='#' class='pe-button pe-gray1 fa-input' onclick='javascript:Sitecore.PageModes.PageEditor.postRequest(\"webedit:sortcontent(id=" + itemId.ToString() + ")\");'>&#xf0c9; " + text + "</a></span>");
        }

        // Remove special character and rename Item
        public HtmlString renameItem(Item item)
        {
            try
            {
                if (!String.IsNullOrEmpty(item.Fields["Title"].Value))
                {
                    if (item.Name != item.Fields["Title"].Value)
                    {
                        string ex = @"[$@&+,:;/=?@#|~{}'`=+<>\[\].^*()\%!’" + "\"" + "]";
                        string title = item.Fields["Title"].Value;
                        string updatedName = title;
                        var regexItem = new System.Text.RegularExpressions.Regex(ex);
                        if (regexItem.IsMatch(title))
                        {
                            updatedName = System.Text.RegularExpressions.Regex.Replace(title, ex, "");
                        }
                        if (item.Name != updatedName)
                        {
                            using (new SecurityDisabler())
                            {
                                item.Editing.BeginEdit();
                                item.Name = updatedName;
                                item.Editing.EndEdit();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Warn("Horizonal Nav Error: " + e.Message, this);
            }

            return new HtmlString("");
        }

        // if rendered item's template name doesn't match to the selected value in Template type drop-list, update to new template type
        public void updateTemplate(Item item, string changedValue)
        {
            item.Editing.BeginEdit();

            if (String.IsNullOrEmpty(item.Fields["Template Type"].Value))
            {
                item.Fields["Template Type"].Value = item.TemplateName;
            }
            else
            {
                if (changedValue == "NavBarLinkItem")
                {
                    var template = Sitecore.Context.Database.Templates["User Defined2/Component Template/NavBarHorizontal/NavBarLinkItem"];
                    item.ChangeTemplate(template);
                    item.DeleteChildren();
                }
                else if (changedValue == "NavBarVerticalItem")
                {
                    var template = Sitecore.Context.Database.Templates["User Defined2/Component Template/NavBarHorizontal/NavBarVerticalItem"];
                    item.ChangeTemplate(template);
                    item.DeleteChildren();
                }
                else if (changedValue == "NavBarSectionItem")
                {
                    var template = Sitecore.Context.Database.Templates["User Defined2/Component Template/NavBarHorizontal/NavBarSectionItem"];
                    item.ChangeTemplate(template);
                    item.DeleteChildren();
                }
            }
            item.Editing.EndEdit();
        }

        // checks user roles to see if they're allowed to edit HN root-element
        public bool UserHasEditPermissions (User user)
        {
            /* users allowed:
               anyone NOT in SC_Role_Author and NOT in SC_Role_Contributor roles, with the exception of UCM authors and contributors (SC_Doctors-MFM_Content group)
            */
            if ((!user.IsInRole("hscnet\\SC_Role_Author") && !user.IsInRole("hscnet\\SC_Role_Contributor")) || 
               ((user.IsInRole("hscnet\\SC_Role_Author") || user.IsInRole("hscnet\\SC_Role_Contributor")) && user.IsInRole("hscnet\\SC_Doctors-MFM_Content")))
            {
                return true;
            }
            return false;
        }
    }
}