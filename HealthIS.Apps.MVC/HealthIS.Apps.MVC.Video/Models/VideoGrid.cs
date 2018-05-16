using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class VideoGrid : Sitecore.Mvc.Presentation.RenderingModel
    {
        public bool dsIsSet { get; set; }
        public string dataSourceID { get; set; }
        public Item dataSourceFolder { get; set; }
        public List<VideoGridSection> sections = new List<VideoGridSection>();

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            dsIsSet = !String.IsNullOrEmpty(rendering.DataSource);
            if (dsIsSet)
            {
                try
                {
                    dataSourceFolder = Sitecore.Context.Database.GetItem(rendering.DataSource);

                    if (dataSourceFolder == null)
                    {
                        dataSourceID = "";
                        dsIsSet = false;
                    }
                    else
                    {
                        dataSourceID = dataSourceFolder.ID.Guid.ToString();
                        //add items to list
                        foreach (Item i in dataSourceFolder.Children)
                        {
                            try
                            {
                                VideoGridSection s = new VideoGridSection(i);
                                if (s.SCItem != null) { sections.Add(s); }
                            }
                            catch { }
                        }
                        dsIsSet = true;
                    }
                }
                catch { dataSourceID = ""; }
            }
        }

        private string youtubeEmbedURL = "https://www.youtube.com/embed/";
        private string youtubeOEmbedURL = "https://www.youtube.com/oembed?url=https://www.youtube.com/watch?v=";
        private string vimeoURL = "https://player.vimeo.com/video/";
        private string panoptoURL = "/panopto/pages/embed.aspx?id=";
        public bool IsValidVideoLink(string url)
        {
            if(!String.IsNullOrEmpty(url))
            {
                try
                {
                    if (url.StartsWith(youtubeEmbedURL))
                    {
                        string vidID = url.Replace(youtubeEmbedURL,"");
                        string oembedURL = youtubeOEmbedURL + vidID + "&format=json";

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(oembedURL);
                        request.Method = "HEAD";
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            return response.ResponseUri.ToString().StartsWith(youtubeOEmbedURL) ? true : false;
                        }
                    }
                    else if (url.StartsWith(vimeoURL) || url.Contains(panoptoURL))
                    {
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "HEAD";
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            if(url.StartsWith(vimeoURL))
                            {
                                return response.ResponseUri.ToString().StartsWith(vimeoURL) ? true : false;
                            }
                            else 
                            {
                                return response.ResponseUri.ToString().Contains(panoptoURL) ? true : false;
                            }
                        }
                    }
                }
                catch { return false; }
            }
            return false;
        }  
    }

    public class VideoGridItem
    {
        public Item SCItem { get; set; }
        public Sitecore.Data.Fields.ImageField image { get; set; }
        public string URL { get; set; }

        public VideoGridItem(Item _scItem)
        {
            if (_scItem != null)
            {
                SCItem = _scItem;
                image = Helpers.getImgField(_scItem, "Thumbnail");
                URL = Helpers.getStrField(_scItem, "URL");
            }
        }
    }

    public class VideoGridSection
    {
        public List<VideoGridItem> videos = new List<VideoGridItem>();
        public Item SCItem { get; set; }
        public string header = "";
        public int columnCount = -1;

        public VideoGridSection(Item _scItem)
        {
            if (_scItem != null)
            {
                header = Helpers.getStrField(_scItem, "Header");
                columnCount = Helpers.getIntField(_scItem, "Column Count");
                SCItem = _scItem;
                foreach (Item i in _scItem.Children)
                {
                    try
                    {
                        VideoGridItem v = new VideoGridItem(i);
                        if (v.SCItem != null) { videos.Add(v); }
                    }
                    catch { }
                }
            }
        }
    }

    public class VimeoVid
    {
        public int id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string upload_date { get; set; }
        public string thumbnail_small { get; set; }
        public string thumbnail_medium { get; set; }
        public string thumbnail_large { get; set; }
        public int user_id { get; set; }
        public string user_name { get; set; }
        public string user_url { get; set; }
        public string user_portrait_small { get; set; }
        public string user_portrait_medium { get; set; }
        public string user_portrait_large { get; set; }
        public string user_portrait_huge { get; set; }
        public int stats_number_of_likes { get; set; }
        public int stats_number_of_plays { get; set; }
        public int stats_number_of_comments { get; set; }
        public int duration { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string tags { get; set; }
        public string embed_privacy { get; set; }
    }
}