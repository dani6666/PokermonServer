using System.Collections.Generic;

namespace Pokermon.Repository
{
    public class Synchronizer
    {
        private readonly Dictionary<int, object> _locks = new();
        private readonly object _synchronizerLock = new();

        public object this[int index]
        {
            get
            {
                lock (_synchronizerLock)
                {
                    if (_locks.TryGetValue(index, out var result))
                        return result;

                    result = new object();
                    _locks[index] = result;
                    return result;
                }
            }
        }

        public void Remove(int index)
        {
            lock (_synchronizerLock)
            {
                _locks.Remove(index);
            }
        }
    }
}
