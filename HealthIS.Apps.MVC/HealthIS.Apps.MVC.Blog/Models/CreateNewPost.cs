using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC.Blog.Models
{
    public class CreateNewPost
    {
        [Required(ErrorMessage = "Please add the article title.")]
        [Display(Name="New Post Title")]
        public string Title { get; set; }
        public IEnumerable<Item> BlogArticleItems { get; set; }
        public Database MasterDb
        {
            get
            {
                return Database.GetDatabase("master");
            }
        }
        public Item ArticleRootPath
        {
            get
            {
                return BlogSettings.ArticleRootPath;
            }
        }
        public Item TemplatePath
        {
            get
            {
                return BlogSettings.TemplatePath;
            }
        }
        public string BlogTemplateName
        {
            get
            {
                return TemplatePath.Name;
            }
        }
        public User UserInformation
        {
            get
            {
                return User.Current;
            }
        }
        public string Message { get; set; }

        public IEnumerable<Item> GetCurrentUserArticles { get; set; }
        public string GetUserFullName(string username)
        {
            string fullName = User.FromName(username, false).Profile.FullName;
            if (String.IsNullOrEmpty(fullName))
            {
                fullName = username;
            }
            return fullName;
        }
        public string IsPublished(Item item) 
        {
            string getMessage = "<small class='publish-message' style='color: #FF4500;'>&#xf00d; <b>Article has not been published yet!</b></small>";
            Item itemInWeb = Sitecore.Data.Database.GetDatabase("web").GetItem(item.ID);
            if (itemInWeb != null)
            {
                getMessage = "<small class='publish-message' style='color: #00BFFF;'>&#xf0ac; <b>Published!</b></small>";
            }
            return getMessage;
        }
    }
}