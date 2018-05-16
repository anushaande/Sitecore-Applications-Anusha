using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;

namespace HealthIS.Apps.MVC.Models
{
    public class AZDirectory : Sitecore.Mvc.Presentation.RenderingModel
    {
        public Item DataSourceFolder { get; set; }
        public List<AZDirectoryItem> Directory = new List<AZDirectoryItem>();
        public bool dsSet = false;
        public string Style = "";

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
                    if (DataSourceFolder == null)
                    {
                        dsSet = false;
                    }
                    else
                    {
                        Style = Helpers.getStrField(DataSourceFolder, "Style");
                    }
                    if (DataSourceFolder.HasChildren)
                    {
                        foreach (Item i in DataSourceFolder.Children)
                        {
                            Directory.Add(new AZDirectoryItem(i));
                        }
                        Directory = Directory.OrderBy(a => a.LinkText).ToList<AZDirectoryItem>();
                    }
                }
            }
            catch (Exception ex)
            {
                HealthIS.Apps.Util.LogError(ex.Message, ex, this);
            }
        }
    }

    public class AZDirectoryItem
    {
        public string LinkText { get; set; }
        public string LinkURL { get; set; }
        public Item SCItem { get; set; }
        public bool isSet { get; set; }

        public AZDirectoryItem(Item sci)
        {
            LinkText = Helpers.getStrField(sci, "Link Text");
            LinkURL = Helpers.getStrField(sci, "Link URL");
            SCItem = sci;
            isSet = !String.IsNullOrEmpty(LinkText) || !String.IsNullOrEmpty(LinkURL);
        }
    }
}