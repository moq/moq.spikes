namespace Moq.Sdk.UnitTests
{
    using System;
    using System.Reflection;

    public interface ICalculator
    {
        int Add(int x, int y);
    }

    public class Calculator : ICalculator
    {
        public virtual int Add(int x, int y)
        {
            return x + y;
        }
    }

    public class CalculatorProxy : Calculator
    {
        private IProxied mock;

        public CalculatorProxy(IProxied mock)
        {
            this.mock = mock;
        }

        public override int Add(int x, int y)
        {
            var invocation = new Invocation(() => base.Add(x, y))
            {
                Mock = mock,
                Target = this,
                Method = typeof(ICalculator).GetMethod("Add"),
                Arguments = new object[] { x, y }
            };

            mock.Execute(invocation);

            return (int)invocation.ReturnValue;
        }
    }
}