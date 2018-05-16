using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HealthIS.Apps.MVC.Models
{
    public class ListBuilder : Sitecore.Mvc.Presentation.RenderingModel
    {
        public Item DataSourceFolder { get; set; }
        public bool dsIsSet { get; set; }
        public List<ListBuilderSection> Sections = new List<ListBuilderSection>();

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            dsIsSet = !String.IsNullOrEmpty(rendering.DataSource);
            if (dsIsSet)
            {
                try
                {
                    DataSourceFolder = Sitecore.Context.Database.GetItem(rendering.DataSource);
                    if (DataSourceFolder != null && DataSourceFolder.HasChildren)
                    {
                        foreach (Item item in DataSourceFolder.Children)
                        {
                            if (item.TemplateName.Contains("Section"))
                            {
                                Sections.Add(new ListBuilderSection(item));
                            }
                        }
                    }
                }
                catch { }
            }
        }

        public static ListBuilderType getListTypeField(Item item, string fieldName)
        {
            try
            {
                switch (item.Fields[fieldName].Value.ToLower())
                {
                    case "none":
                        return ListBuilderType.None;
                    case "mediavertical":
                        return ListBuilderType.MediaVertical;
                    case "mediahorizontal":
                        return ListBuilderType.MediaHorizontal;
                    case "pdflist":
                        return ListBuilderType.PDFList;
                    default: return ListBuilderType.None;
                }
            }
            catch { return ListBuilderType.None; }
        }

        public static ListBuilderSubType getListSubTypeField(Item item, string fieldName)
        {
            try
            {
                switch (item.Fields[fieldName].Value.ToLower())
                {
                    case "none":
                        return ListBuilderSubType.None;
                    case "imagetitleparagraph":
                        return ListBuilderSubType.ImageTitleParagraph;
                    case "imageparagraph":
                        return ListBuilderSubType.ImageParagraph;
                    case "imagetitle":
                        return ListBuilderSubType.ImageTitle;
                    case "imagegrid":
                        return ListBuilderSubType.ImageGrid;
                    case "linklist":
                        return ListBuilderSubType.LinkList;
                    case "pdflist":
                        return ListBuilderSubType.PDFList;
                    default: return ListBuilderSubType.None;
                }
            }
            catch { return ListBuilderSubType.None; }
        }
    }

    public class ListBuilderSection
    {
        public ListBuilderType ListType { get; set; }
        public ListBuilderSubType ListSubType { get; set; }
        public Item SectionItem { get; set; }
        public string Header { get; set; }
        public string sectionHeaderFontSize { get; set; }
        public ListBuilderSection(Item item)
        {
            SectionItem = item;
            Header = Helpers.getStrField(item, "Header");
            sectionHeaderFontSize = Helpers.getStrField(item, "Section Header Font Size");
            ListType = ListBuilder.getListTypeField(item, "List Type");
            ListSubType = ListBuilder.getListSubTypeField(item, "List Sub Type");

            if (String.IsNullOrEmpty(this.sectionHeaderFontSize))
            {
                this.SectionItem.Editing.BeginEdit();
                this.SectionItem.Fields["Section Header Font Size"].Value = "h2";
                this.SectionItem.Editing.EndEdit();
            }
        }
    }

    public enum ListBuilderType
    {
        None,
        MediaVertical,
        MediaHorizontal,
        PDFList
    }

    public enum ListBuilderSubType
    {
        None,
        ImageTitleParagraph,
        ImageParagraph,
        ImageTitle,
        ImageGrid,
        LinkList,
        PDFList
    }
}