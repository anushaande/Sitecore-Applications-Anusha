using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Globalization;
using Sitecore.SecurityModel;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Specialized;

namespace HealthIS.Apps
{
    class VNavAddToLocation : Sitecore.Shell.Framework.Commands.Command
    {
        /// <summary>
        /// Main execution point of the WebEditCommand
        /// </summary>
        public override void Execute(CommandContext context)
        {
            Assert.ArgumentNotNull((object)context, "context");
            NameValueCollection parameters = new NameValueCollection();

            // Get Language
            var itemUri = ItemUri.ParseQueryString();
            var language = context.Parameters["language"];

            if (String.IsNullOrEmpty(language) && itemUri != null)
            {
                language = itemUri.Language.ToString();
            }

            // Store the custom parameters
            parameters["language"] = language;
            parameters["parentId"] = context.Parameters["parentId"];
            parameters["templateId"] = context.Parameters["templateId"];
            parameters["fieldId"] = context.Parameters["fieldId"] ?? String.Empty;

            Context.ClientPage.Start((object)this, "Run", parameters);
        }

        /// <summary>
        /// Handles the Postback of the Sheer Dialogs
        /// </summary>
        protected static void Run(ClientPipelineArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            var language = Language.Parse(args.Parameters["language"]);
            Item parent = Context.ContentDatabase.Items[args.Parameters["parentId"], language];
            Assert.IsNotNull((object)parent, typeof(Item));

            // Retrieval of custom parameters
            ID templateId = VNavAddToLocation.GetSitecoreId(args.Parameters, "templateId");
            ID fieldId = VNavAddToLocation.GetSitecoreId(args.Parameters, "fieldId");

            Assert.IsNotNull((object)templateId, typeof(ID));
            
            if (!parent.Access.CanCreate())
            {
                Context.ClientPage.ClientResponse.Alert("You do not have permission to create an item here.");
            }
            else
            {
                var tid = new TemplateID(templateId);
                Assert.IsNotNull((object)tid, typeof(TemplateID));
                SheerResponse.Eval("console.log('TID: " + tid.ID + "');");
                Item item = parent.Add("Vertical Nav Item", tid);
                Context.ClientPage.SendMessage(Context.ClientPage, "webedit:fieldeditor(command={11111111-1111-1111-1111-111111111111}, fields=Title|Link, id=" + item.ID + ")");
                //SheerResponse.Alert("Page will now reload");
            }
        }

        /// <summary>
        /// Gets a Sitecore Id from a NameValueCollection
        /// </summary>
        /// <param name="collection">Collection of Parameters</param>
        /// <param name="parameter">Parameter Key</param>
        /// <param name="defaultValue">Default value if no parameter under that key found. Default
        /// value is also returned if the value cannot be parsed into a </param>
        /// <returns>Sitecore Id</returns>
        private static ID GetSitecoreId(NameValueCollection collection, string parameter, string defaultValue = null)
        {
            if (collection == null || !collection.HasKeys() || String.IsNullOrEmpty(parameter))
            {
                return null;
            }

            var value = collection[parameter] ?? String.Empty;

            if (String.IsNullOrEmpty(value))
            {
                value = defaultValue;
            }

            ID output;

            ID.TryParse(value, out output);

            return output;
        }

        /// <summary>
        /// Adds a Sitecore Id to a Multilist Field
        /// <para>All parameters are required.</para>
        /// </summary>
        /// <param name="item">Context Item</param>
        /// <param name="fieldId">Multilist Field</param>
        /// <param name="value">Value to add to the multilist field</param>
        private static void AddToMultiField(Item item, ID fieldId, ID value)
        {
            if (item == null || fieldId.IsNull || value.IsNull)
            {
                return;
            }

            MultilistField multiListField = item.Fields[fieldId];

            if (multiListField == null)
            {
                return;
            }

            using (new SecurityDisabler())
            {
                using (new EditContext(item))
                {
                    multiListField.Add(value.ToString());
                }
            }
        }
    }
}
