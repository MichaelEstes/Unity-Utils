using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ActionQueue
{
    private Queue<IQueuedTask> tasks;
    private MonoBehaviour context;
    private bool loop;
    private System.Action onQueueFinished;

    public ActionQueue(MonoBehaviour context, bool loop = false, System.Action onQueueFinished = null)
    {
        this.context = context;
        this.loop = loop;
        this.onQueueFinished = onQueueFinished;
        tasks = new Queue<IQueuedTask>();
    }

    public void AddTask(IQueuedTask task)
    {
        this.tasks.Enqueue(task);
    }

    public void Start()
    {
       if(tasks.Count == 0)
       {
            Debug.LogWarning("No Tasks To Run");
       }

        RunNext();
    }

    void RunNext()
    {
        if (tasks.Count == 0)
        {
            QueueFinished();
            return;
        }

        IQueuedTask next = tasks.Dequeue();

        if (loop)
        {
            AddTask(next);
        }

        if (next.CanStart(this))
        {
            next.OnStart(this, TaskFinished);
        }
    }

    void QueueFinished()
    {
        if (onQueueFinished != null) onQueueFinished();
    }

    void TaskFinished()
    {
        RunNext();
    }

    public void Destroy()
    {
        this.tasks.Clear();
    }
}
