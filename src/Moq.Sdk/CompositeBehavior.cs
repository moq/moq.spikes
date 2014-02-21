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

    public class CompositeBehavior : IBehavior
    {
        private Func<IInvocation, bool> appliesTo;

        public CompositeBehavior(Func<IInvocation, bool> appliesTo)
        {
            this.Before = new List<IBehavior>();
			this.Invoke = new List<IBehavior>();
			this.After = new List<IBehavior>();
            this.Invocations = new List<IInvocation>();

            this.appliesTo = appliesTo;
        }

		public IList<IBehavior> Before { get; private set; }
		public IList<IBehavior> Invoke { get; private set; }
		public IList<IBehavior> After { get; private set; }

        public IList<IInvocation> Invocations { get; private set; }

        public bool AppliesTo(IInvocation invocation)
        {
            return this.appliesTo.Invoke(invocation);
        }

        public void ExecuteFor(IInvocation invocation)
        {
            this.Invocations.Add(invocation);

            ExecuteBehaviors(this.Before, invocation);
            ExecuteBehaviors(this.Invoke, invocation);
            ExecuteBehaviors(this.After, invocation);
        }

		private void ExecuteBehaviors(IEnumerable<IBehavior> aspects, IInvocation invocation)
        {
            foreach (var aspect in aspects.Where(x => x.AppliesTo(invocation)))
            {
				aspect.ExecuteFor(invocation);
            }
        }
    }
}