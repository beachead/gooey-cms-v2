using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Extensions;
using Gooeycms.Constants;

namespace Gooeycms.Data.Model.Store
{
    public enum PackageTypes
    {
        Site,
        Theme
    }

    public class Package : BasePersistedItem
    {
        public const char ScreenshotSeparator = TextConstants.DefaultSeparator;
        public const char FeatureSeparator = TextConstants.NewlineSeparator;

        public virtual String OwnerSubscriptionId { get; set; }
        public virtual String Guid { get; set; }
        public virtual String PackageTypeString { get; set; }
        public virtual String Title { get; set; }
        public virtual String Features { get; set; }
        public virtual Int32 PageCount { get; set; }
        public virtual String Category { get; set; }
        public virtual Double Price { get; set; }
        public virtual Boolean IsApproved { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime Approved { get; set; }
        public virtual String Screenshots { get; set; }

        public virtual PackageTypes PackageType
        {
            get
            {
                return (PackageTypes)Enum.Parse(typeof(PackageTypes), this.PackageTypeString, true);
            }
            set { this.PackageTypeString = value.ToString(); }

        }

        public virtual IList<String> ScreenshotList
        {
            get
            {
                return this.Screenshots.SplitAsList(ScreenshotSeparator);
            }
        }

        public virtual IList<String> FeatureList
        {
            get
            {
                return this.Features.SplitAsList(FeatureSeparator);
            }
        }
    }
}
