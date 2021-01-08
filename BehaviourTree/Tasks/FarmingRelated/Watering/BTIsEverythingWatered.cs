public class BTIsEverythingWatered : BTNode
{
    public BTIsEverythingWatered(BTBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override BTNodeStatus Tick()
    {
        if(blackboard.NPC.AssignedFarm.IsEverythingWatered() == true) { return BTNodeStatus.Success; }
        else { return BTNodeStatus.Failed; }
    }
}