using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sitecore.Pipelines.GetPlaceholderRenderings;
using Sitecore.Data.Items;
using Sitecore;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.WebControls;
using Sitecore.Layouts;
using Sitecore.Web.UI;
using Sitecore.Common;
using System.Web.UI;
using Sitecore.Pipelines.GetChromeData;
using Sitecore.Web.UI.PageModes;

namespace HealthIS.SitecoreExtensions
{

    public class GetDynamicKeyAllowedRenderings : GetAllowedRenderings
    {
        //text that ends in a GUID
        public const string DYNAMIC_KEY_REGEX = @"(.+){[\d\w]{8}\-([\d\w]{4}\-){3}[\d\w]{12}}";

        public new void Process(GetPlaceholderRenderingsArgs args)
        {
            Assert.IsNotNull(args, "args");

            string placeholderKey = args.PlaceholderKey;
            Regex regex = new Regex(DYNAMIC_KEY_REGEX);
            Match match = regex.Match(placeholderKey);
            if (match.Success && match.Groups.Count > 0)
            {
                placeholderKey = match.Groups[1].Value;
            }
            else
            {
                return;
            }

            // Same code as Sitecore.Pipelines.GetPlaceholderRenderings.GetAllowedRenderings but with fake placeholderKey
            Item placeholderItem = null;
            if (ID.IsNullOrEmpty(args.DeviceId))
            {
                placeholderItem = Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase, args.LayoutDefinition);
            }
            else
            {
                using (new DeviceSwitcher(args.DeviceId, args.ContentDatabase))
                {
                    placeholderItem = Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase, args.LayoutDefinition);
                }
            }

            List<Item> renderings = null;
            if (placeholderItem != null)
            {
                bool allowedControlsSpecified;
                args.HasPlaceholderSettings = true;
                renderings = this.GetRenderings(placeholderItem, out allowedControlsSpecified);
                if (allowedControlsSpecified)
                {
                    args.CustomData["allowedControlsSpecified"] = true;
                    args.Options.ShowTree = false;
                }
            }
            if (renderings != null)
            {
                if (args.PlaceholderRenderings == null)
                {
                    args.PlaceholderRenderings = new List<Item>();
                }
                args.PlaceholderRenderings.AddRange(renderings);
            }
        }
    }
