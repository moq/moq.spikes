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

    public class DelegateAspect : IAspect
    {
        private Func<IInvocation, bool> appliesTo;
        private Func<IInvocation, BehaviorAction> executeFor;

        public DelegateAspect(Func<IInvocation, bool> appliesTo, Func<IInvocation, BehaviorAction> executeFor)
        {
            this.appliesTo = appliesTo;
            this.executeFor = executeFor;
        }

        public bool AppliesTo(IInvocation invocation)
        {
            return this.appliesTo.Invoke(invocation);
        }

        public BehaviorAction ExecuteFor(IInvocation invocation)
        {
            return this.executeFor.Invoke(invocation);
        }
    }
}