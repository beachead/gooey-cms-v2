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

        public static Boolean IsDevelopmentMode()
        {
            return GooeyConfigManager.IsDevelopmentEnvironment;
        }

        public static Boolean IsPaypalSandbox()
        {
            return GooeyConfigManager.IsPaypalSandbox;
        }

        public static Boolean IsDefaultTemplate()
        {
            return (!String.IsNullOrEmpty(GooeyConfigManager.GetAsString(ConfigConstants.DefaultTemplate)));
        }

        public static void SetupFlash()
        {
            byte [] data = Encoding.UTF8.GetBytes(GooeyConfigManager.FlashCrossDomainFile);

            IStorageClient client = StorageHelper.GetStorageClient();
            client.Save(StorageClientConst.RootContainerIdentifier, StorageClientConst.RootFolder, ConfigConstants.FlashCrossDomainFilename, data, Permissions.Public);
        }

        public static void TogglePaypalMode()
        {
            if (GooeyConfigManager.IsPaypalSandbox)
            {
                GooeyConfigManager.PaypalPostUrl = ConfigConstants.PaypalProductionUrl;
            }
            else
            {
                GooeyConfigManager.PaypalPostUrl = ConfigConstants.PaypalSandboxUrl;
            }
        }

    }
}
