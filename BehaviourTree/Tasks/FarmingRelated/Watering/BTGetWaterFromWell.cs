using UnityEngine;

public class BTGetWaterFromWell : BTNode
{
    public BTGetWaterFromWell(BTBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override BTNodeStatus Tick()
    {
        if (blackboard.NPC.Item != null && ((IFillable)blackboard.NPC.Item) != null)
        {
            if (blackboard.NPC.ReachedTargetDestination(GameManager.Instance.StructuresManager.Well.transform.position, 3))
            {
                ((IFillable)blackboard.NPC.Item).Fill(GameManager.Instance.StructuresManager.Well.FillType);
                //Debug.Log("Filled " + ((IFillable)blackboard.NPC.Item) + " with " + GameManager.Instance.StructuresManager.Well.FillType);
                return BTNodeStatus.Success;
            }
            else
            {
                return BTNodeStatus.Running;
            }
        }

        else { return BTNodeStatus.Failed; }
    }
}
