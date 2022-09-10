﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PumpConsoleClient.PumpServiceReference {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="PumpServiceReference.IPumpService", CallbackContract=typeof(PumpConsoleClient.PumpServiceReference.IPumpServiceCallback), SessionMode=System.ServiceModel.SessionMode.Required)]
    public interface IPumpService {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPumpService/RunScript", ReplyAction="http://tempuri.org/IPumpService/RunScriptResponse")]
        void RunScript();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPumpService/RunScript", ReplyAction="http://tempuri.org/IPumpService/RunScriptResponse")]
        System.Threading.Tasks.Task RunScriptAsync();
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPumpService/UpdateAndCompileScript", ReplyAction="http://tempuri.org/IPumpService/UpdateAndCompileScriptResponse")]
        void UpdateAndCompileScript(string fileName);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPumpService/UpdateAndCompileScript", ReplyAction="http://tempuri.org/IPumpService/UpdateAndCompileScriptResponse")]
        System.Threading.Tasks.Task UpdateAndCompileScriptAsync(string fileName);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPumpServiceCallback {
        
        [System.ServiceModel.OperationContractAttribute(Action="http://tempuri.org/IPumpService/UpdateStatistics", ReplyAction="http://tempuri.org/IPumpService/UpdateStatisticsResponse")]
        void UpdateStatistics(object statistics);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface IPumpServiceChannel : PumpConsoleClient.PumpServiceReference.IPumpService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class PumpServiceClient : System.ServiceModel.DuplexClientBase<PumpConsoleClient.PumpServiceReference.IPumpService>, PumpConsoleClient.PumpServiceReference.IPumpService {
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance) : 
                base(callbackInstance) {
        }
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName) : 
                base(callbackInstance, endpointConfigurationName) {
        }
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, string remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance, string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, endpointConfigurationName, remoteAddress) {
        }
        
        public PumpServiceClient(System.ServiceModel.InstanceContext callbackInstance, System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(callbackInstance, binding, remoteAddress) {
        }
        
        public void RunScript() {
            base.Channel.RunScript();
        }
        
        public System.Threading.Tasks.Task RunScriptAsync() {
            return base.Channel.RunScriptAsync();
        }
        
        public void UpdateAndCompileScript(string fileName) {
            base.Channel.UpdateAndCompileScript(fileName);
        }
        
        public System.Threading.Tasks.Task UpdateAndCompileScriptAsync(string fileName) {
            return base.Channel.UpdateAndCompileScriptAsync(fileName);
        }
    }
}