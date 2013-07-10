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
using Xunit;

namespace Moq.UnitTests
{
	public class NonLambdaSpec 
	{
		[Fact]
		public void WhenSettingReturnValue_ThenSucceeds()
		{
			var calculator = Mock.Of<ICalculator>();

			calculator.Mode.Returns("DEC");
            
			var mode = calculator.Mode;

			Assert.Equal("DEC", mode);
		}

		[Fact]
		public void WhenUsingArgumentMatcher_ThenSucceeds()
		{
			var calculator = Mock.Of<ICalculator>();

			calculator.Add(Any<int>.Value, 5).Returns(5);

			var result = calculator.Add(10, 5);

			Assert.Equal(5, result);
		}

        // WhenSettingUpMock_CanSignalNoCallsToBase: maybe a using (calculator.Setup()) { // setups next }
        [Fact]
        public void when_action_then_assert()
        {
            var mock = Mock.Of<ICalculator>();
            mock.Add(-1, -1).Throws<ArgumentException>();

            var s = mock.Setup(x => x.PowerUp)
                .Throws<ArgumentException>();
        }

        [Fact]
        public void when_scope_active_then_does_not_call_base()
        {
            var mock = Mock.Of<BaseMock>();

            Assert.Throws<ArgumentException>(() => mock.DoThrows(5));

            using (new Moq.Sdk.SetupScope())
            {
                mock.DoThrows(5).Returns(10);
            }

            Assert.Equal(10, mock.DoThrows(5));

            Assert.Throws<ArgumentException>(() => mock.DoThrows(10));
        }
        
        public class BaseMock
        {
            public virtual int DoThrows(int value)
            {
                throw new ArgumentException();
            }
        }
	}
}
