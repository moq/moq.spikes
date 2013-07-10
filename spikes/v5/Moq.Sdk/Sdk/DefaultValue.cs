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
	/// Calculates default values for given types.
	/// </summary>
	public static class DefaultValue
	{
		/// <summary>
		/// Calculates the default value for the given type <typeparamref name="T"/>.
		/// </summary>
		public static T For<T>()
		{
			return (T)For(typeof(T));
		}

		/// <summary>
		/// Calculates the default value for the given type <paramref name="type"/>
		/// </summary>
		public static object For(Type type)
		{
			if (type.IsValueType)
				return Activator.CreateInstance(type);

			return null;
		}
	}
}
