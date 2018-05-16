using System.Web;
using Sitecore.Resources.Media;
using Sitecore.Resources;
using Sitecore.Configuration;
using Sitecore;
using System.Xml;
using Sitecore.Common;

namespace HealthIS.MediaResolverPatch
{
    using Assert = Sitecore.Diagnostics.Assert;

    public class HealthISMediaConfig : MediaConfig
    {
        public HealthISMediaConfig()
            : base()
        {
        }
        public HealthISMediaConfig(XmlNode configNode)
            : base(configNode)
        {
        }
        public override MediaRequest ConstructMediaRequestInstance(HttpRequest httpRequest)
        {
            Assert.ArgumentNotNull((object)httpRequest, "httpRequest");
            HealthISMediaRequest mediaRequest = new HealthISMediaRequest();
            if (mediaRequest == null)
                return (MediaRequest)null;
            mediaRequest.Initialize(httpRequest);
            return (MediaRequest)mediaRequest;
        }
    }
}