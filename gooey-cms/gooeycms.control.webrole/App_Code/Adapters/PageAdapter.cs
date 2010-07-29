using System;
using System.Collections.Generic;
using Gooeycms.Business.Javascript;
using Gooeycms.Data.Model.Page;

namespace Gooeycms.Webrole.Control.App_Code.Adapters
{
    public class PageAdapter
    {
        private CmsPage page = null;
        public PageAdapter(CmsPage item)
        {
            this.page = item;
        }

        public PageAdapter() { }

        public CmsPage Page
        {
            get { return this.page; }
        }

        public IList<PageAdapter> GetFilteredPages(String filter)
        {
            IList<PageAdapter> results = new List<PageAdapter>();
            IList<CmsPage> pages = PageManager.Instance.Filter(filter);
            foreach (CmsPage page in pages)
                results.Add(new PageAdapter(page));

            return results;
        }
    }
}