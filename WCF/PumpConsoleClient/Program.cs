using System;
using System.ServiceModel;

using PumpConsoleClient.PumpServiceReference;

namespace PumpConsoleClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InstanceContext instanceContext = new InstanceContext(new CallbackHandler());
            using (PumpServiceClient client = new PumpServiceClient(instanceContext))
            {
                client.UpdateAndCompileScript(@"C:\scripts\Sample.script");
                client.RunScript();

                Console.WriteLine("Please, Enter to exit ...");
                Console.ReadKey(true);
                client.Close();
            }
        }
    }
}
