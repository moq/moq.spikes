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

namespace Moq.Sdk
{
	/// <summary>
	/// Factory that creates proxies for mocks.
	/// </summary>
	public interface IProxyFactory
	{
		/// <summary>
		/// Creates the proxy instance requested by the given mock.
		/// </summary>
		/// <typeparam name="T">Main class or interface to proxy.</typeparam>
		/// <param name="mock">The mock instance.</param>
		/// <param name="interfaces">The additional interfaces, if any, to implement in the proxy.</param>
		/// <param name="arguments">The constructor arguments for the base class being mocked, if any. 
        /// Not valid when <typeparamref name="T"/> is an interface.</param>
		T CreateProxy<T>(IProxied mock, Type[] interfaces, object[] arguments);
	}
}
