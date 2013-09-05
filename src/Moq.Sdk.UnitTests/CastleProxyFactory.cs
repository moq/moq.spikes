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
    using Castle.DynamicProxy;
    using System;
    using System.Linq;
    using System.Reflection;
    using ICastleInvocation = Castle.DynamicProxy.IInvocation;

    public class CastleProxyFactory : IProxyFactory
    {
        private static readonly ProxyGenerator generator = new ProxyGenerator();

        public object CreateProxy(IMock mock, Type type)
        {
            if (type.IsInterface)
            {
                return generator.CreateInterfaceProxyWithoutTarget(type, new ForwardingInterceptor(mock));
            }

            return generator.CreateClassProxy(type, new ForwardingInterceptor(mock));
        }

        // Forwards intercepted calls to the proxied mock.
        private class ForwardingInterceptor : IInterceptor
        {
            private IMock mock;

            internal ForwardingInterceptor(IMock mock)
            {
                this.mock = mock;
            }

            public void Intercept(ICastleInvocation invocation)
            {
                this.mock.Invoke(new InvocationAdapter(invocation, this.mock));
            }
        }

        private class InvocationAdapter : Moq.Sdk.IInvocation
        {
            private ICastleInvocation invocation;
            private IMock mock;

            internal InvocationAdapter(ICastleInvocation invocation, IMock mock)
            {
                this.invocation = invocation;
                this.mock = mock;
            }

            public IMock Mock
            {
                get { return this.mock as IMock; }
            }

            public object[] Arguments
            {
                get { return this.invocation.Arguments; }
            }

            public MethodBase Method
            {
                get { return this.invocation.Method; }
            }

            public object ReturnValue
            {
                get { return this.invocation.ReturnValue; }
                set { this.invocation.ReturnValue = value; }
            }

            public void InvokeBase()
            {
                this.invocation.Proceed();
            }

            public object Target
            {
                get { return this.invocation.InvocationTarget; }
            }
        }
    }
}