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

namespace Moq
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    public static class SetupExtensions
    {
        public static ISetup Setup<T>(this T mock, Func<T, Action> action)
        {
            return new SetupImpl();
        }

        public static ISetup Setup<T, TResult>(this T mock, Func<T, TResult> action)
        {
            return new SetupImpl();
        }

        public static void Throws<T>(this object mock)
        {
        }

        private class SetupImpl : ISetup
        {
            public IExtensible Throws<T>()
            {
                return new ExtensibleImpl();
            }
        } 

        private class ExtensibleImpl : IExtensible
        {
        }
    }

    public interface IExtensible { }

    public interface ISetup
    {
        IExtensible Throws<T>();
    }
}