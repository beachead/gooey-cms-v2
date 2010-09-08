using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Util;
using Gooeycms.Business.Markup.Engine;

namespace Beachead.Core.Markup.Engine
{
    public class MarkupEngineFactory
    {
        private static MarkupEngineFactory instance = new MarkupEngineFactory();

        private MarkupEngineFactory() { }
        public static MarkupEngineFactory Instance
        {
            get { return MarkupEngineFactory.instance; }
        }

        /// <summary>
        /// Gets the default markup engine to use
        /// </summary>
        /// <returns></returns>
        public IMarkupEngine GetDefaultEngine()
        {
            IMarkupEngine result;

            String name = CurrentSite.Configuration.MarkupEngineName;
            if (name == null)
                result = new MarkdownMarkupEngine();
            else
            {
                try
                {
                    result = (IMarkupEngine)Activator.CreateInstance(Type.GetType(name));
                }
                catch (Exception e)
                {
                    throw new ApplicationException("The specified markup engine:" + name + " is not valid.", e);
                }
            }

            return result;
        }
    }
}
