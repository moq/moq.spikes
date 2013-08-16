namespace Moq
{
    using System;
    using System.Linq;
    using System.Reflection;

    public class MockBase : IProxied
    {
        private object defaultValue;
        private bool callBase;

        public IInvocation LastCall { get; private set; }

        public MockBase(object defaultValue = null, bool callBase = false)
        {
            this.defaultValue = defaultValue;
            this.callBase = callBase;
        }

        public void Execute(IInvocation invocation)
        {
            this.LastCall = invocation;
            if (callBase)
                invocation.InvokeBase();
            else
                invocation.ReturnValue = this.defaultValue;
        }
    }
}