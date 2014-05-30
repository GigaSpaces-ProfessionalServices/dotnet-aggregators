using System;
using System.Linq.Expressions;
using System.Reflection;
using GigaSpaces.Core.Executors.Extensions;

namespace GigaSpaces.Core.Executors.Tasks
{
    [Serializable]
    public class SumTask<T, T1> : ISpaceTask<long>
    {
        private PropertyInfo _targetProperty;

        public SumTask(Expression<Func<T, T1>> sumExpression)
        {
            if (sumExpression == null)
                throw new ArgumentNullException("sumExpression");

            _targetProperty = sumExpression.ParsePropertyInfo();
        } 

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