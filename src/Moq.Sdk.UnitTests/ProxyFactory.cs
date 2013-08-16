namespace Moq
{
    using Moq.Sdk.UnitTests;
    using System;
    using System.Linq;
    using System.Reflection;

    public class ProxyFactory
    {
        public object CreateProxy(IProxied mock, Type type)
        {
            return new CalculatorProxy(mock);
        }
    }

    public class Invocation : IInvocation
    {
        private Func<object> invokeBase;

        public Invocation(Func<object> invokeBase)
        {
            this.invokeBase = invokeBase;
        }

        public object Mock { get; set; }
        public object Target { get; set; }
        public MethodBase Method { get; set; }
        public object[] Arguments { get; set; }
        public object ReturnValue { get; set; }

        public void InvokeBase()
        {
            this.ReturnValue = this.invokeBase();
        }
    }
}