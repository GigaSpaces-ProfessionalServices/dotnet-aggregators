using System;
using System.Linq.Expressions;
using GigaSpaces.Core.Executors.Tasks;

namespace GigaSpaces.Core.Executors.Reducers
{
    [Serializable]
    public class MinimumReducer<T, T1> : MinimumTask<T, T1>, IDistributedSpaceTask<long, long>
    {
        public MinimumReducer(Expression<Func<T, T1>> minimumExpression) : base(minimumExpression)
        {
        }

        public long Reduce(SpaceTaskResultsCollection<long> results)
        {
            bool isSet = false;
            long output = 0;

            foreach (var spaceTaskResult in results)
            {
                if (!isSet || output > spaceTaskResult.Result)
                {
                    output = spaceTaskResult.Result;
                    isSet = true;
                }
            }

            return output;
        }
    }
}