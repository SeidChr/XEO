using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XEO.Facebook
{
    public interface IFacebookUser
    {
        string Id { get; }

        string TextId { get; }

        string ProfilePictureSmall { get; }

        string SinglePartName { get; }

        string ProfileUrl { get; }
    }
}
