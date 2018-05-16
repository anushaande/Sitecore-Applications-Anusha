using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Web.UI.WebControls;

namespace HealthIS.Apps.MVC.Models
{
    public class ExpandableList : Sitecore.Mvc.Presentation.RenderingModel
    {
        public Item DataSourceFolder { get; set; }
        public bool dsSet = false;
        private List<string> _getIcons;

        public override void Initialize(Sitecore.Mvc.Presentation.Rendering rendering)
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
                }
            }
            catch (Exception ex)
            {
                HealthIS.Apps.Util.LogError(ex.Message, ex, this);
            }
        }        
    }
}