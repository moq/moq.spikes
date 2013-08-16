namespace Moq
{
    using System;
    using System.Reflection;

    public interface IProxied
    {
        void Execute(IInvocation invocation);
    }
}