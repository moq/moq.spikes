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
using Moq.Sdk.Arguments;

namespace Moq
{
	/// <summary>
	/// Provides flexible argument matching for specifing 
	/// mock behaviors.
	/// </summary>
	/// <typeparam name="T">Type of argument to match.</typeparam>
	public static class Any<T>
	{
		/// <summary>
		/// Matches any argument with the given type <typeparam name="T"/>, 
		/// including <see langword="null"/> if the type is a reference type 
		/// or a nullable value type.
		/// </summary>
		public static T Value
		{
			get
			{
				CallContext.AddMatcher(AnyValueMatcher<T>.Instance);
				return default(T);
			}
		}
	}
}
