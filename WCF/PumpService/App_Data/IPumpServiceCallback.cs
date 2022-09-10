using System.ServiceModel;

namespace PumpService.App_Data
{
    [ServiceContract]
    public interface IPumpServiceCallback
    {
        [OperationContract]
        void UpdateStatistics(IStatistics statistics);
    }
}