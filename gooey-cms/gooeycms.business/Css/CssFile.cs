using System;
using Gooeycms.Business.Storage;

namespace Gooeycms.Business.Css
{
    [Serializable]
    public class CssFile : SortableAssetFile
    {
        public override string Separator
        {
            get { return SortableAssetFile.DefaultSeparator; }
        }

        public override string Extension
        {
            get { return SortableAssetFile.CssExtension; }
        }
    }
}
