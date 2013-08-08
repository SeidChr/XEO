using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XEO.Facebook
{
    public interface IFacebookSession
    {
        /// <summary>
        /// Retrieves upcoming events.
        /// </summary>
        /// <returns>A list of Events that are now or later.</returns>
        IEnumerable<IFacebookEvent> GetBasicUpcommingEventsData();

        /// <summary>
        /// Retrieves past events.
        /// </summary>
        /// <returns>A list of Events that passed.</returns>
        IEnumerable<IFacebookEvent> GetBasicPastEventsData();

        /// <summary>
        /// Returns an event for a given id.
        /// </summary>
        /// <param name="id">Id of the facebook event.</param>
        /// <returns>An event wrapper for an facebook event.</returns>
        IFacebookEvent GetEvent(string id);
    }
}
