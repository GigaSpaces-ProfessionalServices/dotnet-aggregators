using System;
using System.Linq.Expressions;
using GigaSpaces.Core.Executors.Tasks;

namespace GigaSpaces.Core.Executors.Reducers
{
    [Serializable]
    public class AverageReducer<T, T1> : AverageTask<T, T1>, IDistributedSpaceTask<long, long>
    {
        public AverageReducer(Expression<Func<T, T1>> averagePropertyExpression) : base(averagePropertyExpression)
        {
        }

        public new long Execute(ISpaceProxy spaceProxy, ITransaction tx)
        {
            return base.Execute(spaceProxy, tx);
        }

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
