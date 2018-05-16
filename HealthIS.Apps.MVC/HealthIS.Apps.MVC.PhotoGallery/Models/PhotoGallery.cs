using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace HealthIS.Apps.MVC.Models
{
    public class PhotoGallery : Sitecore.Mvc.Presentation.RenderingModel
    {
        public Item DataSourceFolder { get; set; }
        public List<PhotoGallerySection> Sections { get; set; }
        public bool dsSet = false;

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
                        Sections = new List<PhotoGallerySection>();
                        foreach (Item s in DataSourceFolder.Children)
                        {
                            PhotoGallerySection section = new PhotoGallerySection(s);                            
                            Sections.Add(section);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Util.LogError(ex.Message, ex, this);
            }
        }

        public class PhotoGallerySection
        {
            public string Heading { get; set; }
            public Item SCItem { get; set; }
            public IEnumerable<SelectListItem> options { get; set; }  // holds combo box items
            public string templateFieldName = "Number of Preview Photos";  // combo box field name on template
            public PhotoGallerySection(Item scItem)
            {
                SCItem = scItem;
                Heading = Helpers.getStrField(scItem, "Heading");
            }

            // return photoset_id from Flickr album url
            public string GetPhotosetId(string albumURL)
            {
                string photoset_id = string.Empty;
                if (!String.IsNullOrEmpty(albumURL) && albumURL.StartsWith("https://www.flickr.com/photos/"))
                {
                    photoset_id = albumURL.Substring(albumURL.LastIndexOf('/') + 1);
                }
                return photoset_id;
            }

            // local method to call getComboBoxItems()
            public void PopulateComboBox()
            {
                // returns combo box items as a List<SelectListItem>
                options = CustomFields.Helpers.getComboBoxItems(SCItem.ID.ToString(), templateFieldName);
            }
        }        
    }
}