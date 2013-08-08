using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEO.Facebook.Default
{
    internal class FacebookUser : IFacebookUser
    {
        public FacebookUser(string id)
        {
            Id = id;
        }

        public string Id
        {
            get;
            private set;
        }

        public string ProfilePictureSmall
        {
            get;
            internal set;
        }

        public string SinglePartName
        {
            get;
            internal set;
        }

        public string TextId
        {
            get;
            internal set;
        }

        public string ProfileUrl
        {
            get 
            {
                return string.Format("https://www.facebook.com/profile.php?id={0}", Id);
            }
        }
    }
}
