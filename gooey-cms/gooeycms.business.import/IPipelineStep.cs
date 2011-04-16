using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Import
{
    public interface IPipelineStep : NCrawler.Interfaces.IPipelineStep
    {
        String ImportSiteHash { get; set; }
    }
}
