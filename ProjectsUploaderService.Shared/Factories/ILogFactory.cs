using Serilog.Core;

namespace ProjectsUploaderService.Shared.Factories
{
    public interface ILogFactory
    {
        Logger CreateLogger(string logPath);
    }
}