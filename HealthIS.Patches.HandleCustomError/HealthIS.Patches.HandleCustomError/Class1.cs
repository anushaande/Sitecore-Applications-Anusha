using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Pipelines.HttpRequest;
using System.Net;

namespace HealthIS.Patches
{
    public class Custom404ResolverPipeline : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            //Assert.ArgumentNotNull(args, "args");

            // Do nothing if the item is actually found
            if (Sitecore.Context.Item != null || Sitecore.Context.Database == null)
                return;

            // all the icons and media library items 
            // for the sitecore client need to be ignored
            if (args.Url.FilePath.StartsWith("/-/"))
                return;

            string destinationPath = string.Empty;
            try
            {
                List<string> arrRedirsLines = System.IO.File.ReadAllLines("/inetpub/wwwroot/Sitecore/Website/bin/301Redirects.txt").ToList<string>();
                List<DirectoryRedirect> dirRedirs = new List<DirectoryRedirect>();
                //args.Context.Response.Write("Requested URL path: " + args.Url.ItemPath);
                foreach (string redir in arrRedirsLines)
                {
                    if (redir.Contains("||") && !redir.Contains("****"))
                    {
                        if (!redir.Contains("<>"))
                        {
                            string[] splitRedir = redir.Split(new string[] { "||" }, StringSplitOptions.None);
                            string from = splitRedir[0];
                            string to = splitRedir[1];
                            if (args.Url.ItemPath.ToLower().Trim() == from.ToLower().Trim())
                            {
                                destinationPath = to;
                                break;
                            }
                        }
                        else
                        {
                            dirRedirs.Add(new DirectoryRedirect(redir));
                        }
                    }
                }
                if (string.IsNullOrEmpty(destinationPath))
                {
                    destinationPath = args.Url.ItemPath.ToLower().Trim().Replace("+", "-");
                    Sitecore.Data.Items.Item notFoundPage = null;
                    notFoundPage = Sitecore.Context.Database.GetItem(destinationPath);
                    if (notFoundPage == null)
                    {
                        var directories = dirRedirs.OrderByDescending(dr => dr.Depth);
                        int currDept = 0;
                        string iPath = cleanItemPath(args.Url.ItemPath);
                        //args.Context.Response.Write("Requested URL path: " + iPath + "<br/>");
                        foreach (var dir in directories)
                        {
                            //args.Context.Response.Write("Directory source: " + dir.Source + "<br/>");
                            //args.Context.Response.Write("Directory destination: " + dir.Destination + "<br/>");
                            //args.Context.Response.Write("Directory source depth: " + dir.Depth + "<br/>");
                            if (iPath.Contains(dir.Source))
                            {
                                string proposedNewDir = iPath.Replace(dir.Source, dir.Destination);
                                //args.Context.Response.Write("proposedNewDir: " + proposedNewDir + "<br/>");
                                if (Sitecore.Context.Database.GetItem(proposedNewDir) != null)
                                {
                                    //args.Context.Response.Write("Redirecting directory path: " + dir.Destination + "<br/>");
                                    destinationPath = proposedNewDir;
                                    break;
                                }
                                else if (Sitecore.Context.Database.GetItem(dir.Destination) != null)
                                {
                                    if (dir.Depth > currDept)
                                    {
                                        //args.Context.Response.Write("Redirecting all to one: " + dir.Destination + "<br/>");
                                        destinationPath = dir.Destination;
                                        currDept = dir.Depth;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { Sitecore.Diagnostics.Log.Error(ex.Message, ex, this); }
            // Get the 404 not found item in Sitecore.
            // You can add more complex code to get the 404 item 
            // from multisite solutions. In a production 
            // environment you would probably get the item from
            // your website configuration.

            if (string.IsNullOrEmpty(destinationPath)){ do404(); }//requested url is not on list to 301, do 404 redir
            else//requested url is on list to 301
            {
                Sitecore.Data.Items.Item notFoundPage = null;
                notFoundPage = Sitecore.Context.Database.GetItem(destinationPath);//get final destination item
                if (notFoundPage == null)//cant final destination item, check if web page
                {
                    if (RemoteFileExists(destinationPath))
                    {
                        args.Context.Response.RedirectPermanent(destinationPath, true);
                    }
                    else { do404(); }
                }
                else { args.Context.Response.RedirectPermanent(Sitecore.Links.LinkManager.GetItemUrl(notFoundPage), true); }//destination item is found, do 301 redir
            }
            
        }

        public string cleanItemPath(string itemPath)
        {
            itemPath = itemPath.ToLower().Trim();
            if (itemPath[itemPath.Length - 1] != '/')
            {
                itemPath += "/";
            }
            return itemPath;
        }

        public void do404()
        {
            Sitecore.Data.Items.Item notFoundPage = null;
            notFoundPage = Sitecore.Context.Database.GetItem("/sitecore/content/Home/PageNotFound");
            if (notFoundPage == null) return;

            // Switch to the 404 item
            Sitecore.Context.Item = notFoundPage;
        }

        /// Checks the file exists or not.
        ///
        /// The URL of the remote file.
        /// True : If the file exits, False if file not exists
        private bool RemoteFileExists(string url)
        {
            try
            {
                Uri uriResult;
                bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                              && (uriResult.Scheme == Uri.UriSchemeHttp
                                  || uriResult.Scheme == Uri.UriSchemeHttps);
                if (result)
                {
                    //Creating the HttpWebRequest
                    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    //Setting the Request method HEAD, you can also use GET too.
                    request.Method = "HEAD";
                    //Getting the Web Response.
                    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                    //Returns TRUE if the Status code == 200
                    return (response.StatusCode == HttpStatusCode.OK);
                }
                else return false;
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }
    }

    public class DirectoryRedirect
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public int Depth { get; set; }
        public DirectoryRedirect(string entry)
        {
            if (!entry.Contains("****") && entry.Contains("<>") && entry.Contains("||"))//isn't a comment and is a directory
            {
                entry = entry.Replace("<>", "");
                string[] splitEntry = entry.Split(new string[] { "||" }, StringSplitOptions.None);
                if (splitEntry.Length > 1)
                {
                    Source = splitEntry[0];
                    Depth = Source.Count(c => c == '/');
                    Destination = splitEntry[1];
                }
            }
        }
    }
}
