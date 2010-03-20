using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace Beachead.Persistence.Hibernate
{
    public class NHibernateProperties : ConfigurationSection
    {
        [ConfigurationProperty("nhibernate")]
        public NameValueConfigurationCollection HibernateProperties
        {
            get { return (NameValueConfigurationCollection)base["nhibernate"]; }
        }
    }
}
