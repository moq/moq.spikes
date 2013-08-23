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
    using System.Reflection;

    public class TestInvocation : IInvocation
    {
        private Func<object> invokeBase;

        public TestInvocation(Func<object> invokeBase)
        {
            this.invokeBase = invokeBase;
        }

        public IMock Mock { get; set; }
        public object Target { get; set; }
        public MethodBase Method { get; set; }
        public object[] Arguments { get; set; }
        public object ReturnValue { get; set; }

        public void InvokeBase()
        {
            this.ReturnValue = this.invokeBase();
        }
    }
}