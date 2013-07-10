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
    using System.Linq;

    /// <summary>
    /// Disables mock invocations while setups are being performed. 
    /// As soon as the setup scope is disposed, the mocks will go 
    /// back to their regular invocation behavior according to their 
    /// configured behaviors.
    /// </summary>
    public class SetupScope : IDisposable
    {
        /// <summary>
        /// Begins the setup scope, temporarily disabing mock normal 
        /// invocation behavior.
        /// </summary>
        public SetupScope()
        {
            CallContext.BeginSetup();
        }

        /// <summary>
        /// Reactivates the mock normal invocation behavior.
        /// </summary>
        public void Dispose()
        {
            CallContext.EndSetup();
        }
    }
}