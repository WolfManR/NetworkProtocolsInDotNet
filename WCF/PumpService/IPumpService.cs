using System.ServiceModel;
using PumpService.App_Data;

namespace PumpService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IPumpService" in both code and config file together.
    [ServiceContract(SessionMode = SessionMode.Required, CallbackContract = typeof(IPumpServiceCallback))]
    public interface IPumpService
    {
        [OperationContract]
        void RunScript();

        [OperationContract]
        void UpdateAndCompileScript(string fileName);
    }
}
