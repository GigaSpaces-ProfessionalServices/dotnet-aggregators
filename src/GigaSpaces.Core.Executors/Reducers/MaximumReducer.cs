using System;
using System.Linq.Expressions;
using GigaSpaces.Core.Executors.Tasks;

namespace GigaSpaces.Core.Executors.Reducers
{
    [Serializable]
    public class MaximumReducer<T, T1> : MaximumTask<T, T1>, IDistributedSpaceTask<long,long>
    {
        public MaximumReducer(Expression<Func<T, T1>> maximumPropertyExpression) : base(maximumPropertyExpression)
        {
        }

        public long Reduce(SpaceTaskResultsCollection<long> results)
        {
            bool isSet = false;
            long output = 0;

            foreach (var spaceTaskResult in results)
            {
                if (!isSet || output < spaceTaskResult.Result)
                {
                    output = spaceTaskResult.Result;
                    isSet = true;
                }
            }

            return output;
        }
    }
}