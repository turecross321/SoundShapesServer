using Bunkum.Core;
using Bunkum.Core.Storage;
using Bunkum.EntityFrameworkDatabase;
using NotEnoughLogs;
using SoundShapesServer.Database;
using SoundShapesServer.Types;

namespace SoundShapesServer.Workers;

public class WorkerManager(Logger logger, IDataStore dataStore, EntityFrameworkDatabaseProvider<GameDatabaseContext> databaseProvider)
{
    private readonly IDataStore _dataStore = dataStore;

    private Thread? _thread = null;
    private bool _threadShouldRun = false;

    private readonly List<IWorker> _workers = [];
    private readonly Dictionary<IWorker, long> _lastWorkTimestamps = new();

    public void AddWorker<TWorker>() where TWorker : IWorker, new()
    {
        TWorker worker = new();
        this._workers.Add(worker);
    }
    public void AddWorker(IWorker worker)
    {
        this._workers.Add(worker);
    }

    private void RunWorkCycle()
    {
        Lazy<DataContext> dataContext = new(() => new DataContext
        {
            Database = databaseProvider.GetContext(),
            Logger = logger,
        });
        
        foreach (IWorker worker in this._workers)
        {
            long now = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            if (this._lastWorkTimestamps.TryGetValue(worker, out long lastWork))
            {
                if(now - lastWork < worker.WorkIntervalMilliseconds) continue;
                
                this._lastWorkTimestamps[worker] = now;
            }
            else
            {
                this._lastWorkTimestamps.Add(worker, now);
            }
            
            logger.LogTrace(SSSContext.Worker, "Running work cycle for " + worker.GetType().Name);
            worker.DoWork(dataContext.Value);
        }
    }

    public void Start()
    {
        logger.LogDebug(SSSContext.Startup, "Starting the worker thread");
        this._threadShouldRun = true;
        Thread thread = new(() =>
        {
            while (this._threadShouldRun)
            {
                try
                {
                    this.RunWorkCycle();
                    Thread.Sleep(100);
                }
                catch(Exception e)
                {
                    logger.LogCritical(SSSContext.Worker, "Critical exception while running work cycle: " + e);
                    logger.LogCritical(SSSContext.Startup, "Waiting for 1 second before trying to run another cycle.");
                    Thread.Sleep(1000);
                }
            }
        });
        
        thread.Start();

        this._thread = thread;
    }
    
    public void Stop()
    {
        if (this._thread == null) return;
        logger.LogDebug(SSSContext.Worker, "Stopping the worker thread");
        
        this._threadShouldRun = false;
        while (this._thread.IsAlive)
        {
            Thread.Sleep(10);
        }
    }
}