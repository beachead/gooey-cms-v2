using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Storage;
using Gooeycms.Business.Pages;
using Gooeycms.Data.Model.Page;
using Gooeycms.Data.Model.Site;
using Gooeycms.Business.Web;

namespace Gooeycms.Business.Pages
{
    public class PageRoleWorker
    {
        private QueueManager manager;

        public PageRoleWorker()
        {
            manager = new QueueManager(QueueNames.PageActionQueue);
        }

        public Boolean IsRunning { get; set; }



        public void ProcessMessages()
        {
            IList<PageTaskMessage> messages = manager.GetAll<PageTaskMessage>();
            foreach (PageTaskMessage message in messages)
            {
                try
                {
                    if (message.Action == PageTaskMessage.Actions.Save)
                        SavePage(message.Page);
                }
                catch (Exception) { }
            }
        }

        private void SavePage(CmsPage page)
        {
            Data.Guid guid = Data.Guid.New(page.SubscriptionId);
            CmsSitePath path = CmsSiteMap.Instance.GetPath(guid,page.Url);
            if (path == null)
            {
                CmsSiteMap.Instance.AddNewPage(guid, page.Url);
                path = CmsSiteMap.Instance.GetPath(guid, page.Url);
            }

            PageManager.Instance.AddNewPage(path.Parent,path.Name,page);
            PageManager.Instance.RemoveObsoletePages(page);
        }
    }
}
