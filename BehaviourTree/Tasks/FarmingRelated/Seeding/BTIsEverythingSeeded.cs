public class BTIsEverythingSeeded : BTNode
{
    public BTIsEverythingSeeded(BTBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override BTNodeStatus Tick()
    {
        if (blackboard.NPC.AssignedFarm.IsEverythingSeeded() == true) { return BTNodeStatus.Success; }
        else { return BTNodeStatus.Failed; }
    }
}
