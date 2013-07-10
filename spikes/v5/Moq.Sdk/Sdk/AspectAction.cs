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
	/// Value returned by <see cref="IAspect.ExecuteFor"/> to 
	/// indicate whether to continue or stop processing other 
    /// aspects in the current phase (i.e. <see cref="Behavior.Before"/>).
	/// </summary>
	public enum AspectAction
	{
		/// <summary>
		/// Continue invoking other behaviors in the pipeline phase.
		/// </summary>
		Continue,

		/// <summary>
		/// Do not continue evaluating other behaviors in the same pipeline phase.
		/// </summary>
		Stop,
	}
}
