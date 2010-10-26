using System;
using Gooeycms.Business.Storage;

namespace Gooeycms.Business.Javascript
{
    [Serializable]
    public class JavascriptFile : SortableAssetFile
    {
        public override string Separator
        {
            get { return SortableAssetFile.DefaultSeparator; }
        }

        public override string Extension
        {
            get { return SortableAssetFile.JavascriptExtension; }
        }
    }
}
