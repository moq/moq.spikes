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
    using System.Linq;

    /// <summary>
    /// Specifies a mock's default set of behavior aspects.
    /// </summary>
    public class BehaviorSettings : IBehaviorSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BehaviorSettings" /> class.
        /// </summary>
        /// <param name="defaultInvoke">The default invocation behavior.</param>
        public BehaviorSettings(IAspect defaultInvoke)
        {
            Guard.NotNull(() => defaultInvoke, defaultInvoke);

            this.Before = new List<IAspect>();
            this.Invoke = new List<IAspect> { defaultInvoke };
            this.After = new List<IAspect>();
        }

        /// <summary>
        /// Gets the list of aspects that will run before the target 
        /// invocation on the mock.
        /// </summary>
        public IList<IAspect> Before { get; private set; }

        /// <summary>
        /// Gets the list of aspects that will run at the target 
        /// invocation on the mock.
        /// </summary>
        public IList<IAspect> Invoke { get; private set; }

        /// <summary>
        /// Gets the list of aspects that will run after the target 
        /// invocation on the mock.
        /// </summary>
        public IList<IAspect> After { get; private set; }
    }
}