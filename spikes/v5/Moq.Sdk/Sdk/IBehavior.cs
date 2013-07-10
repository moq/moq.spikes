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
    /// Exposes a mock behavior.
    /// </summary>
    public interface IBehavior : IBehaviorSettings, IBehaviorSelector
    {
        /// <summary>
        /// Executes the behavior for the given invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        void ExecuteFor(IInvocation invocation);

        /// <summary>
        /// Gets all the invocations that this behavior has executed for.
        /// </summary>
        IEnumerable<IInvocation> Executions { get; }
    }
}
