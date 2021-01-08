public class BTHarvestCrops : BTNode
{
    private Farm farm;

    public BTHarvestCrops(BTBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override BTNodeStatus Tick()
    {
        farm = blackboard.NPC.AssignedFarm;
        FarmingSpot _spot = farm.ConsecutiveUnharvestedFarmingSpot();

        if (_spot != null)
        {
            if (blackboard.NPC.ReachedTargetDestination(_spot.transform.position))
            {
                _spot.Harvest(blackboard.NPC);
                blackboard.NPC.StartSeedAnimation();
            }
            return BTNodeStatus.Running;
        }

        else { return BTNodeStatus.Success; }
    }
}
