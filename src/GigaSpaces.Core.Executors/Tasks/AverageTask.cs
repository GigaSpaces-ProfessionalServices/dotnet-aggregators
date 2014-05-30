using System;
using System.Linq.Expressions;
using System.Reflection;
using GigaSpaces.Core.Executors.Extensions;

namespace GigaSpaces.Core.Executors.Tasks
{
    /// <summary>
    /// Performs an average operation against the space proxy.
    /// </summary>
    /// <typeparam name="T">The target space class.</typeparam>
    /// <typeparam name="T1">The return type of the target property.</typeparam>
    [Serializable]
    public class AverageTask<T,T1> : ISpaceTask<long>
    {
        private readonly PropertyInfo _targetProperty;

        public AverageTask(Expression<Func<T, T1>> averagePropertyExpression)
        {
            if (averagePropertyExpression == null)
                throw new ArgumentNullException("averagePropertyExpression");

            _targetProperty = averagePropertyExpression.ParsePropertyInfo();
        }

        /// <inheritdoc />
        public long Execute(ISpaceProxy spaceProxy, ITransaction tx)
        {
            var records = spaceProxy.ReadMultiple<T>(new SqlQuery<T>(string.Empty) {Projections = new[] {_targetProperty.Name}});

            long sum = 0;
            var recordCount = records.Length;

            foreach (var record in records)
            {
               sum += Convert.ToInt64(_targetProperty.GetValue(record));
            }

            return (sum/recordCount);
        }
    }
}