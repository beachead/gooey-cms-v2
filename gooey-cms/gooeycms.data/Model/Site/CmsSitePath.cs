﻿using System;
using System.Collections.Generic;

namespace Gooeycms.Data.Model.Site
{
    [Serializable]
    public class CmsSitePath : BasePersistedItem
    {
        public virtual String SubscriptionGuid { get; set; }
        public virtual String UrlHash { get; set; }
        public virtual String Url { get; set; }
        public virtual String Parent { get; set; }
        public virtual Int32 Depth { get; set; }
        public virtual Int32 Position { get; set; }
        public virtual String RedirectTo { get; set; }
        public virtual Boolean IsRedirect { get; set; }
        public virtual Boolean IsDirectory { get; set; }
        public virtual Boolean IsVisible { get; set; }
        public virtual Boolean IsPage { get; set; }
        public virtual String _Labels { get; set; }

        public virtual String Name
        {
            get
            {
                if (Depth >= 1)
                {
                    int pos = this.Url.LastIndexOf("/") + 1;
                    return this.Url.Substring(pos);
                }
                else
                {
                    return this.Url;
                }
            }
        }

        public virtual IDictionary<String, String> Labels
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
