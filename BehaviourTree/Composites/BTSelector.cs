using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BTSelector : BTNode
{
    private BTNode[] inputNodes;
    public BTSelector(BTBlackboard _blackboard, params BTNode[] _input)
    {
        blackboard = _blackboard;
        inputNodes = _input;
    }

    public override BTNodeStatus Tick()
    {
        foreach (BTNode _node in inputNodes)
        {
            BTNodeStatus _result = _node.Tick();
            switch (_result)
            {
                case BTNodeStatus.Failed:
                    continue;
                case BTNodeStatus.Running:
                    return BTNodeStatus.Running;
                case BTNodeStatus.Success:
                    return BTNodeStatus.Success;
            }
        }
        return BTNodeStatus.Failed;
    }
}
