public class BTWaterCrops : BTNode
{
    private Farm farm;

    public BTWaterCrops(BTBlackboard _blackboard, Farm _farm)
    {
        blackboard = _blackboard;
        farm = _farm;
    }

    public override BTNodeStatus Tick()
    {
        FarmingSpot _spot = farm.ConsecutiveUnwateredFarmingSpot();

        if (_spot != null)
        {
            if (blackboard.NPC.ReachedTargetDestination(_spot.transform.position))
            {
                _spot.Water();
                blackboard.NPC.StartWaterAnimation();
            }           
            return BTNodeStatus.Running;
        }

        else { return BTNodeStatus.Success; }
    }
}
