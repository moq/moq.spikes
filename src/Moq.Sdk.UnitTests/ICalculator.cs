#region Apache Licensed
/*
 Copyright 2013 Clarius Consulting, Daniel Cazzulino

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License.
*/
#endregion

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