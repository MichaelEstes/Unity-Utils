using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIAction
{
    public abstract string GetKey();
    public abstract Dictionary<string, AIStateCondition> GetConditionMap();
    public abstract void OnEnter(MonoBehaviour aiBehaviour);
    public abstract void OnExit(MonoBehaviour aiBehaviour, string newState);
    public abstract void OnUpdate(MonoBehaviour aiBehaviour);
    public abstract void OnEvent(enum event, AIPlanner planner, MonoBehaviour aiBehaviour);
}
