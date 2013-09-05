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
    using System.Collections.Generic;
    using System.Linq;
    using Xunit;

    public class MockBaseFixture
    {
        [Fact]
        public void when_invocation_performed_then_records_invocation()
        {
            var invocation = new TestInvocation(() => null);
            var mock = new TestMock();

            mock.Invoke(invocation);

            Assert.Equal(1, mock.Invocations.Count);
            Assert.Same(invocation, mock.Invocations[0]);
        }

        [Fact]
        public void when_behavior_matches_invocation_then_it_is_called()
        {
            var mock = new TestMock();
            IInvocation invocation = null;
 
            var behavior = new Behavior(i => true, i => invocation = i);

            mock.Behaviors.Add(behavior);

            mock.Invoke(new TestInvocation(() => null));

            Assert.NotNull(invocation);
        }

        [Fact]
        public void when_behavior_is_executed_then_behavior_tracks_invocation()
        {
            var mock = new TestMock();
            var invocation = new TestInvocation(() => null);
            var behavior = new Behavior(i => true, i => { });
            mock.Behaviors.Add(behavior);

            mock.Invoke(invocation);

            Assert.Equal(1, behavior.Invocations.Count);
            Assert.Same(invocation, behavior.Invocations[0]);
        }

        [Fact]
        public void when_behavior_throws_then_invocation_is_still_recorded()
        {
            var mock = new TestMock();
            var invocation = new TestInvocation(() => null);
            var behavior = new Behavior(i => true, i => { throw new ArgumentException(); });
            mock.Behaviors.Add(behavior);

            Assert.Throws<ArgumentException>(() => mock.Invoke(invocation));

            Assert.Equal(1, behavior.Invocations.Count);
            Assert.Same(invocation, behavior.Invocations[0]);
        }

        [Fact]
        public void when_aspects_configured_then_invokes_in_order()
        {
            var mock = new TestMock();
            var invocation = new TestInvocation(() => null);
            var order = new List<string>();

            var behavior = new Behavior(i => true, i => { });
            behavior.Before.Add(new DelegateAspect(i => true, i => { order.Add("before"); return BehaviorAction.Continue; }));
            behavior.Invoke.Add(new DelegateAspect(i => true, i => { order.Add("invoke"); return BehaviorAction.Continue;  }));
            behavior.After.Add(new DelegateAspect(i => true, i => { order.Add("after"); return BehaviorAction.Continue; }));

            mock.Behaviors.Add(behavior);

            mock.Invoke(invocation);

            Assert.Equal(3, order.Count);
            Assert.Equal("before", order[0]);
            Assert.Equal("invoke", order[1]);
            Assert.Equal("after", order[2]);
        }

        [Fact]
        public void when_aspect_is_disabled_then_does_not_invoke_on_execute()
        {
            var mock = new TestMock();
            var invocation = new TestInvocation(() => null);
            var order = new List<string>();

            var behavior = new Behavior(i => true, i => { });
            behavior.Before.Add(new DelegateAspect(i => true, i => { order.Add("before"); return BehaviorAction.Continue; }));
            behavior.Invoke.Add(new DelegateAspect(i => false, i => { order.Add("invoke-not"); return BehaviorAction.Continue; }));
            behavior.Invoke.Add(new DelegateAspect(i => true, i => { order.Add("invoke"); return BehaviorAction.Continue; }));
            behavior.After.Add(new DelegateAspect(i => true, i => { order.Add("after"); return BehaviorAction.Continue; }));

            mock.Behaviors.Add(behavior);

            mock.Invoke(invocation);

            Assert.Equal(3, order.Count);
            Assert.Equal("before", order[0]);
            Assert.Equal("invoke", order[1]);
            Assert.Equal("after", order[2]);
        }

        [Fact]
        public void when_aspect_stops_further_aspects_on_phase_then_does_not_invoke_next_aspect()
        {
            var mock = new TestMock();
            var invocation = new TestInvocation(() => null);
            var order = new List<string>();

            var behavior = new Behavior(i => true, i => { });
            behavior.Before.Add(new DelegateAspect(i => true, i => { order.Add("before"); return BehaviorAction.Continue; }));
            behavior.Invoke.Add(new DelegateAspect(i => true, i => { order.Add("invoke"); return BehaviorAction.Stop; }));
            behavior.Invoke.Add(new DelegateAspect(i => true, i => { order.Add("invoke-not"); return BehaviorAction.Continue; }));
            behavior.After.Add(new DelegateAspect(i => true, i => { order.Add("after"); return BehaviorAction.Continue; }));
            mock.Behaviors.Add(behavior);

            mock.Invoke(invocation);

            Assert.Equal(3, order.Count);
            Assert.Equal("before", order[0]);
            Assert.Equal("invoke", order[1]);
            Assert.Equal("after", order[2]);
        }

        //mock.Setup(x => x.Add(It.IsAny<int>(), It.IsAny<int>()))
        //    .Callback(() => )
        //    .Returns(15)
        //    .Returns(10)
        //    .Returns(11)
        //    .Loop()
        //    .Callback(() => );
    }
}