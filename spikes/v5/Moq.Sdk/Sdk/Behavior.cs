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

namespace Moq.Sdk
{
	/// <summary>
	/// Represents the configured behaviors for a mock invocation.
	/// </summary>
	[Serializable]
	public class Behavior : IBehavior
	{
		// All this remoting crap is necessary because I'm using the 
		// remoting CallContext so that we're smarter than just 
		// Thread.SetData for multi-threaded runners and code under 
		// test. We need these attributes for test runners that 
		// live inside VS and use cross-AppDomain remoting to run 
		// the tests. Otherwise, the non-serializable implementations 
		// of any of these components will cause the test to fail or 
		// even hang.
		[NonSerialized]
		private List<IAspect> before = new List<IAspect>();
		[NonSerialized]
		private List<IAspect> invoke = new List<IAspect>();
		[NonSerialized]
		private List<IAspect> after = new List<IAspect>();
        [NonSerialized]
        private IBehaviorSelector selector;
        [NonSerialized]
        private List<IInvocation> executions = new List<IInvocation>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Behavior"/> class.
		/// </summary>
		public Behavior(IBehaviorSettings settings, IBehaviorSelector selector)
		{
            Guard.NotNull(() => settings, settings);
            Guard.NotNull(() => selector, selector);

            this.before = new List<IAspect>(settings.Before);
            this.invoke = new List<IAspect>(settings.Invoke);
            this.after = new List<IAspect>(settings.After);

            this.selector = selector;
		}

        /// <summary>
        /// Whether this behavior applies to the given invocation.
        /// </summary>
		public bool AppliesTo(IInvocation invocation)
		{
            return this.selector.AppliesTo(invocation);
		}

        /// <summary>
        /// Executes the behavior for the given invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <exception cref="System.InvalidOperationException">This behavior does not apply to the received invocation.</exception>
		public void ExecuteFor(IInvocation invocation)
		{
			if (!AppliesTo(invocation))
				throw new InvalidOperationException("This behavior does not apply to the received invocation.");

			ExecuteBehaviors(this.Before, invocation);
			ExecuteBehaviors(this.Invoke, invocation);
			ExecuteBehaviors(this.After, invocation);

            executions.Add(invocation);
		}

        /// <summary>
        /// Gets all the invocations that this behavior has executed for.
        /// </summary>
        public IEnumerable<IInvocation> Executions { get { return this.executions; } }

		/// <summary>
		/// Gets the list of behaviors that will run before the target 
		/// invocation on the mock.
		/// </summary>
		public IList<IAspect> Before { get { return this.before; } }

		/// <summary>
		/// Gets the list of behaviors that will run at the target 
		/// invocation on the mock.
		/// </summary>
		public IList<IAspect> Invoke { get { return this.invoke; } }

		/// <summary>
		/// Gets the list of behaviors that will run after the target 
		/// invocation on the mock.
		/// </summary>
		public IList<IAspect> After { get { return this.after; } }

        private void ExecuteBehaviors(IEnumerable<IAspect> behaviors, IInvocation invocation)
        {
            behaviors
                .Where(x => x.IsActiveFor(invocation))
                // Avoid concurrency issues
                .ToArray()
                // Execute lazily one by one, grabbing the resulting action
                .Select(x => x.ExecuteFor(invocation))
                // Only execute as long as we were getting Continue
                .TakeWhile(x => x == AspectAction.Continue)
                // This call is the one that actually causes everything to run
                .ToArray();
        }


        #region Equality

        public bool Equals(Behavior other)
        {
            return Behavior.Equals(this, other);
        }

        public override bool Equals(object obj)
        {
            return Behavior.Equals(this, obj as Behavior);
        }

        public static bool Equals(Behavior obj1, Behavior obj2)
        {
            if (Object.Equals(null, obj1) ||
                Object.Equals(null, obj2) ||
                obj1.GetType() != obj2.GetType())
                return false;

            if (Object.ReferenceEquals(obj1, obj2)) return true;

            // Compare your object properties
            // return obj1.Id == obj2.Id;

            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();

            // Implement your hashing typically by combining 
            // the hashes of your fields
            // int hash = id.GetHashCode();
            // hash = hash ^ title.GetHashCode();

            // return hash;
        }

        #endregion
    }
}
