using SoundShapesServer.Types;

namespace SoundShapesServer.Workers;

public interface IWorker
{
    /// <summary>
    /// How often to perform work, in milliseconds
    /// </summary>
    public int WorkIntervalMilliseconds { get; }

    /// <summary>
    /// Instructs the worker to do work.
    /// </summary>
    public void DoWork(DataContext context);
}