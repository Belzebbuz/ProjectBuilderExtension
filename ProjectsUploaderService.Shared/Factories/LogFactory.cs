using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectsUploaderService.Shared.Factories
{
    public class LogFactory : ILogFactory
    {
        public Logger CreateLogger(string logPath)
        {
            return new LoggerConfiguration().WriteTo.File(logPath, rollingInterval: RollingInterval.Day, retainedFileCountLimit: 3).CreateLogger();
        }
    }
}