/*    public class GetDynamicKeyAllowedRenderings : GetAllowedRenderings
    {
        //text that ends in a GUID
        public const string DYNAMIC_KEY_REGEX = @"(.+){[\d\w]{8}\-([\d\w]{4}\-){3}[\d\w]{12}}";

        public new void Process(GetPlaceholderRenderingsArgs args)
        {
            string placeholderKey = args.PlaceholderKey;
            var regex = new Regex(DYNAMIC_KEY_REGEX);
            var match = regex.Match(placeholderKey);
            if (match.Success && match.Groups.Count > 0)
            {
                placeholderKey = match.Groups[1].Value;
            }
            else
            {
                return;
            }

            //this taken from
            //http://stackoverflow.com/questions/15134720/sitecore-dynamic-placeholders-with-mvc

            Item placeholderItem = null;

            if (ID.IsNullOrEmpty(args.DeviceId))
            {
                placeholderItem = Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase,
                    args.LayoutDefinition);
            }
            else
            {
                using (new DeviceSwitcher(args.DeviceId, args.ContentDatabase))
                {
                    placeholderItem = Client.Page.GetPlaceholderItem(placeholderKey, args.ContentDatabase,
                    args.LayoutDefinition);
                }
            }

            List renderings = null;
            if (placeholderItem != null)
            {
                bool allowedControlsSpecified;
                args.HasPlaceholderSettings = true;
                renderings = this.GetRenderings(placeholderItem, out allowedControlsSpecified);
                if (allowedControlsSpecified)
                {
                    args.CustomData["allowedControlsSpecified"] = true;
                    args.Options.ShowTree = false; //Remove this line if using Sitecore 6.5 (see text)
                }
            }

            if (renderings != null)
            {
                if (args.PlaceholderRenderings == null)
                {
                    args.PlaceholderRenderings = new List();
                }
                args.PlaceholderRenderings.AddRange(renderings);
            }
        }
    }*/


    public class DynamicKeyPlaceholder : WebControl, IExpandable
    {
        protected string _key = Placeholder.DefaultPlaceholderKey;
        protected string _dynamicKey = null;
        protected Placeholder _placeholder;

        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value.ToLower();
            }
        }

        protected string DynamicKey
        {
            get
            {
                if (_dynamicKey != null)
                {
                    return _dynamicKey;
                }
                _dynamicKey = _key;

                //find the last placeholder processed, will help us find our parent
                Stack<Placeholder> stack = Switcher<Placeholder, PlaceholderSwitcher>.GetStack(false);
                Placeholder current = stack.Peek();

                //find the rendering reference we are contained in
                var renderings = Sitecore.Context.Page.Renderings.Where(rendering => (rendering.Placeholder == current.ContextKey || rendering.Placeholder == current.Key) && rendering.AddedToPage);
                if (renderings.Any())
                {
                    //last one added to page defines our parent
                    var thisRendering = renderings.Last();
                    _dynamicKey = _key + thisRendering.UniqueId;
                }
                return _dynamicKey;
            }
        }

        protected override void CreateChildControls()
        {
            Sitecore.Diagnostics.Tracer.Debug("DynamicKeyPlaceholder: Adding dynamic placeholder with Key " + DynamicKey);
            _placeholder = new Placeholder();
            _placeholder.Key = this.DynamicKey;
            this.Controls.Add(_placeholder);
            _placeholder.Expand();
        }

        protected override void DoRender(HtmlTextWriter output)
        {
            base.RenderChildren(output);
        }

        #region IExpandable Members

        public void Expand()
        {
            this.EnsureChildControls();
        }

        #endregion
    }

    /*public class DynamicKeyPlaceholder : WebControl, IExpandable
    {
        protected string _key = Placeholder.DefaultPlaceholderKey;
        protected string _dynamicKey = null;
        protected Placeholder _placeholder;

        public Placeholder InnerPlaceholder
        {
            get { return _placeholder; }
        }

        public string Key
        {
            get
            {
                return _key;
            }
            set
            {
                _key = value.ToLower();
            }
        }

        protected string DynamicKey
        {
            get
            {
                if (_dynamicKey != null)
                {
                    return _dynamicKey;
                }

                _dynamicKey = _key;

                //find the last placeholder processed, will help us find our parent
                var stack = Switcher<Placeholder, PlaceholderSwitcher>.GetStack(false);

                if (stack == null || stack.Count == 0)
                {
                    //not used within a placeholder apparently. dynamic key is actually not necessary in this case.
                    return _dynamicKey;
                }

                var current = stack.Peek();

                //find the rendering reference we are contained in
                var renderings = Sitecore.Context.Page.Renderings.Where(rendering => (rendering.Placeholder
                    == current.ContextKey || rendering.Placeholder == current.Key) && rendering.AddedToPage);

                var renderingReferences = renderings as IList<renderingreference>
                        ?? renderings.ToList();

                if (renderingReferences.Any())
                {
                    //last one added to page defines our parent
                    var rendering = renderingReferences.Last();

                    //use rendering reference unique ID to uniquely and permanently identify the placeholder
                    _dynamicKey = _key + rendering.UniqueId;

                }
                return _dynamicKey;
            }
        }

        protected override void CreateChildControls()
        {
            Sitecore.Diagnostics.Tracer.Debug("DynamicKeyPlaceholder: Adding dynamic placeholder with Key" + DynamicKey);

            _placeholder = new Placeholder { Key = this.DynamicKey };
            this.Controls.Add(_placeholder);

            _placeholder.Expand();
        }

        protected override void DoRender(HtmlTextWriter output)
        {
            base.RenderChildren(output);
        }

        #region IExpandable Members

        public void Expand()
        {
            this.EnsureChildControls();
        }

        #endregion
    }*/



    public class GetDynamicPlaceholderChromeData : GetPlaceholderChromeData
    {
        public override void Process(GetChromeDataArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.IsNotNull(args.ChromeData, "Chrome Data");
            if (!"placeholder".Equals(args.ChromeType, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            string placeholderKey = args.CustomData["placeHolderKey"] as string;
            Regex regex = new Regex(GetDynamicKeyAllowedRenderings.DYNAMIC_KEY_REGEX);
            Match match = regex.Match(placeholderKey);
            if (!match.Success || match.Groups.Count <= 0)
            {
                return;
            }

            string newPlaceholderKey = match.Groups[1].Value;

            // Handles replacing the displayname and description of the placeholder area to the master reference without changeing other references
            Item item = null;
            if (args.Item != null)
            {
                string layout = ChromeContext.GetLayout(args.Item);
                item = Sitecore.Client.Page.GetPlaceholderItem(newPlaceholderKey, args.Item.Database, layout);
                if (item != null)
                {
                    args.ChromeData.DisplayName = item.DisplayName;
                }
                if ((item != null) && !string.IsNullOrEmpty(item.Appearance.ShortDescription))
                {
                    args.ChromeData.ExpandedDisplayName = item.Appearance.ShortDescription;
                }
            }
        }
    }

    /*public class GetDynamicKeyPlaceholderChromeData : GetPlaceholderChromeData
    {
        public override void Process(GetChromeDataArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            Assert.IsNotNull(args.ChromeData, "Chrome Data");
            if ("placeholder".Equals(args.ChromeType, StringComparison.OrdinalIgnoreCase))
            {
                string placeholderKey = args.CustomData["placeHolderKey"] as string;
                var regex = new Regex(GetDynamicKeyAllowedRenderings.DYNAMIC_KEY_REGEX);
                var match = regex.Match(placeholderKey);
                if (match.Success && match.Groups.Count > 0)
                {
                    string newPlaceholderKey = match.Groups[1].Value;
                    args.CustomData["placeHolderKey"] = newPlaceholderKey;

                    base.Process(args);

                    args.CustomData["placeHolderKey"] = placeholderKey;
                }
                else
                {
                    base.Process(args);
                }
            }
        }
    }*/
}
