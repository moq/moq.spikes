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
using Moq.Sdk;
using Moq.Sdk.Behaviors;

namespace Moq
{
	/// <summary>
	/// Provides Moq language features as extension methods 
	/// to properties and method calls.
	/// </summary>
	public static class MoqFluentExtensions
	{
		/// <summary>
		/// Specifies that the previous property or method invocation 
		/// will return the given value when called.
		/// </summary>
		public static void Returns<T>(this T target, T value)
		{
			CallContext
                .GetBehavior()
                .Invoke
                .Insert(0, new ReturnSingleValue(value));
		}
	}
}
