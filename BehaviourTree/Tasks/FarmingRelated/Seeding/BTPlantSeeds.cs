public class BTPlantSeeds : BTNode
{
    private Farm farm;

    public BTPlantSeeds(BTBlackboard _blackboard)
    {
        blackboard = _blackboard;
    }

    public override BTNodeStatus Tick()
    {
        farm = blackboard.NPC.AssignedFarm;
        FarmingSpot _spot = farm.ConsecutiveUnseededFarmingSpot();

        if (_spot != null)
        {
            if (blackboard.NPC.ReachedTargetDestination(_spot.transform.position))
            {
                _spot.Seed();
                blackboard.NPC.StartSeedAnimation();
            }
            return BTNodeStatus.Running;
        }

        else { return BTNodeStatus.Success; }
    }
}
