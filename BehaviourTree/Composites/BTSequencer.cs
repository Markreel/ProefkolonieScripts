class BTSequencer : BTNode
{
    private BTNode[] inputNodes;
    public BTSequencer(BTBlackboard _blackboard, params BTNode[] _input)
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
                case BTNodeStatus.Running:
                    return _result;
                case BTNodeStatus.Success:
                    continue;
            }
        }
        return BTNodeStatus.Success;
    }
}
