using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueuedTask : IQueuedTask
{
    private Action<ActionQueue, Action> onStart;
    private Action<ActionQueue> onAdded;
    private Predicate<ActionQueue> canStart;
    private Action<ActionQueue> queueFinished;


    public QueuedTask(Action<ActionQueue, Action> onStart, Action<ActionQueue> onAdded = null, Predicate<ActionQueue> canStart = null, Action<ActionQueue> queueFinished = null)
    {
        this.onStart = onStart;
        this.onAdded = onAdded;
        this.canStart = canStart;
        this.queueFinished = queueFinished;
    }

    public bool CanStart(ActionQueue context)
    {
        if(canStart != null)
        {
            return canStart(context);
        }

        return true;
    }

    public void OnAdded(ActionQueue context)
    {
        onAdded?.Invoke(context);
    }

    public void OnStart(ActionQueue context, Action onFinish)
    {
        onStart(context, onFinish);
    }

    public void QueueFinish(ActionQueue context)
    {
        queueFinished?.Invoke(context);
    }
}
