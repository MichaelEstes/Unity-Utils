
using System;

public class WaitGroup
{
    private int registeredCount;
    private Action onFinished;

    public WaitGroup()
    {
        registeredCount = 0;
    }

    public void Wait(Action onFinished)
    {
        this.onFinished = onFinished;
    }

    public void Add(int amount)
    {
        registeredCount += amount;
    }

    public void Done()
    {
        registeredCount -= 1;

        if (registeredCount == 0)
        {
            onFinished();
        }
    }
}
