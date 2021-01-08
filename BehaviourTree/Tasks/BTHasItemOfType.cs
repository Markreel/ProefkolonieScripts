public class BTHasItemOfType : BTNode
{
    private System.Type type;

    public BTHasItemOfType(BTBlackboard _blackboard, System.Type _type)
    {
        blackboard = _blackboard;
        type = _type;
    }

    public override BTNodeStatus Tick()
    {
        if(blackboard.NPC.Item != null && blackboard.NPC.Item.GetType() == type) { return BTNodeStatus.Success; }
        else { return BTNodeStatus.Failed; }
    }
}
