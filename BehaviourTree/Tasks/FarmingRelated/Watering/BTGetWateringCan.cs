using UnityEngine;

public class BTGetWateringCan : BTNode
{
    public BTGetWateringCan(BTBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override BTNodeStatus Tick()
    {
        if (blackboard.NPC.ReachedTargetDestination(blackboard.NPC.AssignedHouse.AccessPoint.position))
        {
            blackboard.NPC.Item = blackboard.NPC.AssignedHouse.GetWateringCan();
            //Debug.Log("Picked up wateringcan: " + blackboard.NPC.Item);
            return BTNodeStatus.Success;
        }

        return BTNodeStatus.Running;
    }
}