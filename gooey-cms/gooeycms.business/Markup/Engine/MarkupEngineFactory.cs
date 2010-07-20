using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            return new DefaultMarkupEngine();
        }
    }
}
