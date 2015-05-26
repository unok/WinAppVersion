using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinAppVersion
{
    class WinAppVersionException : Exception
    {
        public WinAppVersionException(string msg, Exception e)
            : base(msg, e)
        {
        }
        public WinAppVersionException(string msg)
            : base(msg)
        {
        }
    }
}
