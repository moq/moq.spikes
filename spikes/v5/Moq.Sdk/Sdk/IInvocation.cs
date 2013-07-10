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
using System.Reflection;

namespace Moq.Sdk
{
	/// <summary>
	/// Represents an invocation to the mock, intercepted by 
	/// the proxy created by a <see cref="IProxyFactory"/>, 
	/// and used to abstract the underlying proxy mechanism.
	/// </summary>
	public interface IInvocation
	{
		/// <summary>
		/// Gets the mock the invocation was performed on.
		/// </summary>
		IMock Mock { get; }

		/// <summary>
		/// Gets the arguments of the invocation.
		/// </summary>
		object[] Arguments { get; }

		/// <summary>
		/// Gets the method being invoked.
		/// </summary>
		MethodInfo Method { get; }

		/// <summary>
		/// Gets the mock object invocation target.
		/// </summary>
		object Target { get; }

		/// <summary>
		/// Gets or sets the return value for this invocation.
		/// </summary>
		object ReturnValue { get; set; }

		/// <summary>
		/// Invokes the base implementation on the type, if any.
		/// </summary>
		void InvokeBase();
	}
}
