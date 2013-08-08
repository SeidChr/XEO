using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XEO.Facebook
{
    public interface IFacebookEvent
    {
        string Id { get; }

        string Title { get; }

        IEnumerable<IFacebookUser> GetGoingUsers();
        IEnumerable<IFacebookUser> GetMaybeUsers();
        IEnumerable<IFacebookUser> GetInvitedUsers();
    }
}
