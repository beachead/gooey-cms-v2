using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Forms.Plugins;
using System.Reflection;

namespace Gooeycms.Business.Forms
{
    public class FormPluginFactory
    {
        private static FormPluginFactory instance;
        private IList<IFormPlugin> plugins = null;

        private FormPluginFactory()
        {
        }

        public static FormPluginFactory Instance
        {
            get
            {
                if (instance == null)
                    instance = new FormPluginFactory();

                return instance;
            }
        }

        /// <summary>
        /// Retrieves all of the plugins which are available for
        /// this CMS system
        /// </summary>
        /// <returns></returns>
        public IList<IFormPlugin> GetPlugins()
        {
            if (this.plugins == null)
            {
                this.plugins = new List<IFormPlugin>();
                Assembly assem = Assembly.GetExecutingAssembly();
                foreach (Type type in assem.GetTypes())
                {
                    if ((type.IsClass) && (!type.IsAbstract))
                    {
                        if (typeof(IFormPlugin).IsAssignableFrom(type))
                        {
                            IFormPlugin plugin = (IFormPlugin)System.Activator.CreateInstance(type);
                            this.plugins.Add(plugin);
                        }
                    }
                }
            }

            return this.plugins;
        }
    }
}
