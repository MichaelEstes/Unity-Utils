using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MoveToLoop : MonoBehaviour
{
    public Transform toMove;

    public Transform start;
    public Transform end;

    public float speed;
    public float smoothTime;
    public float pauseTime;

    private Vector3 velocity;

    private ActionQueue moveActions;

    private Transform currentTarget;
    private Action onReachedTarget;

    private WaitForSeconds pauseDelay;
    private Coroutine pauseWait;

    void Awake()
    {
        pauseDelay = new WaitForSeconds(pauseTime);
    }

    private void OnEnable()
    {

        transform.position = start.position;

        moveActions = new ActionQueue(this, true);
        moveActions.AddTask(new QueuedTask(MoveStart));
        moveActions.AddTask(new QueuedTask(MovePause));
        moveActions.Start();
    }

    private void OnDisable()
    {
        if(pauseWait != null)
        {
            StopCoroutine(pauseWait);
            pauseWait = null;
        }
        moveActions?.Destroy();
    }

    private void Update()
    {
        if(onReachedTarget != null)
        {
            Vector3 targetPos = currentTarget.position;
            toMove.position = Vector3.SmoothDamp(toMove.position, targetPos, ref velocity, smoothTime, speed);

            if(Mathf.Abs((toMove.position - targetPos).sqrMagnitude) < 0.5f)
            {
                onReachedTarget();
                onReachedTarget = null;
            }
        }
    }

    private void MoveStart(ActionQueue context, Action onFinish)
    {
        onReachedTarget = onFinish;
        currentTarget = currentTarget == start ? end : start;
    }

    private void MovePause(ActionQueue context, Action onFinish)
    {
        pauseWait = StartCoroutine(PauseDelay(onFinish));
    }

    IEnumerator PauseDelay(Action onFinish)
    {
        yield return pauseDelay;
        pauseWait = null;
        onFinish?.Invoke();
    }
}
