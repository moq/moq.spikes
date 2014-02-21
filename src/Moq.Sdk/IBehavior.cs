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
	/// A behavior encapsulates both a matching condition for an invocation 
	/// (<see cref="AppliesTo"/>) as well as its execution (<see cref="ExecuteFor"/>).
	/// It represents what most mocking libraries call a "setup" or "expectation".
	/// </summary>
	/// <remarks>
	/// Behaviors track the number of times they have been invoked, for later 
	/// verification purposes.
	/// </remarks>
    public interface IBehavior
    {
		/// <summary>
		/// Gets the list of invocations performed so far for this 
		/// behavior.
		/// </summary>
		IList<IInvocation> Invocations { get; }

		/// <summary>
		/// Determines whether the current behavior applies to the 
		/// given invocation.
		/// </summary>
        bool AppliesTo(IInvocation invocation);

		/// <summary>
		/// Executes the behavior for the given invocation.
		/// </summary>
        void ExecuteFor(IInvocation invocation);
    }
}
