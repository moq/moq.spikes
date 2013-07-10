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
	///	Used by static extension methods to provide fluent APIs  
	///	without using setup expressions.
	/// </summary>
	public static class CallContext
	{
        private static ContextData<Stack<bool>> settingUp = ContextData.Create<Stack<bool>>(new Stack<bool>());
        private static ContextData<IInvocation> lastInvocation = ContextData.Create<IInvocation>();
		private static ContextData<Stack<IArgumentMatcher>> argumentMatchers = ContextData.Create(new Stack<IArgumentMatcher>());
		private static ContextData<Behavior> lastPipeline = ContextData.Create<Behavior>();

		/// <summary>
		/// Gets or sets the last invocation performed on the mock.
		/// </summary>
		public static IInvocation LastInvocation
		{
			get { return lastInvocation.Value; }
			internal set { lastInvocation.Value = value; lastPipeline.Value = null; }
		}

        /// <summary>
        /// Gets a value indicating whether mocks are being set up so that 
        /// actual invocations should be suspended.
        /// </summary>
        public static bool IsSettingUp
        {
            get { return settingUp.Value.Count != 0; }
        }

		/// <summary>
		/// Adds an argument matcher for the current mock invocation.
		/// </summary>
		public static void AddMatcher(IArgumentMatcher matcher)
		{
			argumentMatchers.Value.Push(matcher);
			lastPipeline.Value = null;
		}

		/// <summary>
		/// Gets the behavior for the current invocation.
		/// </summary>
		public static IBehavior GetBehavior()
		{
			if (lastPipeline.Value != null)
				return lastPipeline.Value;

            // This behavior is key to enabling static extension methods 
            // on arbitrary types and allow transparent "recording" 
            // behavior. The moment an extension method such as 
            // Returns<T>(this T target, T value) invokes this GetBehavior 
            // method, we assume the most recent call (the retrieval of 
            // a property value, or invocation of a method that returns
            // some value T) was intended for matching purposes only, 
            // so we need to "unwind" it and build a pipeline with it
            // so that it can be further customized by the extension method
            // implementation.

            // Here's what we do:
			// Use last invocation, argument matchers, 
			// actual invocation arguments, build 
			// list of matchers for all arguments, 
			// and add pipeline to mock.
			if (lastInvocation.Value == null)
				throw new InvalidOperationException("There is no mock being called.");

			var currentMatchers = argumentMatchers.Value;
			var finalMatchers = new List<IArgumentMatcher>();
			var invocation = lastInvocation.Value;
			var parameters = invocation.Method.GetParameters();

			for (int i = 0; i < invocation.Arguments.Length; i++)
			{
				var argument = invocation.Arguments[i];
				var parameter = parameters[i];

				if (Object.Equals(argument, DefaultValue.For(parameter.ParameterType)) &&
					currentMatchers.Count != 0 &&
					parameter.ParameterType.IsAssignableFrom(currentMatchers.Peek().ArgumentType))
				{
					finalMatchers.Add(currentMatchers.Pop());
				}
				else
				{
					finalMatchers.Add(new Arguments.ConstantMatcher(parameter.ParameterType, argument));
				}
			}

            lastPipeline.Value = new Behavior(
                invocation.Mock.DefaultBehavior,
                invocation.Mock.SelectorFactory.CreateSelector(invocation, finalMatchers));

            invocation.Mock.Invocations.Remove(CallContext.LastInvocation);

			return lastPipeline.Value;
		}

        internal static void BeginSetup()
        {
            settingUp.Value.Push(true);
        }

        internal static void EndSetup()
        {
            settingUp.Value.Pop();
        }
	}
}
