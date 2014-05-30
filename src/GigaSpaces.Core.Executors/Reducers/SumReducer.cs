using System;
using System.Linq.Expressions;
using GigaSpaces.Core.Executors.Tasks;

namespace GigaSpaces.Core.Executors.Reducers
{
    [Serializable]
    public class SumReducer<T, T1> : SumTask<T, T1>, IDistributedSpaceTask<long, long>
    {
        public SumReducer(Expression<Func<T, T1>> sumExpression) : base(sumExpression)
        {
        }

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