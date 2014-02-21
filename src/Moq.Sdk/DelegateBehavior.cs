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

    public class DelegateBehavior : IBehavior
    {
        private Func<IInvocation, bool> appliesTo;
        private Action<IInvocation> executeFor;

        public DelegateBehavior(Action<IInvocation> execute, Func<IInvocation, bool> appliesTo = null)
        {
            this.appliesTo = appliesTo ?? (i => true);
            this.executeFor = execute;
			this.Invocations = new List<IInvocation>();
        }

		public IList<IInvocation> Invocations { get; private set; }

        public bool AppliesTo(IInvocation invocation)
        {
            return this.appliesTo.Invoke(invocation);
        }

        public void ExecuteFor(IInvocation invocation)
        {
			this.Invocations.Add(invocation);
			this.executeFor(invocation);
        }
    }
}