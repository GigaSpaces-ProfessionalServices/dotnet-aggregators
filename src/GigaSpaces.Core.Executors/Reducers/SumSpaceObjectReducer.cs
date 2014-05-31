using System;
using System.Linq;
using GigaSpaces.Core.Executors.Tasks;

namespace GigaSpaces.Core.Executors.Reducers
{    
    /// <summary>
    /// Performs a sum reduce operation in the space on the specified space class.
    /// </summary>
    /// <typeparam name="T">The space class to sum.</typeparam>
    [Serializable]
    public class SumSpaceObjectReducer<T> : SumSpaceObjectTask<T>, IDistributedSpaceTask<long,long>
    {
        /// <inheritdoc />
        public long Reduce(SpaceTaskResultsCollection<long> results)
        {
            return results.Sum(spaceTaskResult => spaceTaskResult.Result);
        }
    }
}
