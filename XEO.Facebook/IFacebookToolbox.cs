using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEO.Facebook
{
    public interface IFacebookToolbox
    {
        IFacebookSession Login(string username, string password);
    }
}
