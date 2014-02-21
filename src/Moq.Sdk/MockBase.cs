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

	/// <summary>
	/// Base class that mock libraries can use to inherit 
	/// their mock classes from.
	/// </summary>
    public abstract class MockBase : IMock
    {
		Lazy<object> instance;

		/// <summary>
		/// Initializes a new instance of the <see cref="MockBase"/> class.
		/// </summary>
        protected MockBase()
        {
			instance = new Lazy<object>(CreateObject);
            Invocations = new List<IInvocation>();
            Behaviors = new List<IBehavior>();
        }

		/// <summary>
		/// Gets the list of configured behaviors for this mock.
		/// </summary>
        public IList<IBehavior> Behaviors { get; private set; }

		/// <summary>
		/// Gets all the invocations that were performed on the mock instance.
		/// </summary>
        public IList<IInvocation> Invocations { get; private set; }

		/// <summary>
		/// Gets the mocked object instance.
		/// </summary>
		public object Object { get { return instance.Value; } }

		/// <summary>
		/// Performs an invocation on the mock.
		/// </summary>
		/// <remarks>
		/// Whether or not a behavior matches the invocation, the actual invocation
		/// is always recorded.
		/// </remarks>
		public virtual void Invoke(IInvocation invocation)
		{
			Invocations.Add(invocation);

			var behavior = Behaviors.FirstOrDefault(x => x.AppliesTo(invocation));
			if (behavior != null)
				behavior.ExecuteFor(invocation);
		}

		protected abstract object CreateObject();
	}
}