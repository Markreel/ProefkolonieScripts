using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BTNodeStatus { Failed, Running, Success }

public abstract class BTNode
{
    protected BTBlackboard blackboard;
    public abstract BTNodeStatus Tick();
}