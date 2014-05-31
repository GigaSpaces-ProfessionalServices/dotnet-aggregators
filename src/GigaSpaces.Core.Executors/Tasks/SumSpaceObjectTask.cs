using System;

namespace GigaSpaces.Core.Executors.Tasks
{
    /// <summary>
    /// Performs a sum operation on the instances of the space class.
    /// </summary>
    /// <typeparam name="T">The space class to count.</typeparam>
    [Serializable]
    public class SumSpaceObjectTask<T> : ISpaceTask<long>
    {
        /// <inheritdoc />
        public long Execute(ISpaceProxy spaceProxy, ITransaction tx)
        {
            T[] readMultiple = spaceProxy.ReadMultiple<T>(new SqlQuery<T>(string.Empty));
            return readMultiple.Length;
        }
    }
}