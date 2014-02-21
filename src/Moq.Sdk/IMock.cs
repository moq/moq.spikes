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

namespace Moq.Sdk
{
    using System;
    using System.Collections.Generic;

	/// <summary>
	/// Represents a mocked object with its configured <see cref="Behaviors"/> 
	/// and performed <see cref="Invocations"/>.
	/// </summary>
    public interface IMock
    {
		/// <summary>
		/// Gets the list of configured behaviors for this mock.
		/// </summary>
        IList<IBehavior> Behaviors { get; }

		/// <summary>
		/// Gets all the invocations that were performed on the mock instance.
		/// </summary>
		IList<IInvocation> Invocations { get; }

		/// <summary>
		/// Gets the mocked object instance.
		/// </summary>
		object Object { get; }

		/// <summary>
		/// Performs an invocation on the mock.
		/// </summary>
		/// <remarks>
		/// Whether or not a behavior matches the invocation, the actual invocation 
		/// is always recorded.
		/// </remarks>
        void Invoke(IInvocation invocation);
    }
}