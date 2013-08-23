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
    using System.Reflection;

    public abstract class MockBase : IMock
    {
        protected MockBase()
        {
            this.Invocations = new List<IInvocation>();
            this.Behaviors = new List<Behavior>();
        }

        public virtual void Invoke(IInvocation invocation)
        {
            this.Invocations.Add(invocation);

            var behavior = this.Behaviors.FirstOrDefault(x => x.AppliesTo(invocation));
            if (behavior != null)
                behavior.ExecuteFor(invocation);
        }

        public IList<Behavior> Behaviors { get; private set; }
        public IList<IInvocation> Invocations { get; private set; }
    }
}