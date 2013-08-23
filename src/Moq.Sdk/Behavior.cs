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

    public class Behavior
    {
        private Func<IInvocation, bool> appliesTo;
        private Action<IInvocation> executeFor;

        public Behavior(Func<IInvocation, bool> appliesTo, Action<IInvocation> executeFor)
        {
            this.Before = new List<Aspect>();
            this.Invoke = new List<Aspect>();
            this.After = new List<Aspect>();
            this.Invocations = new List<IInvocation>();

            this.appliesTo = appliesTo;
            this.executeFor = executeFor;
        }

        public IList<Aspect> Before { get; private set; }
        public IList<Aspect> Invoke { get; private set; }
        public IList<Aspect> After { get; private set; }

        public IList<IInvocation> Invocations { get; private set; }

        public bool AppliesTo(IInvocation invocation)
        {
            return this.appliesTo.Invoke(invocation);
        }

        public void ExecuteFor(IInvocation invocation)
        {
            this.Invocations.Add(invocation);

            this.executeFor(invocation);

            ExecuteAspects(this.Before, invocation);
            ExecuteAspects(this.Invoke, invocation);
            ExecuteAspects(this.After, invocation);
        }

        private void ExecuteAspects(IEnumerable<Aspect> aspects, IInvocation invocation)
        {
            foreach (var aspect in aspects)
            {
                aspect.ExecuteFor(invocation);
            }
        }
    }
}