using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Comparers;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.Pipelines;
using Sitecore.Pipelines.ExpandInitialFieldValue;
using Sitecore.Resources;
using Sitecore.SecurityModel;
using Sitecore.Shell.Applications.Dialogs.SortContent;
using Sitecore.Text;
using Sitecore.Web;
using Sitecore.Web.UI;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Pages;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace Sitecore.Shell.Applications.Dialogs.FacDirSort
{
    public class SortForm : DialogForm
    {
        protected Scrollbox MainContainer;
        private string itemIconURL;

        // on load
        protected override void OnLoad(EventArgs e)
        {
          Assert.ArgumentNotNull((object) e, "e");
          base.OnLoad(e);
          if (Context.ClientPage.IsEvent)
            return;
          SortContentOptions sortContentOptions = SortContentOptions.Parse();
          string contentToSortQuery = sortContentOptions.ContentToSortQuery;
          Assert.IsNotNullOrEmpty(contentToSortQuery, "query");
          Item[] itemsToSort = this.GetItemsToSort(sortContentOptions.Item, contentToSortQuery);
          Array.Sort<Item>(itemsToSort, (IComparer<Item>) new DefaultComparer());
          if (itemsToSort.Length < 1) // no items to sort
          {
            this.OK.Disabled = true;
          }
          else if (itemsToSort.Length == 1 && sortContentOptions.Item.TemplateName.Equals("Faculty Directory DB Section") // item to sort = one db view item
                        && IsEditable(sortContentOptions.Item) && sortContentOptions.Item.Children.Count == 1)
          {
              Item DBViewItem = sortContentOptions.Item.Children[0];
              itemIconURL = Images.GetThemedImageSource(DBViewItem.Appearance.Icon, ImageDimension.id16x16);
              string contentToSort = string.Empty;
              if (DBViewItem.TemplateName.Equals("Faculty Directory DB View"))
              {
                  contentToSort = DBViewItem.Fields["Faculty List"].Value;
              }             
              string[] dataToSort = this.GetDataToSort(contentToSort);
              if (dataToSort.Length > 1) // at least two db entries to sort
              {
                this.MainContainer.Controls.Clear();
                this.MainContainer.InnerHtml = this.RenderData((IEnumerable<string>)dataToSort);
              }                            
          }
          else // at least two input view items to sort
          {
            this.MainContainer.Controls.Clear();
            this.MainContainer.InnerHtml = this.RenderItems((IEnumerable<Item>)itemsToSort);
          }
        }

        // on ok
        protected override void OnOK(object sender, EventArgs args)
        {
          Assert.ArgumentNotNull(sender, "sender");
          Assert.ArgumentNotNull((object) args, "args");            
          ListString listString = new ListString(WebUtil.GetFormValue("sortorder"));
          if (listString.Count == 0) // no changes made
          {
              base.OnOK(sender, args);
          }
          else
          {
            if(IsDBsort(listString.ToString()))
            {
                SortContentOptions sortContentOptions = SortContentOptions.Parse();
                ProcessDbOrder(sortContentOptions.Item.Children[0], listString); // save sort order of db view data
            }
            else // sort input view items
            {
                ListString source = listString;
                this.Sort(source.Select<string, ID>(x => ShortID.DecodeID(x)));
                SheerResponse.SetDialogValue("1");
            }
            base.OnOK(sender, args);
          }
        }

        // gets db view data
        private string[] GetDataToSort(string data)
        {
            List<string> dataList = new List<string>();
            if (!String.IsNullOrEmpty(data))
            {
                foreach (string x in data.Split('&')) { dataList.Add(x); }
            }
            return dataList.ToArray();
        }

        // render db view data
        private string RenderData(IEnumerable<string> list)
        {
            Assert.ArgumentNotNull((object)list, "list");
            HtmlTextWriter writer = new HtmlTextWriter((TextWriter)new StringWriter());
            string[] info;
            writer.Write("<div id='dbv'>"); // needed for javascript to differentiate between db view and input view sorting
            writer.Write("<ul id='sort-list'>");
            foreach (string fac in list)
            {
                info = fac.Split('=');
                string jobTitle = string.Empty;
                if (!info[1].Equals(";dump;") && !info[1].Equals("%3Bdump%3B")) { jobTitle = info[1].Replace("'", ""); }
                writer.Write("<li id='{0}' class='sort-item editable' title='{1}'>", info[0], jobTitle);
                writer.Write("<img src='/sitecore/shell/Themes/Standard/Images/draghandle9x15.png' class='drag-handle' />");
                writer.Write("<img src='{0}' class='item-icon' />", itemIconURL);
                writer.Write("<span unselectable='on' class='item-name'>{0}</span>", "Person ID: " + info[0]);
                writer.Write("</li>");
            }
            writer.Write("</ul></div>");
            return writer.InnerWriter.ToString();
        }

        // checks to see if user is sorting db view or input view
        private bool IsDBsort(string id)
        {
            bool digitsOnly = false;
            if (!String.IsNullOrEmpty(id))
            {
                string x = id.Split('|')[0];
                digitsOnly = x.All(char.IsDigit);
            }
            return digitsOnly;
        }
        
        // saves order of db view data
        private void ProcessDbOrder(Item dbViewItem, ListString list)
        {
            string newOrder = string.Empty;
            string[] facList = dbViewItem.Fields["Faculty List"].Value.Split('&');
            Dictionary<string, string> originalOrder = facList.ToDictionary(item => item.Split('=')[0], item => item.Split('=')[1]);
            string lastSequence = list.ToString().Split(',').Last();
            string[] personIDs = lastSequence.Split('|');
            foreach (string p in personIDs)
            {
                string myJobTitle = originalOrder[p];
                if (String.IsNullOrEmpty(newOrder)) { newOrder += p + "=" + myJobTitle; }
                else
                {
                    newOrder += "&" + p + "=" + myJobTitle;
                }
            }
            dbViewItem.Editing.BeginEdit();
            try
            {
                dbViewItem.Fields["Faculty List"].Value = newOrder;
                dbViewItem.Editing.EndEdit();
            }
            catch (Exception) { dbViewItem.Editing.CancelEdit(); }
            SheerResponse.Eval("window.top.location.reload();");
        }
        
        // renders input view items
        private string RenderItems(IEnumerable<Item> items)
        {
          Assert.ArgumentNotNull((object) items, "items");
          HtmlTextWriter writer = new HtmlTextWriter((TextWriter) new StringWriter());
          writer.Write("<ul id='sort-list'>");
          foreach (Item obj in items)
          {
              bool flag = IsEditable(obj);
              string sortBy = obj.Fields["First Name"].Value + " " + obj.Fields["Last Name"].Value;
              string jobTitle = obj.Fields["Job Title"].Value.Replace("'", "");
              string str = !flag ? Translate.Text("You cannot edit this item because you do not have write access to it.") : sortBy;
              writer.Write("<li id='{0}' class='sort-item {1}' title='{2}'>", (object)obj.ID.ToShortID(), flag ? (object)"editable" : (object)"non-editable", (object)jobTitle);
              writer.Write("<img src='/sitecore/shell/Themes/Standard/Images/draghandle9x15.png' class='drag-handle' />");
              writer.Write("<img src='{0}' class='item-icon' />", (object)Images.GetThemedImageSource(obj.Appearance.Icon, ImageDimension.id16x16));
              writer.Write("<span unselectable='on' class='item-name'>{0}</span>", "Name: " + (object)StringUtil.Clip(sortBy, 40, true));
              writer.Write("</li>");
            }
          writer.Write("</ul>");
          return writer.InnerWriter.ToString();
        }

        // sorts the specified order of input view items      
        private void Sort(IEnumerable<ID> orderList)
        {
          Assert.ArgumentNotNull((object) orderList, "orderList");
          SortContentOptions sortContentOptions = SortContentOptions.Parse();
          int defaultSortOrder = Settings.DefaultSortOrder;
          Item[] itemsToSort = this.GetItemsToSort(sortContentOptions.Item, sortContentOptions.ContentToSortQuery);
          foreach (ID order in orderList)
          {
            Item obj = Array.Find<Item>(itemsToSort, x => x.ID == order);
            if (obj != null)
            {
              using (new SecurityDisabler())
              {
                using (new EditContext(obj, false, false))
                  obj.Appearance.Sortorder = defaultSortOrder;
              }
              defaultSortOrder += 100;
            }
          }
        }

        // gets input view items
        private Item[] GetItemsToSort(Item item, string query)
        {
          Assert.ArgumentNotNull((object) item, "item");
          Assert.IsNotNullOrEmpty(query, "query");
          Item[] objArray;
          try
          {
            using (new LanguageSwitcher(item.Language))
              objArray = !query.StartsWith("fast:") ? item.Axes.SelectItems(query) ?? new Item[0] : item.Database.SelectItems(query.Substring(5)) ?? new Item[0];
          }
          catch (Exception ex)
          {
            objArray = new Item[0];
            Log.Error("Failed to execute query:" + query, ex, (object) this);
          }
          return objArray;
        }

        // determines whether specified item is editable
        private static bool IsEditable(Item item)
        {
          Assert.IsNotNull((object) item, "item");
          if (!Context.IsAdministrator && item.Locking.IsLocked() && !item.Locking.HasLock() || item.Appearance.ReadOnly)
            return false;
          return item.Access.CanWrite();
        }        
    }
}