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

using System;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using Castle.DynamicProxy;
using Castle.DynamicProxy.Generators;
using System.Diagnostics.CodeAnalysis;

using CastleInterceptor = Castle.DynamicProxy.IInterceptor;
using CastleInvocation = Castle.DynamicProxy.IInvocation;
using Moq.Sdk;

namespace Moq
{
	/// <summary>
	/// Provides a proxy factory for mocks based on Castle Dynamic Proxy.
	/// </summary>
	internal class CastleProxyFactory : IProxyFactory
	{
		private static readonly ProxyGenerator generator = CreateProxyGenerator();

		[SuppressMessage("Microsoft.Performance", "CA1810:InitializeReferenceTypeStaticFieldsInline", Justification = "By Design")]
		static CastleProxyFactory()
		{
#pragma warning disable 618
			AttributesToAvoidReplicating.Add<SecurityPermissionAttribute>();
#pragma warning restore 618

#if !SILVERLIGHT
			AttributesToAvoidReplicating.Add<ReflectionPermissionAttribute>();
			AttributesToAvoidReplicating.Add<PermissionSetAttribute>();
			AttributesToAvoidReplicating.Add<System.Runtime.InteropServices.MarshalAsAttribute>();
#if !NET3x
			AttributesToAvoidReplicating.Add<System.Runtime.InteropServices.TypeIdentifierAttribute>();
#endif
#endif
		}

		public T CreateProxy<T>(IProxied interceptable, Type[] interfaces, object[] arguments)
		{
			var mockType = typeof(T);
			interfaces = interfaces.Concat(new[] { typeof(IMocked) }).ToArray();

			if (mockType.IsInterface)
			{
				return (T)generator.CreateInterfaceProxyWithoutTarget(mockType, interfaces, new ForwardingInterceptor(interceptable));
			}

			try
			{
				return (T)generator.CreateClassProxy(mockType, interfaces, new ProxyGenerationOptions(), arguments, new ForwardingInterceptor(interceptable));
			}
			catch (TypeLoadException e)
			{
				throw;
				//throw new ArgumentException(Resources.InvalidMockClass, e);
			}
			catch (MissingMethodException e)
			{
				throw;
				//throw new ArgumentException(Resources.ConstructorNotFound, e);
			}
		}

		private static ProxyGenerator CreateProxyGenerator()
		{
			return new ProxyGenerator();
		}

		// Forwards intercepted calls to the proxied mock.
		private class ForwardingInterceptor : CastleInterceptor
		{
			private IProxied mock;

			internal ForwardingInterceptor(IProxied mock)
			{
				this.mock = mock;
			}

			public void Intercept(CastleInvocation invocation)
			{
				if (invocation.Method.DeclaringType == typeof(IMocked))
				{
					// "Mixin" of IMocked.Mock
					invocation.ReturnValue = this.mock;
					return;
				}

				this.mock.Execute(new InvocationAdapter(invocation, this.mock));
			}
		}

		// Adapts the Castle invocation contract to Moq's.
		[Serializable]
		private class InvocationAdapter : Moq.Sdk.IInvocation
		{
			[NonSerialized]
			private CastleInvocation invocation;
			[NonSerialized]
			private IProxied mock;

			internal InvocationAdapter(CastleInvocation invocation, IProxied mock)
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

			public MethodInfo Method
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