using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nito.AsyncEx;
using System.Threading;
using NLog;

namespace PlayFoos.Engine.Workers
{
    public abstract class Worker : IDisposable
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        //protected readonly int _lockTimeoutMs;
        private readonly CancellationTokenSource _lockImmediately;
        protected readonly AsyncLock _lock = new AsyncLock();

        public Worker()
        {
            _lockImmediately = new CancellationTokenSource();
            _lockImmediately.Cancel();
        }

        public async Task<bool> UpdateAsync()
        {
            try
            {
                using (await _lock.LockAsync(_lockImmediately.Token))
                {
                    return await UpdateInternalAsync();
                }
            }
            catch (OperationCanceledException)
            {
                // skip work if already locked
                return true;
            }
            catch (Exception e)
            {
                logger.ErrorException("Error in Worker", e);
                return false;
            }
        }

        public bool IsWorking()
        {
            try
            {
                using (_lock.Lock(_lockImmediately.Token))
                {
                    return false;
                }
            }
            catch (OperationCanceledException)
            {
                return true;
            }
        }

        protected abstract Task<bool> UpdateInternalAsync();

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _lock.Lock();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
        #endregion
    }
}
