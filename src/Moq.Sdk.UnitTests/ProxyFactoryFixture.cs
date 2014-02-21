using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Moq.Sdk.UnitTests
{
	public abstract class ProxyFactoryFixture : IDisposable
	{
		[Fact]
		public void when_mock_object_created_then_implements_IMocked()
		{
			var mock = CreateMock(typeof(ICalculator));
			var obj = mock.Object;
			var mocked = obj as IMocked;

			Assert.NotNull(mocked);
		}

		[Fact]
		public void when_mock_object_created_then_can_retrieve_mock()
		{
			var mock = CreateMock(typeof(ICalculator));
			var obj = mock.Object;
			var mocked = obj as IMocked;

			Assert.Same(mock, mocked.Mock);
		}

		[Fact]
		public void when_invoking_mock_then_invokes_behavior()
		{
			var mock = CreateMock(typeof(ICalculator));
			mock.Behaviors.Add(new CompositeBehavior(i => true)
			{
				Invoke =
                    {
                        new DelegateBehavior(i =>
                        {
                            i.ReturnValue = 15;
                        })
                    }
			});

			var calculator = (ICalculator)mock.Object;

			Assert.NotNull(calculator);
			Assert.Equal(15, calculator.Add(0, 0));
			Assert.Equal(1, mock.Invocations.Count);
			Assert.Equal(1, mock.Behaviors[0].Invocations.Count);
		}

		protected abstract IMock CreateMock(Type type);

		#region IDisposable

		private bool disposed;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			disposed = true;
		}

		~ProxyFactoryFixture()
		{
			Dispose(false);
		}

		#endregion
	}
}