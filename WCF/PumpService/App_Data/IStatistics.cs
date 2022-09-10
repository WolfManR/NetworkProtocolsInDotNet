namespace PumpService.App_Data
{
    public interface IStatistics
    {
        int SuccessTacts { get; set; }

        int ErrorTacts { get; set; }

        int AllTacts { get; set; }
    }
}