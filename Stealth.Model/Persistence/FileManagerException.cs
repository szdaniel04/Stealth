using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stealth.Persistence
{
    public class FileManagerException : Exception
    {
        public FileManagerException() : base() { }
        public FileManagerException(string msg) : base(msg) { }

        public FileManagerException(string msg, Exception e) : base(msg, e) { }
    }
}
