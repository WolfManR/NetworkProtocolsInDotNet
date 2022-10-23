using System;
using PumpConsoleClient.PumpServiceReference;

namespace PumpConsoleClient
{
    internal class CallbackHandler : IPumpServiceCallback
    {
        public void UpdateStatistics(Statistics statistics)
        {
            Console.Clear();
            Console.WriteLine("Обновление по статистике выполнения скрипта");
            Console.WriteLine($"Всего     тактов: {statistics.AllTacts}");
            Console.WriteLine($"Успешных  тактов: {statistics.SuccessTacts}");
            Console.WriteLine($"Ошибочных тактов: {statistics.ErrorTacts}");
        }
    }
}