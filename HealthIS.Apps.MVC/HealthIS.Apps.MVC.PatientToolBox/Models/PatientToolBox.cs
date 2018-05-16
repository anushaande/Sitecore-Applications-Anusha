using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace HealthIS.Apps.MVC.Models
{
    public class PatientToolBox : IRenderingModel
    {
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Title3 { get; set; }
        public string URL1 { get; set; }
        public string URL2 { get; set; }
        public string URL3 { get; set; }
        public Item currentItem = PageContext.Current.Item;

        public string PatientToolBoxPhoneNumber
        {
            get { return currentItem["PatientToolBox Phone Number"]; }
        }

        public void Initialize(Rendering rendering)
        {
            Title1 = currentItem["Title 1"];
            Title2 = currentItem["Title 2"];
            Title3 = currentItem["Title 3"];
            URL1 = currentItem["URL 1"];
            URL2 = currentItem["URL 2"];
            URL3 = currentItem["URL 3"];
        }
    }
}