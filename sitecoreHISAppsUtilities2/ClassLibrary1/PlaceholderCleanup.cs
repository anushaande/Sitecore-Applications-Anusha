using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore;

namespace HealthIS.SitecoreEvents
{
    class SaveEvent
    {
        public void onItemSaved(object sender, EventArgs args)
        {
            return;
        }
    }
/*    static class SaveEvent 
    {
        public const string DYNAMIC_KEY_REGEX = @"(.+){[\d\w]{8}\-([\d\w]{4}\-){3}[\d\w]{12}}";
        public void OnItemSaved(object sender, EventArgs args)
        {
            var item = Event.ExtractParameter(args, 0) as Item;

            if (item != null)
            {
                //if the rendering reference points to a dynamic placeholder then ensure that that placeholder exists
                //if not then remove the reference. This takes care of the scenario where a scaffolding
                //component has been removed without first removing the Sub-Layouts that may by bound to it.
                var device = Context.Device;
                var renderingReferences = item.Visualization.GetRenderings(device, false);

                foreach (var renderingReference in renderingReferences)
                {
                    var key = renderingReference.Placeholder;
                    var regex = new Regex(DYNAMIC_KEY_REGEX);
                    var match = regex.Match(renderingReference.Placeholder);

                    if (match.Success && match.Groups.Count > 0)
                    {

                        //get the rendering reference unique id that we are contained in
                        var parentRenderingId = key.Substring(key.Length - GUID_LENGTH, GUID_LENGTH).ToUpper();

                        //if this parent renderingReference is not in the current list of rendering references 
                        //then the current rendering reference should be removed as it means that the parent
                        //rendering reference has been removed by the user without first removing  the children
                        if (renderingReferences.All(r => r.UniqueId.ToUpper() != parentRenderingId))
                        {
                            //use an extension method to remove the orphaned rendering reference
                            //from the item's layout definition
                            RemoveRenderingReference(item, renderingReference.UniqueId);
                        }
                    }
                }
            }
        }
        public static void RemoveRenderingReference(this Item item, string renderingReferenceUid)
        {
            var doc = new System.Xml.XmlDocument();
            doc.LoadXml(item[FieldIDs.LayoutField]);

            //remove the orphaned rendering reference from the layout definition
            var node = doc.SelectSingleNode(string.Format("//r[@uid='{0}']", renderingReferenceUid));

            if (node != null && node.ParentNode != null)
            {
                node.ParentNode.RemoveChild(node);

                //save layout definition back to the item
                using (new EditContext(item))
                {
                    item[FieldIDs.LayoutField] = doc.OuterXml;
                }
            }
        }
    }*/
}