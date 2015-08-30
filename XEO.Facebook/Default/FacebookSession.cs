using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Collections.Specialized;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

using XEO.Core;
using XEO.Core.Xml;
using XEO.Core.Web;

namespace XEO.Facebook.Default
{
    internal class FacebookSession : IFacebookSession
    {
        private const string StartUrl = @"https://www.facebook.com";
        private const string LoginUrl = @"https://www.facebook.com/login.php?login_attempt=1";
        private const string OperationHost = @"https://m.facebook.com";
        // href="/logout.php?h=Afc83VnyWo72X7TW&amp;t=1371665128&amp;refid=7
        
        private const string MobileUpcomingEventsUrl = @"https://m.facebook.com/events/";
        private const string MobilePastEventsUrl = @"https://m.facebook.com/events/?archive";

        private const string UserAgent = @"Mozilla/5.0 (Windows NT 6.2) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/27.0.1453.110 Safari/537.36";

        private string logoutUrl = null;
        private bool disposed = false;

        private Encoding FacbookTextEncoding;

        internal WebClient Client { get; set; }
        internal CookieContainer Cookies { get; set; }
        internal IDictionary<HttpRequestHeader, string> Headers { get; set; }

        public FacebookSession(string username, string password)
        {
            FacbookTextEncoding = Encoding.UTF8;

            Cookies = new CookieContainer();
            Headers = new Dictionary<HttpRequestHeader, string>();

            var extendedClient = new ExtendedWebClient();
            extendedClient.Encoding = FacbookTextEncoding;
            extendedClient.CookieContainer = Cookies;
            extendedClient.DefaultHeaderContainer = Headers;

            Client = extendedClient;

            Headers.Add(HttpRequestHeader.UserAgent, UserAgent);

            Login(username, password);
        }

        /// <summary>
        /// Retrieves a list of events that are in past.
        /// </summary>
        /// <returns>Enumeration of events.</returns>
        public IEnumerable<IFacebookEvent> GetBasicPastEventsData()
        {
            return GetBasicEventData(MobilePastEventsUrl);
        }

        /// <summary>
        /// Returns a list of events that are in future.
        /// </summary>
        /// <returns>Enumeration of events.</returns>
        public IEnumerable<IFacebookEvent> GetBasicUpcommingEventsData()
        {
            return GetBasicEventData(MobileUpcomingEventsUrl);
        }

        /// <summary>
        /// Creates an event-Object with the given id.
        /// </summary>
        /// <param name="id">Id of the facebook event.</param>
        /// <returns>A newly created event object with the given id.</returns>
        public IFacebookEvent GetEvent(string id)
        {
            var result = new FacebookEvent(this, id);
            return result;
        }

        /// <summary>
        /// Logs in the session. throws an exception if unsuccessfull.
        /// </summary>
        /// <param name="email">Email/username to login.</param>
        /// <param name="password">Password for the email.</param>
        private void Login(string email, string password)
        {
            var postParameters = GetLoginPostParameters(email, password);
            var loginResultBytes = Client.UploadValues(LoginUrl, postParameters);
            var facebookXElement = GetHtmlXElement(loginResultBytes);
            ValidateLogin(facebookXElement);
        }

        /// <summary>
        /// Validating the result of the login action. Throws an exception when login was unsuccessfull.
        /// </summary>
        /// <param name="facebookXElement">Xelement of the landing page right after login.</param>
        private void ValidateLogin(XElement facebookXElement)
        {
            var errorBox = facebookXElement.Descendants("div")
                .Where(div => 
                {
                    var divClass = div.Attribute("class");
                    return divClass != null && divClass.Value.Contains("login_error_box");
                })
                .FirstOrDefault();

            if (errorBox!=null)
            {
                var errorMessage = errorBox.Elements("div").First().Value;
                throw new Exception(errorMessage);
            }
        }

        /// <summary>
        /// Extracts the html-tag and parses it with xdocument.
        /// </summary>
        /// <param name="htmlContent">Html source to parse</param>
        /// <returns>XElement with html tag</returns>
        private XElement GetHtmlXElement(string htmlContent)
        {
            var clearString = Regex.Replace(htmlContent, @"\<script.*?\>.*?\<\/script\>","");
            var matches = Regex.Matches(
                clearString, @"\<body.*\<\/body\>", 
                RegexOptions.Singleline|
                RegexOptions.IgnoreCase|
                RegexOptions.CultureInvariant
            );
            
            var htmlXElement = matches
                .Cast<Match>()
                .Select(tag => XElement.Parse(tag.Value))
                .SingleOrDefault();

            return htmlXElement;
        }

