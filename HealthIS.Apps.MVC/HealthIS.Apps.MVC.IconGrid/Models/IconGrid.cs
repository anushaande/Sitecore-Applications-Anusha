using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace HealthIS.Apps.MVC.Models
{
    public class IconGrid : RenderingModel
    {
        public Item DataSourceFolder { get; set; }
        public string header, headerFontSize, iconSize, columns = string.Empty;        
        public bool dsSet = false;

        public IEnumerable<SelectListItem> iconSizeOptions { get; set; }  // holds combo box items
        public string iconField = "Icon Size";  // combo box field name on template

        public IEnumerable<SelectListItem> columnOptions { get; set; }  // holds combo box items        
        public string columnField = "Columns";  // combo box field name on template        

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            try
            {
                //Set model specific objects
                Rendering = rendering;
                dsSet = !String.IsNullOrEmpty(rendering.DataSource);
                if (dsSet)
                {
                    DataSourceFolder = Sitecore.Context.Database.GetItem(rendering.DataSource);
                    if (DataSourceFolder != null)
                    {
                        header = Helpers.getStrField(Item, "Header");
                        headerFontSize = Helpers.getStrField(Item, "Header Font Size");
                    }
                    else
                    {                        
                        dsSet = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.LogError(ex.Message, ex, this);
            }
        }

        // local method to call getComboBoxItems()
        public void PopulateComboBox()
        {
            // returns combo box items as a List<SelectListItem>
            iconSizeOptions = CustomFields.Helpers.getComboBoxItems(DataSourceFolder.ID.ToString(), iconField);
            columnOptions = CustomFields.Helpers.getComboBoxItems(DataSourceFolder.ID.ToString(), columnField);
        }

        // cleanup GridItems if columns were added or removed
        public void GetGridItems(string columns)
        {
            int numGridItems = DataSourceFolder.Children.Count;
            if (numGridItems > 0 && !String.IsNullOrEmpty(columns))
            {
                Int32.TryParse(columns.ToLower(), out int cols);

                // columns were added
                if (numGridItems < cols)
                {
                    int diff = cols - numGridItems;
                    int total = diff + numGridItems;
                    if (total < 7)
                    {
                        for (int i = 0; i < diff; i++)
                        {
                            AddGridItems(numGridItems + i);                            
                        }
                    }                    
                }

                // columns were removed
                if (numGridItems > cols)
                {
                    int diff = numGridItems - cols;
                    for (int i = 1; i <= diff; i++)
                    {
                        int index = numGridItems - i;
                        DataSourceFolder.Children[index].Recycle();
                    }
                }
            }
        }

        // add Icon Grid Items to DataSourceFolder
        private void AddGridItems(int index)
        {
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                TemplateItem template = Sitecore.Context.Database.GetItem("/sitecore/templates/User Defined2/Component Template/Icon Grid/Icon Grid Item");
                string name = DataSourceFolder.Name + "I" + index.ToString();
                Item newItem = DataSourceFolder.Add(name, template);
                newItem.Editing.BeginEdit();
                try
                {
                    newItem.Fields["Title"].Value = "Icon Title";
                    newItem.Fields["Link URL"].Value = "http://health.usf.edu";                    
                    newItem.Editing.EndEdit();
                }
                catch (Exception ex)
                {
                    Util.LogError(ex.Message, ex, this);
                }
            }
        }

        // get class based on number of columns selected by user
        public string GetColumnClass(string columns)
        {
            string col = string.Empty;
            if (!String.IsNullOrEmpty(columns))
            {
                int x = 0;
                Int32.TryParse(columns.ToLower(), out x);
                switch (x)
                {
                    case 6:
                        col = "col-sm-2";
                        break;
                    case 5:
                        col = "col-sm-2a";
                        break;
                    case 4:
                        col = "col-sm-3";
                        break;
                    case 3:
                        col = "col-sm-4";
                        break;
                    case 2:
                        col = "col-sm-6";
                        break;
                    default:
                        col = "col-sm-12";
                        break;
                }
            }
            return col;
        }

        // get class based on icon size selected by user
        public string GetIconSize(string iconSize)
        {
            string sizeClass = string.Empty;
            if (!String.IsNullOrEmpty(iconSize))
            {
                switch (iconSize.ToLower())
                {
                    case "extra large":
                        sizeClass = " ico-xl";
                        break;
                    case "large":
                        sizeClass = " ico-lg";
                        break;
                    case "medium":
                        sizeClass = " ico-md";
                        break;
                    default:
                        sizeClass = " ico-sm";
                        break;
                }
            }
            return sizeClass;
        }
    }
}