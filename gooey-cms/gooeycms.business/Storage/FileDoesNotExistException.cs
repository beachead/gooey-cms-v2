using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Storage
{
    public class FileDoesNotExistException : Exception
    {
        public FileDoesNotExistException()
        {
        }

        public FileDoesNotExistException(String file) :
            base("The following file does not exist:" + file)
        {
        }
    }
}
