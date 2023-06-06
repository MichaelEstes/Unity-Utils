using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IQueuedTask
{
    void OnAdded(ActionQueue context);
    void OnStart(ActionQueue context, System.Action onFinish);
    void QueueFinish(ActionQueue context);
    bool CanStart(ActionQueue context);
}
