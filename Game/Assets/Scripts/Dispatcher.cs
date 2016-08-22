using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Dispatcher : MonoBehaviour
{
    private int locked;
    private bool running;
    private Queue<Action> waitingActions;

    public static Dispatcher Current { get; private set; }

    public void BeginInvoke(Action action)
    {
        while (true)
        {
            if (0 == Interlocked.Exchange(ref locked, 1))
            {
                waitingActions.Enqueue(action);
                running = true;
                Interlocked.Exchange(ref locked, 0);
                break;
            }
        }
    }

    private void Awake()
    {
        if (Current != null)
        {
            Destroy(Current);
        }

        Current = this;
        waitingActions = new Queue<Action>();
    }

    private void Update()
    {
        if (running)
        {
            Queue<Action> execute = null;
            if (0 == Interlocked.Exchange(ref locked, 1))
            {
                execute = new Queue<Action>(waitingActions.Count);
                while (waitingActions.Count != 0)
                {
                    Action action = waitingActions.Dequeue();
                    execute.Enqueue(action);
                }
                running = false;
                Interlocked.Exchange(ref locked, 0);
            }

            if (execute != null)
            {
                while (execute.Count != 0)
                {
                    Action action = execute.Dequeue();
                    action();
                }
            }
        }
    }
}