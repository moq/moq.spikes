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

namespace Moq.Sdk.UnitTests
{
    using System;
    using System.Linq;
    using Xunit;

    public class DynamicProxyFixture
    {
        [Fact]
        public void when_invoking_dynamic_proxy_then_invokes_mock_behavior()
        {
            var mock = new Mock<ICalculator>();
            mock.Behaviors.Add(new Behavior(
                i => true,
                i => { })
                {
                    Invoke =
                    {
                        new DelegateAspect(i => true, i =>
                        {
                            i.ReturnValue = 15;
                            return BehaviorAction.Continue;
                        })
                    }
                });

            var calculator = mock.Object;

            Assert.NotNull(calculator);
            Assert.Equal(15, calculator.Add(0, 0));
            Assert.Equal(1, mock.Invocations.Count);
            Assert.Equal(1, mock.Behaviors[0].Invocations.Count);
        }
    }
}