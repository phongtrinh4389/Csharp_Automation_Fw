using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Automation_Framework.Core.Common
{
    public class ErrorHandler : Exception
    {
        public ErrorHandler()
        {
            NLogger.Error(this);
        }
        public ErrorHandler(string message) : base(message)
        {
            NLogger.Error(this);
        }
        public ErrorHandler(string message, Exception inner) : base(message, inner)
        {
            NLogger.Error(this);
        }
    }

}
