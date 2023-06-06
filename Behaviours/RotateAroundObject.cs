using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundObject : MonoBehaviour
{
    public Transform toRotate;
    public Transform staticObj;
    public float speed;
    public Axis axis;

    private Vector3 dir;

    private void Start()
    {
        dir = Utils.AxisToDir(axis, toRotate);
    }

    void Update()
    {
        toRotate.RotateAround(staticObj.position, dir, Time.deltaTime * speed);
        toRotate.rotation = Quaternion.FromToRotation(-Vector3.up, staticObj.position - toRotate.position);
    }
}
