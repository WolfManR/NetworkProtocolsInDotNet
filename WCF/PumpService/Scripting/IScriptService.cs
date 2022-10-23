namespace PumpService.Scripting
{
    public interface IScriptService
    {
        bool Compile();
        void Run(int count);
    }
}