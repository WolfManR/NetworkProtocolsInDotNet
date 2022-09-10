using PumpService.App_Data;

namespace PumpService.Scripting
{
    public class ScriptService : IScriptService
    {
        private readonly IPumpServiceCallback _callback;
        private readonly IScriptConfiguration _configuration;
        private readonly IStatistics _statistics;

        public ScriptService(IPumpServiceCallback callback, IScriptConfiguration configuration)
        {
            _callback = callback;
            _configuration = configuration;
            _statistics = new Statistics();
        }

        public bool Compile() => false;

        public void Run(int count)
        {
        }
    }
}