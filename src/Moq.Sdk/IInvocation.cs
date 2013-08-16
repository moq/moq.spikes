namespace Moq
{
    using System;
    using System.Reflection;

    public interface IInvocation
    {
        object Mock { get; }
        object Target { get; }
        MethodBase Method { get; }
        object[] Arguments { get; }
        object ReturnValue { get; set; }
        
        void InvokeBase();
    }
}