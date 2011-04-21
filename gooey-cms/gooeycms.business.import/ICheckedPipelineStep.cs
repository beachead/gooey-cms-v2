using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Import
{
    public interface ICheckedPipelineStep : IPipelineStep
    {
        /// <summary>
        /// Called by the cralwer prior to adding the pipeline step.
        /// If the error check fails, it should throw an InvalidPipelineException
        /// </summary>
        void PerformErrorCheck();
    }
}
