using PumpService.App_Data;

using System.ServiceModel;

namespace PumpService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "PumpService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select PumpService.svc or PumpService.svc.cs at the Solution Explorer and start debugging.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class PumpService : IPumpService
    {
        IPumpServiceCallback Callback => OperationContext.Current?.GetCallbackChannel<IPumpServiceCallback>();

        public void RunScript()
        {
        }

        public void UpdateAndCompileScript(string fileName)
        {
        }
    }
}
