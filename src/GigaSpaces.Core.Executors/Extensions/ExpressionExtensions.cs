using System;
using System.Linq.Expressions;
using System.Reflection;

namespace GigaSpaces.Core.Executors.Extensions
{
    internal static class ExpressionExtensions
    {
        /// <summary>
        /// Parses <see cref="PropertyInfo"/> from the provided expression.
        /// </summary>
        /// <typeparam name="T">The containing type.</typeparam>
        /// <typeparam name="T1">The property return type.</typeparam>
        /// <param name="propertyExpression">The property expression to parse.</param>
        /// <returns>A <see cref="PropertyInfo"/> representing the property expressed.</returns>
        public static PropertyInfo ParsePropertyInfo<T, T1>(this Expression<Func<T, T1>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;

            if (memberExpression != null && memberExpression.Member is PropertyInfo)
            {
                var propertyInfo = memberExpression.Member as PropertyInfo;
                return propertyInfo;
            }
            throw new InvalidOperationException(
                "The call to the constructor should have provided a valid MemberExpression for the target property.");
        }
    }
}