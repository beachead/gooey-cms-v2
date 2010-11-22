using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Membership;
using Gooeycms.Business.Storage;
using Gooeycms.Constants;

namespace Gooeycms.Business.Util
{
    public static class GooeyStatus
    {
        public static Boolean IsMembershipConfigured()
        {
            return (MembershipUtil.IsRolesConfigured);
        }

        public static Boolean IsFlashConfigured()
        {
            IStorageClient client = StorageHelper.GetStorageClient();
            return client.Exists(StorageClientConst.RootContainerIdentifier,StorageClientConst.RootFolder,ConfigConstants.FlashCrossDomainFilename);
        }

        public static void SetupFlash()
        {
            byte [] data = Encoding.UTF8.GetBytes(GooeyConfigManager.FlashCrossDomainFile);

            IStorageClient client = StorageHelper.GetStorageClient();
            client.Save(StorageClientConst.RootContainerIdentifier, StorageClientConst.RootFolder, ConfigConstants.FlashCrossDomainFilename, data, Permissions.Public);
        }
    }
}
