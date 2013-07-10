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
using System.Diagnostics;

// This stuff was given to us by Peli, to make Moq friendly to Pex.
internal sealed class __ProtectAttribute : Attribute
{
}

namespace Moq.Sdk
{
	[__Protect]
	[DebuggerStepThrough]
	public static class PexProtector
	{
		public static void Invoke(Action action)
		{
			action();
		}

		public static T Invoke<T>(Func<T> function)
		{
			return function();
		}
	}
}