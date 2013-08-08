using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;
using XEO.Core.Xml;

namespace XEO.Facebook.Default
{
    internal class FacebookEvent : IFacebookEvent
    {
        /// <summary>
        /// Constant value for retrieving subsequent user-list pages.
        /// </summary>
        private const int UsersPageStep = 30;

        /// <summary>
        /// The session this even is using to fetch data.
        /// </summary>
        private FacebookSession Session { get; set; }

        /// <summary>
        /// Backing store for the title property
        /// </summary>
        private string title = null;

        /// <summary>
        /// Creates a new instance of a Facebook Event.
        /// </summary>
        /// <param name="session">The session this event is bound to.</param>
        /// <param name="id">Id of the event.</param>
        public FacebookEvent(FacebookSession session, string id)
        {
            Session = session;
            Id = id;
        }

        /// <summary>
        /// Gets the id of this event.
        /// </summary>
        public string Id
        {
            get;
            private set;
        }

        /// <summary>
        /// Returns the title. A call on this property might include a web request.
        /// </summary>
        public string Title
        {
            get 
            {
                if (title == null)
                {
                    PopulateEventData();
                }

                return title;
            }

            internal set 
            {
                title = value;
            }
        }

        /// <summary>
        /// Returns a list of users that are on the attendees list.
        /// </summary>
        /// <returns>List of IFacebookUser.</returns>
        public IEnumerable<IFacebookUser> GetGoingUsers()
        {
            return GetUsers("going");
        }

        /// <summary>
        /// Returns a list of users that are on the maybe list.
        /// </summary>
        /// <returns>List of IFacebookUser.</returns>
        public IEnumerable<IFacebookUser> GetMaybeUsers()
        {
            return GetUsers("maybe");
        }

        /// <summary>
        /// Returns a list of users that are on the invited list.
        /// </summary>
        /// <returns>List of IFacebookUser.</returns>
        public IEnumerable<IFacebookUser> GetInvitedUsers()
        {
            return GetUsers("invited");
        }


        private void PopulateEventData()
        {
            var eventDocument = Session.GetFacebookMobilePageDocument(
                string.Format("https://m.facebook.com/events/{0}", Id));

            Title = GetTitle(eventDocument);
        }

        private string GetTitle(XDocument eventDocument)
        {
            return eventDocument.Element("html").Element("head").Element("title").Value;
        }

        private IEnumerable<IFacebookUser> GetUsers(string userList)
        {
            var attendeesDocument = Session.GetFacebookMobilePageDocument(
                string.Format("https://m.facebook.com/events/{0}/guests/{1}", Id, userList));

            var users = GetUsers(attendeesDocument);

            foreach (var user in users)
            {
                yield return user;
            }

            // ?start=30
            int step = 0;
            while (HasShowMoreButton(attendeesDocument))
            {
                step++;

                attendeesDocument = Session.GetFacebookMobilePageDocument(
                    string.Format("https://m.facebook.com/events/{0}/guests/{1}?start={2}", Id, userList, step * UsersPageStep));

                users = GetUsers(attendeesDocument);

                foreach (var user in users)
                {
                    yield return user;
                }
            }
        }

        private bool HasShowMoreButton(XDocument userPageDocument)
        {
            return userPageDocument
                .Descendants("div")
                .Where(div => div.TryGetAttributeValue("id") == "m_more_item")
                .Any();
        }

        private IEnumerable<IFacebookUser> GetUsers(XDocument userPageDocument)
        {
            var users = userPageDocument
                .Descendants("div")
                .Where(div =>
                {
                    return
                        div.TryGetAttributeValue("class").StartsWith("item ")
                        && div.TryGetAttributeValue("id") != "m_more_item";
                })
                .Select(div => GetUserFromItemDiv(div))
                .Where(user => user != null);

            return users;
        }

        private IFacebookUser GetUserFromItemDiv(XElement itemDiv)
        {
            /**
             * The true id is encoded in an "data-store" attribute on an a-tag.
             * It is html-encoded JSON that contains the field "id":100000529375110
             * The id can only be called reliable with /profile.php?id=XXX
             * 
             * ID is also endoded in the profile-image-link
             * 
             * TBI!
             */

            FacebookUser result = null;

            var listItemContentDiv = itemDiv
                .Descendants("div")
                .Where(div => div.TryGetAttributeValue("class").EndsWith(" cc"))
                .FirstOrDefault();

            if (listItemContentDiv != null)
            {
                var idAndNameDiv = listItemContentDiv.Descendants("div")
                    .Where(div => div.TryGetAttributeValue("class").EndsWith(" c"))
                    .FirstOrDefault();

                var userLinkElement = idAndNameDiv.Element("a");
                var userLink = userLinkElement.TryGetAttributeValue("href");
                var textId = GetTextId(userLink);

                var imgStyle = listItemContentDiv.Element("a").Element("i").Attribute("style").Value;
                var url = Regex.Match(imgStyle, "url\\(\"(?<url>.*?)\"\\)").Groups["url"].Value;

                var userId = Regex.Match(url, @"\d+_(?<userId>\d+)_\d+").Groups["userId"].Value;

                result = new FacebookUser(userId);
                result.TextId = textId;
                result.SinglePartName = userLinkElement.Value;
                result.ProfilePictureSmall = url;

            }

            return result;
        }

        private string GetTextId(string profileUrl)
        {
            var a = profileUrl.LastIndexOf('/');
            var b = profileUrl.IndexOf('?');

            var userId = profileUrl.Substring(a + 1, b - a - 1);

            if (userId == "profile.php")
            {
                userId = null;
                // yay, we got an old-style Id here!!
                // /profile.php?id=XXXXXXXXXXX&fref=pb
                //a = profileUrl.IndexOf('=');
                //b = profileUrl.IndexOf('&');
                //userId = profileUrl.Substring(a + 1, b - a - 1);
            }

            return userId;
        }
    }
}
