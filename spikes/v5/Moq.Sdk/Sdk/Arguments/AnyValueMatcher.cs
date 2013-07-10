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

namespace Moq.Sdk.Arguments
{
	/// <summary>
	/// Matches any argument with the given type <typeparamref name="T"/>, 
	/// including <see langword="null"/> if the type is a reference type 
	/// or a nullable value type.
	/// </summary>
	/// <typeparam name="T">Type of argument to match.</typeparam>
	public class AnyValueMatcher<T> : IArgumentMatcher
	{
		private static bool IsNullable = typeof(T).IsGenericType &&
			typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>);

		static AnyValueMatcher()
		{
			Instance = new AnyValueMatcher<T>();
		}

		private AnyValueMatcher()
		{
		}

		/// <summary>
		/// Gets the singleton instance of this matcher.
		/// </summary>
		public static IArgumentMatcher Instance { get; private set; }

		/// <summary>
		/// Gets the type of the argument this matcher supports.
		/// </summary>
		public Type ArgumentType { get { return typeof(T); } }

		/// <summary>
		/// Evaluates whether the given value matches this instance.
		/// </summary>
		public bool Matches(object value)
		{
			// Non-nullable value types never match against a null value.
			if (typeof(T).IsValueType && !IsNullable && value == null)
				return false;

			return value == null ||
				typeof(T).IsAssignableFrom(value.GetType());
		}

        #region Equality

        public override bool Equals(object obj)
        {
            return AnyValueMatcher<T>.Equals(this, obj as AnyValueMatcher<T>);
        }

        public override int GetHashCode()
        {
            return typeof(T).GetHashCode();
        }

        private static bool Equals(AnyValueMatcher<T> obj1, AnyValueMatcher<T> obj2)
        {
            if (Object.Equals(null, obj1) ||
                Object.Equals(null, obj2) ||
                obj1.GetType() != obj2.GetType())
                return false;

            // If both are non-null and they have the same type, 
            // they match essentially any value of the same type, 
            // so they are equal.
            return true;
        }

        #endregion
    }
}
