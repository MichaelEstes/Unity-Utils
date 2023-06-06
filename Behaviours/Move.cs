using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Axis moveOn;
    public float speed;

    private Vector3 dir;

    void Start()
    {
        switch (moveOn)
        {
            case Axis.Up:
                dir = new Vector3(0, speed, 0);
                break;
            case Axis.Forward:
                dir = new Vector3(0, 0, speed);
                break;
            case Axis.Right:
                dir = new Vector3(speed, 0, 0);
                break;
            default:
                dir = Vector3.zero;
                break;
        }
    }

    void Update()
    {
        float dt = Time.deltaTime;

        Vector3 pos = transform.position;
        pos += dt * dir;
        transform.position = pos;
    }
}
