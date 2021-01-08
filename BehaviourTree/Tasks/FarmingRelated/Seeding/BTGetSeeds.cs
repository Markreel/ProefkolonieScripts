using UnityEngine;

public class BTGetSeeds : BTNode
{
    public BTGetSeeds(BTBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override BTNodeStatus Tick()
    {
        if(blackboard.NPC.ReachedTargetDestination(blackboard.NPC.AssignedHouse.AccessPoint.position))
        {
            blackboard.NPC.Item = blackboard.NPC.AssignedHouse.GetSeeds(); //geef zaad type mee
            //Debug.Log("Picked up seeds");
            return BTNodeStatus.Success;
        }

        return BTNodeStatus.Running; 
    }
}