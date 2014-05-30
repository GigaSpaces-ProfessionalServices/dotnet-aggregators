using System;
using System.Linq.Expressions;
using GigaSpaces.Core.Executors.Tasks;

namespace GigaSpaces.Core.Executors.Reducers
{
    /// <summary>
    /// Performs an average reduce operation against the space proxy.
    /// </summary>
    /// <typeparam name="T">The space class containing the target property.</typeparam>
    /// <typeparam name="T1">The return type of the target property.</typeparam>
    [Serializable]
    public class AverageReducer<T, T1> : AverageTask<T, T1>, IDistributedSpaceTask<long, long>
    {
        public AverageReducer(Expression<Func<T, T1>> averagePropertyExpression) : base(averagePropertyExpression)
        {
        }

        /// <inheritdoc />
        public long Reduce(SpaceTaskResultsCollection<long> results)
        {
            long sum = 0;

            foreach (var spaceTaskResult in results)
            {
                sum += spaceTaskResult.Result;
            }

            return sum/results.Count;
        }
    }
}
