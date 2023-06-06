using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIStateCondition
{
    public abstract bool IsMet(MonoBehaviour aiBehaviour);
}
