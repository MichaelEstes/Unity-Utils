using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIPlanner : MonoBehaviour
{
    protected Dictionary<string, AIAction> actionMap = new Dictionary<string, AIAction>();

    MonoBehaviour aiBehaviour;

    public string currentActionKey;

    public AIAction currentAction;

    Dictionary<string, AIStateCondition> conditionMap;

    void Awake()
    {
        BuildActionMap();
    }

    protected abstract AIAction[] GetActions();

    protected void BuildActionMap()
    {
        AIAction[] actions = GetActions();
        foreach (AIAction action in actions)
        {
            actionMap.Add(action.GetKey(), action);
        }
    }

    public virtual void Init(MonoBehaviour aiBehaviour)
    {
        this.aiBehaviour = aiBehaviour;
    }

    public void OnEvent(enum event)
    {
        currentAction.OnEvent(event, this, aiBehaviour);
    }

    public string ConditionMet()
    {
        foreach (KeyValuePair<string, AIStateCondition> condition in conditionMap)
        {
            if (condition.Value.IsMet(aiBehaviour))
            {
                return condition.Key;
            }
        }

        return "";
    }

    public void SetNewAction(string newState)
    {
        if (!actionMap.ContainsKey(newState))
        {
            Debug.LogError("SetNewAction: Can't Find Action For Key: " + newState);
            return;
        }

        if (!(currentAction is null))
        {
            currentAction.OnExit(aiBehaviour, newState);
        }

        currentAction = actionMap[newState];
        currentActionKey = currentAction.GetKey();
        currentAction.OnEnter(aiBehaviour);
        conditionMap = currentAction.GetConditionMap();
    }

    public virtual void OnDeath()
    {
        Destroy(this);
    }

    void OnDestroy()
    {
        if (!(currentAction is null))
        {
            currentAction.OnExit(aiBehaviour, "");
        }
        StopAllCoroutines();
    }
}
