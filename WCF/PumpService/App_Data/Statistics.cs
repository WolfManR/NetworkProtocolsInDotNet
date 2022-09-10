namespace PumpService.App_Data
{
    public class Statistics : IStatistics
    {
        public int SuccessTacts { get; set; }
        public int ErrorTacts { get; set; }
        public int AllTacts { get; set; }
    }
}