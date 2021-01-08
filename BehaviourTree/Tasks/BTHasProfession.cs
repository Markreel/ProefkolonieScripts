public class BTHasProfession : BTNode
{
    private Profession profession;

    public BTHasProfession(BTBlackboard _blackboard, Profession _profession)
    {
        blackboard = _blackboard;
        profession = _profession;
    }

    public override BTNodeStatus Tick()
    {
        if(blackboard.NPC.Profession == profession) { return BTNodeStatus.Success; }
        else { return BTNodeStatus.Failed; }
    }
}
