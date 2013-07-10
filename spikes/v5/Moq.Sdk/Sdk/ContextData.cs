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
using Remoting = System.Runtime.Remoting.Messaging;

namespace Moq.Sdk
{
	// Internal helpers for strong-typed values 
	// in the CallContext.
	internal static class ContextData
	{
		public static ContextData<T> Create<T>()
		{
			return new ContextData<T>();
		}

		public static ContextData<T> Create<T>(T value)
		{
			return new ContextData<T>(value);
		}
	}

	internal class ContextData<T>
	{
		private string slotName = Guid.NewGuid().ToString();

		public ContextData()
		{
		}

		public ContextData(T initialValue)
		{
			this.Value = initialValue;
		}

		public T Value
		{
			get
			{
				var contextValue = Remoting.CallContext.LogicalGetData(this.slotName);
				if (contextValue != null)
					return (T)contextValue;

				return default(T);
			}
			set
			{
				Remoting.CallContext.LogicalSetData(this.slotName, value);
			}
		}
	}
}
