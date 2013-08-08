using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using XEO.Core;
using XEO.Core.Xml;
using XEO.Core.Web;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Xsl;
using System.Xml.XPath;

namespace XEO.Facebook.XPath
{
    internal class FacebookSession : Default.FacebookSession
    {
        public FacebookSession(string username, string password) : base(username, password)
        {
        }

        protected override IEnumerable<IFacebookEvent> GetBasicEventData(string url)
        {
            var eventsDocument = GetFacebookMobilePageDocument(url);

            var divs = eventsDocument.XPathSelectElements("//div[@id='m_events_list']/div[@id!='' and number(@id)!='NaN']");
            var list = divs.ToArray();

            return null;
        }

    }
}
