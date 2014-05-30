using System;
using System.Linq.Expressions;
using GigaSpaces.Core.Executors.Tasks;

namespace GigaSpaces.Core.Executors.Reducers
{
    /// <summary>
    /// Performs a maximum reduce operation in the space on the specified property.
    /// </summary>
    /// <typeparam name="T">The space class containing the target property.</typeparam>
    /// <typeparam name="T1">The return type of the target property.</typeparam>
    [Serializable]
    public class MaximumReducer<T, T1> : MaximumTask<T, T1>, IDistributedSpaceTask<long,long>
    {
        public MaximumReducer(Expression<Func<T, T1>> maximumPropertyExpression) : base(maximumPropertyExpression)
        {
        }


        /// <inheritdoc />
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