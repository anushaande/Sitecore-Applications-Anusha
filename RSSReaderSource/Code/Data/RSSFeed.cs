using Sitecore.Data.Items;
using Sitecore.Data.Fields;

namespace Sitecore.RSSReader.Data
{
	public class RSSFeed
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RSSFeed"/> class.
		/// </summary>
		/// <param name="item">The item.</param>
		public RSSFeed(Item item)
		{
			_innerItem = item;
		}

		private Item _innerItem = null;
		/// <summary>
		/// Gets or sets the inner item.
		/// </summary>
		/// <value>The inner item.</value>
		public Item InnerItem
		{
			get { return _innerItem; }
		}

		private LinkField _feedUrl = null;
		public LinkField FeedUrl
		{
			get
			{
				if (_feedUrl == null)
				{
					_feedUrl = _innerItem.Fields["feed url"];
				}
				return _feedUrl;
			}
		}
		private string _feedCSSClass = null;
		/// <summary>
		/// Gets the feed CSS class.
		/// </summary>
		/// <value>The feed CSS class.</value>
		public string FeedCSSClass
		{
			get
			{
				if (_feedCSSClass == null)
				{
					_feedCSSClass = this._innerItem["feed css class"];
				}
				return _feedCSSClass;
			}
		}

		private string _feedItemCSSClass = null;
		/// <summary>
		/// Gets the feed item CSS class.
		/// </summary>
		/// <value>The feed item CSS class.</value>
		public string FeedItemCSSClass
		{
			get
			{
				if (_feedItemCSSClass == null)
				{
					_feedItemCSSClass = this._innerItem["feed item css class"];
				}
				return _feedItemCSSClass;
			}
		}

		private string _feedTitleCSSClass = null;
		/// <summary>
		/// Gets the feed title CSS class.
		/// </summary>
		/// <value>The feed title CSS class.</value>
		public string FeedTitleCSSClass
		{
			get
			{
				if (_feedTitleCSSClass == null)
				{
					_feedTitleCSSClass = this._innerItem["feed title css class"];
				}
				return _feedTitleCSSClass;
			}
		}

		private string _feedDescriptionCSSClass = null;
		/// <summary>
		/// Gets the feed description CSS class.
		/// </summary>
		/// <value>The feed description CSS class.</value>
		public string FeedDescriptionCSSClass
		{
			get
			{
				if (_feedDescriptionCSSClass == null)
				{
					_feedDescriptionCSSClass = this._innerItem["feed description css class"];
				}
				return _feedDescriptionCSSClass;
			}
		}

		private string _feedItemTitleCSSClass = null;
		/// <summary>
		/// Gets the feed item title CSS class.
		/// </summary>
		/// <value>The feed item title CSS class.</value>
		public string FeedItemTitleCSSClass
		{
			get
			{
				if (_feedItemTitleCSSClass == null)
				{
					_feedItemTitleCSSClass = this._innerItem["feed item title css class"];
				}
				return _feedItemTitleCSSClass;
			}
		}

		private string _feedItemDescriptionCSSClass = null;
		/// <summary>
		/// Gets the feed item description CSS class.
		/// </summary>
		/// <value>The feed item description CSS class.</value>
		public string FeedItemDescriptionCSSClass
		{
			get
			{
				if (_feedItemDescriptionCSSClass == null)
				{
					_feedItemDescriptionCSSClass = this._innerItem["feed item description css class"];
				}
				return _feedItemDescriptionCSSClass;
			}
		}

		private string _feedItemPublishDateCSSClass = null;
		/// <summary>
		/// Gets the feed item publish date.
		/// </summary>
		/// <value>The feed item publish date.</value>
		public string FeedItemPublishDateCSSClass
		{
			get
			{
				if (_feedItemPublishDateCSSClass == null)
				{
					_feedItemPublishDateCSSClass = this._innerItem["feed item publication date css class"];
				}
				return _feedItemPublishDateCSSClass;
			}
		}

        private string _feedItemImageCSSClass = null;
        /// <summary>
        /// Gets the feed item publish date.
        /// </summary>
        /// <value>The feed item publish date.</value>
        public string FeedItemImageCSSClass
        {
            get
            {
                if (_feedItemImageCSSClass == null)
                {
                    _feedItemImageCSSClass = this._innerItem["feed item image css class"];
                }
                return _feedItemImageCSSClass;
            }
        }

        public string FeedItemTitle
        {
            get
            {
                if(_innerItem != null){
                    if (_innerItem.Name == "RSS Reader")
                    {
                        return "Datasource not Defined";
                    }
                    return _innerItem.Name;
                }
                return "Config Item Not Defined";
            }
        }

        private bool _displayFeedTitle = true;
        public bool DisplayFeedTitle
        {
            get
            {
                try{
                    _displayFeedTitle = int.Parse(this._innerItem["Display Feed Title"]) == 1;
                }
                catch
                {
                    _displayFeedTitle = false;
                }
                return _displayFeedTitle;
            }
            set
            {
                this._innerItem.Editing.BeginEdit();
                if (value)
                {
                    this._innerItem.Fields["Display Feed Title"].Value = "1";
                }
                else
                {
                    this._innerItem.Fields["Display Feed Title"].Value = "";
                }
                this._innerItem.Editing.EndEdit();
            }
        }

        private bool _displayFeedDesc = true;
        public bool DisplayFeedDesc
        {
            get
            {
                try
                {
                    this._displayFeedDesc = int.Parse(this._innerItem["Display Feed Description"]) == 1;
                }
                catch
                {
                    this._displayFeedDesc = false;
                }
                return this._displayFeedDesc;
            }
            set
            {
                this._innerItem.Editing.BeginEdit();
                if (value)
                    this._innerItem.Fields["Display Feed Description"].Value = "1";
                else
                    this._innerItem.Fields["Display Feed Description"].Value = "";
                this._innerItem.Editing.EndEdit();
            }
        }

        private bool _displayTitleFirst = true;
        public bool DisplayTitleFirst
        {
            get
            {
                try
                {
                    this._displayTitleFirst = int.Parse(this._innerItem["Display Title First"]) == 1;
                }
                catch
                {
                    this._displayTitleFirst = false;
                }
                return this._displayTitleFirst;
            }
            set
            {
                this._innerItem.Editing.BeginEdit();
                if (value)
                    this._innerItem.Fields["Display Title First"].Value = "1";
                else
                    this._innerItem.Fields["Display Title First"].Value = "";
                this._innerItem.Editing.EndEdit();
            }
        }

        private int _descriptionCharacterLimit = -1;
        public int DescriptionCharacterLimit
        {
            get
            {
                if(_descriptionCharacterLimit == -1){
                    _descriptionCharacterLimit = int.Parse(this._innerItem["Item Description Character Limit"]);
                }
                return _descriptionCharacterLimit;
            }
        }

        private int _numberOfItems = -1;
        public int NumberOfItems
        {
            get
            {
                if (_numberOfItems == -1)
                {
                    _numberOfItems = int.Parse(this._innerItem["Number of Items"]);
                }
                return _numberOfItems;
            }
        }

        private int _numberOfImages = -1;
        public int NumberOfImages
        {
            get
            {
                if (_numberOfImages == -1)
                {
                    _numberOfImages = int.Parse(this.InnerItem["Number of Images"]);
                }
                return _numberOfImages;
            }
        }
	}
}
