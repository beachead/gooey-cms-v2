using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Campaign;

namespace Gooeycms.Business.Campaigns
{
    public interface ICampaignEngine
    {
        //Determine if tracking is enabled
        Boolean IsEnabled { get; }

        /// <summary>
        /// Gets the tracking script that should be embedded into the page
        /// </summary>
        /// <returns></returns>
        String GetTrackingScript();

        /// <summary>
        /// Generates the tracking link that is required by the specified campaign tracking engine
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="source"></param>
        /// <param name="linkType"></param>
        /// <param name="landingPage"></param>
        /// <returns></returns>
        String GetTrackingLink(CmsCampaign campaign, String source, String linkType, String landingPage, Boolean isFileType);

        /// <summary>
        /// Converts a url-based campaign string to a querystring based
        /// </summary>
        /// <param name="rawString"></param>
        /// <returns></returns>
        String ConvertTrackingLink(String rawString);

        /// <summary>
        /// Method which determines the campaign that is active for the current request.
        /// </summary>
        /// <returns></returns>
        String GetActiveUserCampaign();

        /// <summary>
        /// Method which returns the goal page is for use in the querystring to trigger a goalpage
        /// </summary>
        /// <returns></returns>
        String GetGoalPageId();
    }
}
