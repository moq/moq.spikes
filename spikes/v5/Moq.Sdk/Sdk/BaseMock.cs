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
using System.Diagnostics;

namespace Moq.Sdk
{
    /// <summary>
    /// Specific mocking libraries inherit from this base class to gain the 
    /// pipeline behavior, etc.
    /// </summary>
    public abstract class BaseMock : IMock, IProxied
    {
        // TODO: there will be an introspection API for 
        // accessing these key values for rendering.

        private IBehaviorSettings defaultBehaviors;
        private List<IInvocation> invocations = new List<IInvocation>();
        private List<IBehavior> behaviors = new List<IBehavior>();
        private IBehaviorSelectorFactory selectorFactory;

        IBehaviorSelectorFactory IMock.SelectorFactory { get { return this.selectorFactory; } }
        IBehaviorSettings IMock.DefaultBehavior { get { return this.defaultBehaviors; } }
        IList<IInvocation> IMock.Invocations { get { return this.invocations; } }
        IList<IBehavior> IMock.Behaviors { get { return this.behaviors; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMock"/> class.
        /// </summary>
        /// <param name="defaultBehaviors">The default mock behaviors.</param>
        protected BaseMock(IBehaviorSettings defaultBehaviors)
            : this(defaultBehaviors, new DefaultSelectorFactory())
        {
            Guard.NotNull(() => defaultBehaviors, defaultBehaviors);

            this.defaultBehaviors = defaultBehaviors;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseMock" /> class.
        /// </summary>
        /// <param name="defaultBehaviors">The default mock behaviors.</param>
        /// <param name="selectorFactory">The behavior selector factory.</param>
        protected BaseMock(IBehaviorSettings defaultBehaviors, IBehaviorSelectorFactory selectorFactory)
        {
            Guard.NotNull(() => defaultBehaviors, defaultBehaviors);
            Guard.NotNull(() => selectorFactory, selectorFactory);

            this.defaultBehaviors = defaultBehaviors;
            this.selectorFactory = selectorFactory;
        }

        void IProxied.Execute(IInvocation invocation)
        {
            Debug.Assert(invocation.Mock == this, "Executed invocation does not belong to this mock.");

            CallContext.LastInvocation = invocation;

            var pipeline = this.behaviors.FirstOrDefault(x => x.AppliesTo(invocation));
            if (pipeline == null)
            {
                pipeline = CallContext.GetBehavior();
                this.behaviors.Add(pipeline);
            }

            if (!CallContext.IsSettingUp)
            {
                invocations.Add(invocation);
                pipeline.ExecuteFor(invocation);
            }
            // If we're setting up the mock and the method is non-void, 
            // we must return a default value so that the proxy doesn't fail.
            else if (invocation.Method.ReturnType != typeof(void))
            {
                invocation.ReturnValue = DefaultValue.For(invocation.Method.ReturnType);
            }
       }

        protected abstract Type MockedType { get; }
    }
}
