using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Security;
using Sitecore.Security.Accounts;
using Sitecore.SecurityModel;
using Sitecore.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace HealthIS.Apps.MVC.Blog.Models
{
    public class DetailPage
    {
        public bool IsExperiencePage {
            get
            {
                return Sitecore.Context.PageMode.IsExperienceEditor;
            }
        }
        public Database CurrentDb {
            get
            {
                return CurrentPage.Database;
            }
        }
        public Item CurrentPage {
            get
            {
                return Sitecore.Context.Item;
            }
        }
        public string Title
        {
            get
            {
                return FieldRenderer.Render(CurrentPage, "Title");
            }
        }
        public string Description
        {
            get
            {
                return FieldRenderer.Render(CurrentPage, "Description");
            }
        }
        public string Content
        {
            get
            {
                return FieldRenderer.Render(CurrentPage, "Content");
            }
        }
        public string ThumbnailImage
        {
            get
            {
                string parameter = "w=250&as=1";
                return FieldRenderer.Render(CurrentPage, "ThumbnailImage", parameter);
            }
        }
        public List<string> CurrentArticleCategories
        {
            get
            {
                List<string> getCategoryList = GetListOfMultilistFieldValue("Categories").Split(',').ToList();
                return getCategoryList;
            }
        }
        public List<string> CurrentArticleTags
        {
            get
            {
                List<string> getTagList = GetListOfMultilistFieldValue("Tags").Split(',').ToList();
                return getTagList;
            }
        }
        public string MetaTitle
        {
            get
            {
                return FieldRenderer.Render(CurrentPage, "Meta Title");
            }
        }
        public string MetaDescription
        {
            get
            {
                return FieldRenderer.Render(CurrentPage, "Meta Description");
            }
        }
        public string searchIndexName {
            get
            {
                return BlogSettings.SearchIndexName;
            }
        }
        public Item ArticleRootPath {
            get
            {
                return BlogSettings.ArticleRootPath;
            }
        }
        public Item CategoryRepository
        {
            get
            {
                return BlogSettings.CategoryRepository;
            }
        }
        public Item TagRepository
        {
            get
            {
                return BlogSettings.TagRepository;
            }
        }
        public Item TagTemplatePath
        {
            get
            {
                return BlogSettings.TagTemplatePath;
            }
        }
        public Item AuthorProfileLibrary
        {
            get
            {
                return BlogSettings.AuthorProfileLibrary;
            }
        }
        public Item AuthorProfileTemplate
        {
            get
            {
                return BlogSettings.AuthorProfileTemplate;
            }
        }
        public UserProfile CurrentUserProfile
        {
            get
            {
                return User.Current.Profile;
            }
        }
        public bool CurrentUserWroteThisPost
        {
            get
            {
                if (CurrentPage.Statistics.CreatedBy == CurrentUserProfile.UserName) {
                    return true;
                }
                return false;
            }
        }

        public Item GetAuthorProfile
        {
            get
            {
                try
                {
                    Item authorProfile = CurrentDb.GetItem(AuthorProfileLibrary.ID).Axes.GetDescendants().Where(c => c.Name.Equals(CurrentPage.Fields["__Created by"].Value.Replace("\\", ""))).FirstOrDefault();
                    if (authorProfile != null)
                    {
                        return authorProfile;
                    }
                }
                catch { }

                return null;
            }
        }

        public List<Item> GetRelatedPostByCategory
        {
            get
            {
                try
                {
                    Random rnd = new Random();

                    // Convert all category names
                    List<Item> getAllCategoryRelatedPosts = new List<Item>();
                    foreach (string categoryName in CurrentArticleCategories)
                    {
                        //string convertedName = categoryName.Trim().ToLower().Replace(" ", "-");
                        string convertedName = ConvertName(categoryName);
                        var relatedPosts = TaxonomyFilter.GetSearchPredicate(CurrentPage, "healthisblogcategory", convertedName)
                                    .Where(w => w != null)
                                    .OrderBy(o2 => o2.Publishing.PublishDate)
                                    .Take(4)    // Take only 4 posts
                                    .ToList();

                        getAllCategoryRelatedPosts.AddRange(relatedPosts);
                    }

                    // Get randomly chooson last 4 posts
                    return getAllCategoryRelatedPosts
                            .Where(w => w != null)
                            .Distinct()
                            .OrderBy(o => rnd.Next())
                            .Take(4)    // Take only 4 posts
                            .OrderBy(o2 => o2.Publishing.PublishDate)
                            .ToList();
                }
                catch
                {
                    return null;
                }
            }
        }

        public string GetListOfMultilistFieldValue(string fieldName)
        {
            List<string> currentArticleCategories = new List<string>();
            List<string> categoryList = CurrentPage.Fields[fieldName].Value.Split('|').ToList();

            if (String.IsNullOrEmpty(CurrentPage.Fields[fieldName].Value))
            {
                //return "No " + fieldName + " Found. Please Update " + fieldName;
                return "";
            }

            categoryList.ForEach(delegate (String catName)
            {
                string name = CurrentDb.GetItem(catName).Name.Trim();
                if (fieldName == "Categories")
                {
                    name = CurrentDb.GetItem(catName).Fields["Phrase"].Value;
                }
                currentArticleCategories.Add(name);
            });

            return currentArticleCategories.Aggregate((i, j) => i + ", " + j);
        }

        // Convert item name
        public string ConvertName(string originalValue)
        {
            string convertItemName = originalValue;
            string ex = @"[$@&+,:;/=?@#|~{}'`=+<>\[\].^*()\%!’" + "\"" + "]";
            var regexItem = new System.Text.RegularExpressions.Regex(ex);
            if (regexItem.IsMatch(convertItemName))
            {
                convertItemName = System.Text.RegularExpressions.Regex.Replace(convertItemName, ex, "");
            }
            return convertItemName;
        }

        // Update tag field
        public void UpdateTagField(string userIput, string fieldName, Item tagRepository)
        {
            List<string> newInputData = userIput.Split('|').ToList();

            if (tagRepository.HasChildren)
            {
                Item getTagRoot = Sitecore.Configuration.Factory.GetDatabase("master").SelectSingleItem(TagRepository.Paths.FullPath);
                List<Item> getDescendants = getTagRoot.Axes.GetDescendants().Where(x => x.TemplateName == "Tag").ToList();

                // Get new tag list
                List<String> matchKey = newInputData.Where(p => getDescendants.Any(p2 => p2.Name.ToLower() == ConvertName(p.Trim().ToLower()))).ToList();
                List<String> notMatchKey = newInputData.Where(p => getDescendants.Any(p2 => p2.Name.ToLower() != ConvertName(p.Trim().ToLower()))).ToList();
                List<string> allTagItemId = new List<string>();
                notMatchKey.RemoveAll(r => matchKey.Any(r2 => r == r2));

                foreach (Item i in getDescendants)
                {
                    if (matchKey.Where(m => m.ToLower().ToString().Equals(i.Name.ToLower())).Any())
                    {
                        // Ignore other duplicated item having the same name
                        if (!allTagItemId.Where(a => CurrentDb.GetItem(a).Name.ToLower().Equals(i.Name.ToLower())).Any())
                        {
                            //Sitecore.Diagnostics.Log.Info("Match: " + i, this);
                            allTagItemId.Add(i.ID.ToString());
                        }
                    }
                }

                // Create new tag item
                foreach (string s in notMatchKey)
                {
                    if (!String.IsNullOrEmpty(s))
                    {
                        //Sitecore.Diagnostics.Log.Info("Not Match: " + s, this);
                        string itemNameAndKey = ConvertName(s).ToLower();
                        TemplateItem tagTemplate = CurrentDb.GetTemplate(TagTemplatePath.ID);
                        Item newTag = tagRepository.Add(itemNameAndKey.Trim(), tagTemplate);
                        allTagItemId.Add(newTag.ID.ToString());
                    }
                }

                // List of all assigned tags
                string outputList = allTagItemId.Aggregate((i, j) => i + "|" + j);
                using (new SecurityDisabler())
                {
                    CurrentPage.Editing.BeginEdit();
                    CurrentPage.Fields[fieldName].Value = outputList;
                    CurrentPage.Editing.EndEdit();
                    CurrentPage.Editing.AcceptChanges();
                }
            }
        }

        // Data required for Open Graph Meta Tags (SEO)
        public static Tuple<string, string, string, string, string, string> GetOpenGraphMetaTagsData()
        {
            string articleUrl = "", articleTitle = "", articleDescription = "", articleImageUrl = "",  imageWidth = "", imageHeight = "";
            if (!Sitecore.Context.PageMode.IsExperienceEditorEditing)
            {
                Item currentPage = Sitecore.Context.Item;

                articleUrl = "http://health.usf.edu" + Sitecore.Links.LinkManager.GetItemUrl(currentPage);
                articleTitle = currentPage.Fields["Title"].Value;
                articleDescription = currentPage.Fields["Description"].Value;
                ImageField imageField = currentPage.Fields["ThumbnailImage"];
                if (!String.IsNullOrEmpty(imageField.Value))
                {
                    articleImageUrl = "http://health.usf.edu" + Sitecore.Resources.Media.MediaManager.GetMediaUrl(imageField.MediaItem);
                    imageWidth = imageField.Width;
                    imageHeight = imageField.Height;
                }
            }
            return Tuple.Create(articleUrl, articleTitle, articleDescription, articleImageUrl, imageWidth, imageHeight);
        }
    }
}