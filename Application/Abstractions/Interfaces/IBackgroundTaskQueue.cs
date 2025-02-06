namespace Application.Abstractions.Interfaces
{
    /// <summary>
    /// Interface for the background task queue.
    /// Allows adding any task (method) for subsequent execution.
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// Adds a background task to the queue.
        /// </summary>
        /// <param name="task">A task delegate that accepts a CancellationToken and returns a Task.</param>
        void EnqueueTask(Func<CancellationToken, Task> task);

        /// <summary>
        /// Asynchronously dequeues a task from the queue.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token.</param>
        /// <returns>A task delegate..</returns>
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
    
}
