public class BTIsEverythingHarvested : BTNode
{
    public BTIsEverythingHarvested(BTBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override BTNodeStatus Tick()
    {
        if (blackboard.NPC.AssignedFarm.IsEverythingHarvested() == true) { return BTNodeStatus.Success; }
        else { return BTNodeStatus.Failed; }
    }
}
