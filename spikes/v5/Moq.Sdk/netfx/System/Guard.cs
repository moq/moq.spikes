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
using System.Linq.Expressions;

/// <summary>
/// Common guard class for argument validation.
/// </summary>
///	<nuget id="netfx-Guard" />
//[DebuggerStepThrough]
static class Guard
{
    /// <summary>
    /// Ensures the given <paramref name="value"/> is not null.
    /// Throws <see cref="ArgumentNullException"/> otherwise.
    /// </summary>
    /// <exception cref="System.ArgumentException">The <paramref name="value"/> is null.</exception>
    public static void NotNull<T>(Expression<Func<T>> reference, T value)
    {
        if (value == null)
            throw new ArgumentNullException(GetParameterName(reference), "Parameter cannot be null.");
    }

    /// <summary>
    /// Ensures the given string <paramref name="value"/> is not null or empty.
    /// Throws <see cref="ArgumentNullException"/> in the first case, or 
    /// <see cref="ArgumentException"/> in the latter.
    /// </summary>
    /// <exception cref="System.ArgumentException">The <paramref name="value"/> is null or an empty string.</exception>
    public static void NotNullOrEmpty(Expression<Func<string>> reference, string value)
    {
        NotNull<string>(reference, value);
        if (value.Length == 0)
            throw new ArgumentException("Parameter cannot be empty.", GetParameterName(reference));
    }

    /// <summary>
    /// Ensures the given string <paramref name="value"/> is valid according 
    /// to the <paramref name="validate"/> function. Throws <see cref="ArgumentNullException"/> 
    /// otherwise.
    /// </summary>
    /// <exception cref="System.ArgumentException">The <paramref name="value"/> is not valid according 
    /// to the <paramref name="validate"/> function.</exception>
    public static void IsValid<T>(Expression<Func<T>> reference, T value, Func<T, bool> validate, string message)
    {
        if (!validate(value))
            throw new ArgumentException(message, GetParameterName(reference));
    }

    /// <summary>
    /// Ensures the given string <paramref name="value"/> is valid according 
    /// to the <paramref name="validate"/> function. Throws <see cref="ArgumentNullException"/> 
    /// otherwise.
    /// </summary>
    /// <exception cref="System.ArgumentException">The <paramref name="value"/> is not valid according 
    /// to the <paramref name="validate"/> function.</exception>
    public static void IsValid<T>(Expression<Func<T>> reference, T value, Func<T, bool> validate, string format, params object[] args)
    {
        if (!validate(value))
            throw new ArgumentException(string.Format(format, args), GetParameterName(reference));
    }

    private static string GetParameterName(Expression reference)
    {
        var lambda = reference as LambdaExpression;
        var member = lambda.Body as MemberExpression;

        return member.Member.Name;
    }
}