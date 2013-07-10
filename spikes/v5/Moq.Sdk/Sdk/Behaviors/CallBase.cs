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

namespace Moq.Sdk.Behaviors
{
	/// <summary>
	/// Behavior that invokes the base class implementation,  
    /// if any.
	/// </summary>
	public class CallBase : IAspect
	{
        /// <summary>
        /// Determines whether this behavior is active for
        /// the given invocation, which is only true if the 
        /// invocation has a non-void return type.
        /// </summary>
        public bool IsActiveFor(IInvocation invocation)
		{
            return invocation.Method.ReturnType != typeof(void);
        }

        /// <summary>
        /// Sets the return value to the default one according 
        /// to the return type, as implemented by <see cref="DefaultValue"/>.
        /// </summary>
        public AspectAction ExecuteFor(IInvocation invocation)
		{
            invocation.InvokeBase();

			return AspectAction.Stop;
		}
	}
}
