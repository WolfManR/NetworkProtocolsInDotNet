using PumpService.App_Data;

using System.ServiceModel;
using PumpService.Scripting;

namespace PumpService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PumpService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PumpService.svc or PumpService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PumpService : IPumpService
    {
        private readonly IScriptService _scriptService;
        private readonly IScriptConfiguration _scriptConfiguration;

        public PumpService()
        {
            _scriptConfiguration = new ScriptConfiguration();
            _scriptService = new ScriptService(Callback, _scriptConfiguration);
        }

        IPumpServiceCallback Callback => OperationContext.Current?.GetCallbackChannel<IPumpServiceCallback>();

        public void RunScript()
        {
            _scriptService.Run(10);
        }

        public void UpdateAndCompileScript(string fileName)
        {
            _scriptConfiguration.FileName = fileName;
            _scriptService.Compile();
        }
    }
}
