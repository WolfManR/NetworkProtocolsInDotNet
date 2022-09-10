using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CSharp;
using PumpService.App_Data;

namespace PumpService.Scripting
{
    public class ScriptService : IScriptService
    {
        private readonly IPumpServiceCallback _callback;
        private readonly IScriptConfiguration _configuration;
        private readonly IStatistics _statistics;

        private CompilerResults _compilerResults;

        public ScriptService(IPumpServiceCallback callback, IScriptConfiguration configuration)
        {
            _callback = callback;
            _configuration = configuration;
            _statistics = new Statistics();
        }

        public bool Compile()
        {
            if (!File.Exists(_configuration.FileName)) return false;

            try
            {
                CompilerParameters compilerParameters = new CompilerParameters
                {
                    GenerateInMemory = true
                };
                compilerParameters.ReferencedAssemblies.AddRange(new[]
                {"System.dll","System.Core.dll","System.Data.dll", "Microsoft.CSharp.dll", Assembly.GetExecutingAssembly().Location});

                CSharpCodeProvider provider = new CSharpCodeProvider();
                var fileData = LoadFile(_configuration.FileName);
                _compilerResults = provider.CompileAssemblyFromSource(compilerParameters, fileData);
                
                return !_compilerResults.Errors.HasErrors;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Run(int count)
        {
            if (_compilerResults == null || _compilerResults.Errors.HasErrors)
            {
                if(!Compile()) return;
            }
            
            Type t = _compilerResults?.CompiledAssembly.GetType("Sample.SampleScript");
            MethodInfo entryPointMethod = t?.GetMethod("EntryPoint");
            if(entryPointMethod is null) return;

            Task.Run(() =>
            {
                for (var i = 0; i < count; i++)
                {
                    if ((bool)entryPointMethod.Invoke(Activator.CreateInstance(t), null))
                    {
                        _statistics.SuccessTacts++;
                    }
                    else
                    {
                        _statistics.ErrorTacts++;
                    }

                    _statistics.AllTacts++;
                    _callback.UpdateStatistics(_statistics);
                    Thread.Sleep(1000);
                }
            });
        }

        private static string LoadFile(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open);
            try
            {
                int length = (int)fileStream.Length;
                var buffer = new byte[length];
                int count;
                int sum = 0;
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;
                return Encoding.UTF8.GetString(buffer);
            }
            finally
            {
                fileStream.Close();
            }
        }
    }
}