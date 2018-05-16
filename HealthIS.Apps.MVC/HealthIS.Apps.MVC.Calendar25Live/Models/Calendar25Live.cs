using Sitecore.Mvc.Presentation;
using System;

namespace HealthIS.Apps.MVC.Models
{
    public class Calendar25Live : RenderingModel
    {
        public string Header, Spud = string.Empty;
        public bool dsSet { get; set; }        

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
                    Header = Helpers.getStrField(Item, "Header");
                    Spud = Helpers.getStrField(Item, "Spud");
                }
            }
            catch (Exception ex)
            {
                Util.LogError(ex.Message, ex, this);
            }
        }

        // check to see if input has valid parameters needed to render a 25Live calendar
        public bool IsValidSpud()
        {
            if (!String.IsNullOrEmpty(Spud))
            {
                if (Spud.Contains("webName") && Spud.Contains("spudType")) { return true; }
                else { return false; }
            }
            else { return true; }
        }
    }
}