        /// <summary>
        /// Transforms data to text, 
        /// extracts the html-tag and parses it with xdocument.
        /// </summary>
        /// <param name="htmlContent">Html source to parse</param>
        /// <returns>XElement with html tag</returns>
        private XElement GetHtmlXElement(byte[] responseData)
        {
            var responseString = FacbookTextEncoding.GetString(responseData);
            
            var htmlXElement = GetHtmlXElement(responseString);
            
            return htmlXElement;
        }

        /// <summary>
        /// Queries a root facebook page to parse login parameters.
        /// </summary>
        /// <param name="email">Email of the user that is used to send login.</param>
        /// <param name="password">Password of the user that is used to login.</param>
        /// <returns>A collection of name/value pairs that must be send via post parameters.</returns>
        private NameValueCollection GetLoginPostParameters(string email, string password)
        {
            var facebookText = Client.DownloadString(StartUrl);
            var formElements = GetHtmlXElement(facebookText).Descendants("form");
            var loginForm = formElements
                .Where(form => form.Attribute("id").Value == "login_form")
                .SingleOrDefault();

            var loginInputFields = loginForm
                .Descendants("input")
                .Select(inputTag => new
                {
                    Name = inputTag.TryGetAttributeValue("name"),
                    Value = inputTag.TryGetAttributeValue("value")
                });

            var postParameters = new NameValueCollection();
            foreach (var loginField in loginInputFields)
            {
                switch (loginField.Name)
                {
                    case "":
                    case "next":
                        break;
                    case "pass":
                    case "password":
                        postParameters.Add(loginField.Name, password);
                        break;
                    case "email":
                        postParameters.Add(loginField.Name, email);
                        break;
                    default:
                        postParameters.Add(loginField.Name, loginField.Value);
                        break;
                }
            }

            return postParameters;
        }

        /// <summary>
        /// Removes scripts, replaces \&nbsp\; and removes the default xhtml namespace.
        /// </summary>
        /// <param name="originalResult">Source loaded from the facebook mobile.</param>
        /// <returns></returns>
        internal string PrepareMobilePageXHtmlResult(string originalResult)
        {
            var clearString = Regex.Replace(originalResult, @"\<script.*?\>.*?\<\/script\>", "");

            clearString = clearString
                .Replace("&nbsp;", "&#160;")
                .Replace("xmlns=\"http://www.w3.org/1999/xhtml\"", "");

            return clearString;
        }

        /// <summary>
        /// Retrieves and XDocument from a facebook mobile page.
        /// </summary>
        /// <param name="url">The page url</param>
        /// <returns>The XDocument generated/retrieved from the given url.</returns>
        internal XDocument GetFacebookMobilePageDocument(string url)
        {
            var pageSource = Client.DownloadString(url);
            var clearedString = PrepareMobilePageXHtmlResult(pageSource);
            var document = XDocument.Parse(clearedString);

            logoutUrl = OperationHost + GetLogoutUrl(document);

            return document;
        }

        private string GetLogoutUrl(XDocument document)
        {
            var result = logoutUrl;

            var logoutHref = document.XPathSelectElement(@"//a[@data-siogil='logout']/@href");

            if (logoutHref != null) 
            {
                logoutUrl = logoutHref.Value;
            }


            return logoutUrl;
        }


        protected virtual IEnumerable<IFacebookEvent> GetBasicEventData(string url)
        {
            var eventsDocument = GetFacebookMobilePageDocument(url);

            var eventIds = eventsDocument
                .Descendants("article")
                .Select(article => new { Id = article.TryGetAttributeValue("id"), Element = article })
                .Where(div => div.Id != string.Empty)
                .Select(div =>
                {
                    var result = new FacebookEvent(this, div.Id);

                    var titleDiv = div
                        .Element
                        .Descendants("div")
                        .Where(div2 => div2.TryGetAttributeValue("class").StartsWith("title"))
                        .FirstOrDefault();

                    if (titleDiv != null)
                    {
                        result.Title = titleDiv.Value;
                    }

                    return result;
                });

            return eventIds;
        }


        private void Print(string text)
        {
            if (Factory<IEnvironment>.Instance.WriteToConsole)
            {
                Console.WriteLine(text);
            }
        }

        public void Dispose()
        {
            
        }
    }
}
