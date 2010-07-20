using System;

namespace Gooeycms.Data.Model
{
    public class Configuration : BasePersistedItem
    {
        public virtual String Name { get; set; }
        public virtual String Value { get; set; }
    }

    public class ConfigurationDao : BaseDao
    {
        public Configuration FindByKey(String key)
        {
            String hql = "select config from Configuration config where config.Name = :name";
            return base.NewHqlQuery(hql).SetString("name", key).UniqueResult<Configuration>();
        }
    }

    public class SiteConfiguration : Configuration
    {
        public virtual String SubscriptionGuid { get; set; }
    }

    public class SiteConfigurationDao : BaseDao
    {
        public SiteConfiguration FindByKey(Data.Guid guid, String key)
        {
            String hql = "select config from SiteConfiguration config where config.SubscriptionGuid = :guid and config.Name = :name";
            return base.NewHqlQuery(hql).SetString("guid",guid.Value).SetString("name", key).UniqueResult<SiteConfiguration>();
        }
    }
}
