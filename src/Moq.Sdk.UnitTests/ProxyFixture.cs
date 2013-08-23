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
    using System.Linq;
    using System.Reflection;
    using Xunit;

    public class ProxyFixture
    {
        [Fact]
        public void when_proxy_factory_invoked_then_gets_implementation_of_interface()
        {
            var factory = new ProxyFactory();
            var mock = new MockBase();

            var proxy = factory.CreateProxy(mock, typeof(ICalculator));

            Assert.NotNull(proxy);
            Assert.True(proxy is ICalculator);
        }

        [Fact]
        public void when_proxy_instance_called_then_calls_back_into_mock_behavior()
        {
            var factory = new ProxyFactory();
            var mock = new MockBase();

            var proxy = (ICalculator)factory.CreateProxy(mock, typeof(ICalculator));

            proxy.Add(5, 5);

            Assert.NotNull(mock.LastCall);
        }

        [Fact]
        public void when_proxy_instance_called_then_can_retrieve_invocation_method()
        {
            var factory = new ProxyFactory();
            var mock = new MockBase();

            var proxy = (ICalculator)factory.CreateProxy(mock, typeof(ICalculator));

            proxy.Add(5, 5);

            Assert.Equal(typeof(ICalculator).GetMethod("Add"), mock.LastCall.Method);
        }

        [Fact]
        public void when_proxy_instance_called_then_can_retrieve_invocation_arguments()
        {
            var factory = new ProxyFactory();
            var mock = new MockBase();

            var proxy = (ICalculator)factory.CreateProxy(mock, typeof(ICalculator));

            proxy.Add(5, 10);

            Assert.Equal(5, (int)mock.LastCall.Arguments[0]);
            Assert.Equal(10, (int)mock.LastCall.Arguments[1]);
        }

        [Fact]
        public void when_proxy_instance_called_then_can_retrieve_mock_from_invocation()
        {
            var factory = new ProxyFactory();
            var mock = new MockBase();

            var proxy = (ICalculator)factory.CreateProxy(mock, typeof(ICalculator));

            proxy.Add(5, 10);

            Assert.Same(mock, mock.LastCall.Mock);
        }

        [Fact]
        public void when_proxy_instance_called_then_can_retrieve_invocation_target()
        {
            var factory = new ProxyFactory();
            var mock = new MockBase();

            var proxy = (ICalculator)factory.CreateProxy(mock, typeof(ICalculator));

            proxy.Add(5, 10);

            Assert.Same(proxy, mock.LastCall.Target);
        }

        [Fact]
        public void when_interface_proxy_called_then_can_set_return_value()
        {
            var factory = new ProxyFactory();
            var mock = new MockBase(100);

            var proxy = (ICalculator)factory.CreateProxy(mock, typeof(ICalculator));

            var result = proxy.Add(5, 10);

            Assert.Equal(100, result);
        }

        [Fact]
        public void when_class_proxy_instance_called_then_can_invoke_base_implementation_from_invocation()
        {
            var factory = new ProxyFactory();
            var mock = new MockBase(callBase: true);

            var proxy = (ICalculator)factory.CreateProxy(mock, typeof(ICalculator));

            var result = proxy.Add(5, 10);

            Assert.Equal(15, result);
        }
    }
}