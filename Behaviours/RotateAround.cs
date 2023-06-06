using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RotateAround : MonoBehaviour
{
    public float speed;
    public Axis axis;

    private Vector3 dir;

    void Update()
    {
        dir = Utils.AxisToDir(axis, transform);
        transform.RotateAround(transform.position, dir, Time.deltaTime * speed);
    }
}
