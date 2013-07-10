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
	/// Defines an aspect of the behavior of a mock.
	/// </summary>
	public interface IAspect
	{
		/// <summary>
		/// Determines whether this behavior aspect is active for 
		/// the given invocation. 
		/// </summary>
		/// <remarks>
		/// This allows conditional behaviors, which 
		/// would enable some scenarios like sequencing.
		/// </remarks>
		bool IsActiveFor(IInvocation invocation);

		/// <summary>
		/// Executes the behavior for the given invocation.
		/// </summary>
		/// <param name="invocation">The invocation that triggers the behavior.</param>
		/// <returns>A value indicating whether the behavior should continue or stop 
        /// processing other aspects for the particular phase of the invocation.</returns>
		AspectAction ExecuteFor(IInvocation invocation);
	}
}
