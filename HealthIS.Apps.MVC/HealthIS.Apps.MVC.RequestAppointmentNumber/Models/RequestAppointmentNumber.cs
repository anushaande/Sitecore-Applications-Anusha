using Sitecore;
using Sitecore.Data.Items;
using System;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class RequestAppointmentNumber : Sitecore.Mvc.Presentation.RenderingModel
    {
        public bool dsSet = false;
        public string datasourcePath { get; set; }
        public bool dsEditable = false;

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
                    datasourcePath = rendering.Item.Paths.Path;
                    dsEditable = Helpers.IsDatasourceEditable(rendering.Item, PageItem.Paths.Path);
                }                
            }
            catch (Exception ex)
            {
                Util.LogError(ex.Message, ex, this);
            }
        }

        public string ApptPhoneNumber
        {
            get { return Item["Appointment Phone Number"]; }
        }

        public HtmlString GetRequestAppointmentNumber()
        {
            Item dataItem = PageItem.Database.GetItem("/sitecore/content/Data Components/Care/Clinical Request Appointment Number");
            string data = "";
            if (dataItem == null)
            {
                dataItem = PageItem.Database.GetItem(Sitecore.Data.ID.Parse("{AC570FD8-6126-4695-B77C-5FECA2E4CDFA}"));
            }
            if (dataItem != null)
            {
                data = dataItem.Fields["Data Container"].Value.Replace("{{ApptPhoneNumber}}", ApptPhoneNumber);
            }
            return new HtmlString(data);
        }
    }
}