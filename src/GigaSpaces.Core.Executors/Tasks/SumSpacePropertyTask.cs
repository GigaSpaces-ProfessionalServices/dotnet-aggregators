using System;
using System.Linq.Expressions;
using System.Reflection;
using GigaSpaces.Core.Executors.Extensions;

namespace GigaSpaces.Core.Executors.Tasks
{
    /// <summary>
    /// Performs a sum operation in the space on the specified property.
    /// </summary>
    /// <typeparam name="T">The space class containing the target property.</typeparam>
    /// <typeparam name="T1">The return type of the target property.</typeparam>
    [Serializable]
    public class SumSpacePropertyTask<T, T1> : ISpaceTask<long>
    {
        private PropertyInfo _targetProperty;

        public SumSpacePropertyTask(Expression<Func<T, T1>> sumExpression)
        {
            if (sumExpression == null)
                throw new ArgumentNullException("sumExpression");

            _targetProperty = sumExpression.ParsePropertyInfo();
        }

        /// <inheritdoc />
        public long Execute(ISpaceProxy spaceProxy, ITransaction tx)
        {
            long output = 0;

            var records =
                spaceProxy.ReadMultiple<T>(new SqlQuery<T>(string.Empty) { Projections = new[] { _targetProperty.Name } });

            foreach (T record in records)
            {
                output += Convert.ToInt64(_targetProperty.GetValue(record));
            }

            return output;
        }
    }
}