using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptEnablePassthrough : MonoBehaviour
{
    public List<MonoBehaviour> behaviours;

    private void OnEnable()
    {
        foreach (MonoBehaviour behaviour in behaviours) behaviour.enabled = true;
    }
}
