using System;
using System.Linq.Expressions;
using System.Reflection;
using GigaSpaces.Core.Executors.Extensions;

namespace GigaSpaces.Core.Executors.Tasks
{
    [Serializable]
    public class MinimumTask<T, T1> : ISpaceTask<long>
    {
        private PropertyInfo _targetProperty;

        public MinimumTask(Expression<Func<T, T1>> minimumExpression)
        {
            if(minimumExpression == null)
                throw new ArgumentNullException("minimumExpression");

            _targetProperty = minimumExpression.ParsePropertyInfo();
        } 

        public long Execute(ISpaceProxy spaceProxy, ITransaction tx)
        {
            bool isSet = false;
            long output = 0;

            var records =
                spaceProxy.ReadMultiple<T>(new SqlQuery<T>(string.Empty) { Projections = new[] { _targetProperty.Name } });

            foreach (T record in records)
            {
                long testValue = Convert.ToInt64(_targetProperty.GetValue(record));
                if (!isSet || testValue < output)
                {
                    output = testValue;
                    isSet = true;
                }
            }

            return output;
        }
    }
}