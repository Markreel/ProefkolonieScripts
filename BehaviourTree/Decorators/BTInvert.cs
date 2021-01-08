public class BTInvert : BTNode
{
    BTNode decoratedNode;

    public BTInvert(BTNode _decoratedNode)
    {
        decoratedNode = _decoratedNode;
    }

    public override BTNodeStatus Tick()
    {
        BTNodeStatus _status = decoratedNode.Tick();

        if (_status == BTNodeStatus.Failed) { return BTNodeStatus.Success; }
        else if (_status == BTNodeStatus.Success) { return BTNodeStatus.Failed; }
        else { return BTNodeStatus.Running; }
    }
}