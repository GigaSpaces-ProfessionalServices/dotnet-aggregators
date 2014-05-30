using System;
using System.Linq.Expressions;
using GigaSpaces.Core.Executors.Tasks;

namespace GigaSpaces.Core.Executors.Reducers
{
    /// <summary>
    /// Performs a sum reduce operation in the space on the specified property.
    /// </summary>
    /// <typeparam name="T">The space class containing the target property.</typeparam>
    /// <typeparam name="T1">The return type of the target property.</typeparam>
    [Serializable]
    public class SumReducer<T, T1> : SumTask<T, T1>, IDistributedSpaceTask<long, long>
    {
        public SumReducer(Expression<Func<T, T1>> sumExpression) : base(sumExpression)
        {
        }

        /// <inheritdoc />
        public long Reduce(SpaceTaskResultsCollection<long> results)
        {
            long output = 0;

            foreach (var spaceTaskResult in results)
            {
                output += spaceTaskResult.Result;
            }

            return output;
        }
    }
}