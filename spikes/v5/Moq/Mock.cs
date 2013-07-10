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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moq.Sdk;
using System.Diagnostics;
using Moq.Sdk.Behaviors;

namespace Moq
{
	/// <summary>
	/// Allows creating mocks.
	/// </summary>
	public abstract class Mock : BaseMock
	{
        protected Mock(IBehaviorSettings defaultBehaviors)
            : base(defaultBehaviors)
        {
        }

        protected Mock(IBehaviorSettings defaultBehaviors, IBehaviorSelectorFactory selectorFactory)
            : base(defaultBehaviors, selectorFactory)
        {
        }

        public static IDisposable Setup()
        {
            return new SetupScope();
        }

		/// <summary>
		/// Creates a mock of the given type <typeparamref name="T"/>.
		/// </summary>
		/// <returns>An instance of the mocked object.</returns>
		public static T Of<T>()
			where T : class
		{
			return new Mock<T>(new CastleProxyFactory()).Object;
		}
	}

	public class Mock<T> : Mock
		where T : class
	{
		private IProxyFactory proxyFactory;
		private T instance;

		internal protected Mock(IProxyFactory proxyFactory)
            : base(typeof(T).IsInterface ? new BehaviorSettings(new ReturnDefaultValue()) : new BehaviorSettings(new CallBase()))
		{
			this.proxyFactory = proxyFactory;
		}

		public T Object
		{
			// Lazily creates the proxy on first acccess. 
			// This is needed because we need to allow the 
			// user to add more interfaces to the mock before 
			// creating the proxy.
			get { return this.instance ?? this.InitializeInstance(); }
		}

		protected override Type MockedType { get { return typeof(T); } }

		private T InitializeInstance()
		{
			// This is required to make Moq friendly to Pex.
			return PexProtector.Invoke(() =>
			{
				this.instance = proxyFactory.CreateProxy<T>(
					this,
					new Type[0],
					new object[0]
					// TODO: add support for adding interfaces.
					/*this.ImplementedInterfaces.ToArray(),
					this.constructorArguments */);

				return this.instance;
			});
		}
	}
}
