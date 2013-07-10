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
	/// Matches arguments against a fixed constant value.
	/// </summary>
	public class ConstantMatcher : IArgumentMatcher
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConstantMatcher"/> class.
		/// </summary>
		/// <param name="argumentType">Type of the argument to match.</param>
		/// <param name="matchValue">The value to match against.</param>
		public ConstantMatcher(Type argumentType, object matchValue)
		{
			this.ArgumentType = argumentType;
			this.MatchValue = matchValue;
		}

		/// <summary>
		/// Gets the type of the argument this matcher supports.
		/// </summary>
		public Type ArgumentType { get; private set; }

		/// <summary>
		/// The value to match against invocation arguments.
		/// </summary>
		public object MatchValue { get; private set; }

		/// <summary>
		/// Evaluates whether the given value equals the <see cref="MatchValue"/> 
		/// received in the constructor, using default object equality behavior.
		/// </summary>
		public bool Matches(object value)
		{
			return object.Equals(value, this.MatchValue);
		}


        #region Equality

        public override bool Equals(object obj)
        {
            return ConstantMatcher.Equals(this, obj as ConstantMatcher);
        }

        public override int GetHashCode()
        {
            return Tuple.Create<Type, object>(this.ArgumentType, this.MatchValue).GetHashCode(); 
        }

        private static bool Equals(ConstantMatcher obj1, ConstantMatcher obj2)
        {
            if (Object.Equals(null, obj1) ||
                Object.Equals(null, obj2) ||
                obj1.GetType() != obj2.GetType())
                return false;

            if (Object.ReferenceEquals(obj1, obj2)) return true;

            return Tuple.Create<Type, object>(obj1.ArgumentType, obj1.MatchValue).Equals(
                Tuple.Create<Type, object>(obj2.ArgumentType, obj2.MatchValue));
        }

        #endregion
	}
}
