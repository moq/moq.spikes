using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Moq.Sdk.UnitTests
{
	public class PipelineFixture
	{
	}

	public class Argument
	{
		public Argument(int index, ParameterInfo info, object value)
		{
			Index = index;
			Info = info;
			Name = info.Name;
			Value = value;
		}

		public int Index { get; private set; }
		public ParameterInfo Info { get; private set; }
		public string Name { get; private set; }
		public object Value { get; private set; }
	}

	public class ArgumentList : IEnumerable<Argument>
	{
		Lazy<Argument[]> arguments;
		Lazy<Dictionary<string, Argument>> infoByName;
		Lazy<Dictionary<int, Argument>> infoByIndex;

		public ArgumentList(object[] argumentValues, ParameterInfo[] parameterInfos)
		{
			this.arguments = new Lazy<Argument[]>(() =>
				argumentValues.Select((value, index) => new Argument(index, parameterInfos[index], value)).ToArray());

			this.infoByName = new Lazy<Dictionary<string, Argument>>(() =>
				arguments.Value.ToDictionary(info => info.Name));

			this.infoByIndex = new Lazy<Dictionary<int, Argument>>(() =>
				arguments.Value.ToDictionary(info => info.Index));
		}

		public object this[int index]
		{
			get { return arguments.Value[index]; }
		}

		public object this[string name]
		{
			get { return arguments.Value[infoByName.Value[name].Index]; }
		}

		public ParameterInfo GetInfo(int index)
		{
			return infoByIndex.Value[index].Info;
		}

		public ParameterInfo GetInfo(string name)
		{
			return infoByName.Value[name].Info;
		}

		IEnumerator<Argument> IEnumerable<Argument>.GetEnumerator()
		{
			return ((IEnumerable<Argument>)arguments.Value).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Argument>)this).GetEnumerator();
		}
	}
}
