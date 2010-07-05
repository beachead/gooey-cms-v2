using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
}
