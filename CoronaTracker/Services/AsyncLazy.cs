using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CoronaTracker.Services
{
    public class AsyncLazy<T> : Lazy<Task<T>>
    {

        private readonly Lazy<Task<T>> Instance;

        public AsyncLazy(Func<T> factory) => Instance = new Lazy<Task<T>>(() => Task.Run(factory));

        public AsyncLazy(Func<Task<T>> factory) => Instance = new Lazy<Task<T>>(() => Task.Run(factory));

        public TaskAwaiter<T> GetAwaiter() => Instance.Value.GetAwaiter();

    }
}
