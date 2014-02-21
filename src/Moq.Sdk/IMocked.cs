using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moq.Sdk
{
	/// <summary>
	/// Interface that must be implemented by proxy objects and which 
	/// provides access to the mock corresponding to an object instance.
	/// </summary>
	public interface IMocked
	{
		/// <summary>
		/// Gets the mock corresponding to the implementing object instance.
		/// </summary>
		IMock Mock { get; }
	}
}
