using System;
using System.Linq.Expressions;
using GigaSpaces.Core.Executors.Tasks;

namespace GigaSpaces.Core.Executors.Reducers
{
    /// <summary>
    /// Performs a minimum reduce operation in the space on the specified property.
    /// </summary>    
    /// <typeparam name="T">The space class containing the target property.</typeparam>
    /// <typeparam name="T1">The return type of the target property.</typeparam>
    [Serializable]
    public class MinimumReducer<T, T1> : MinimumTask<T, T1>, IDistributedSpaceTask<long, long>
    {
        public MinimumReducer(Expression<Func<T, T1>> minimumExpression) : base(minimumExpression)
        {
        }

        /// <inheritdoc />
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