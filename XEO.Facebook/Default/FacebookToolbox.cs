using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEO.Facebook.Default
{
    [Export(typeof(IFacebookToolbox))]
    internal class FacebookToolbox : IFacebookToolbox
    {
        public IFacebookSession Login(string username, string password)
        {
            var result = new FacebookSession(username, password);
            return result;
        }
    }
}
