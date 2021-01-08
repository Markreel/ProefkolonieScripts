using UnityEngine;

public class BTCurrentItemIsFilledWithType : BTNode
{
    private FillType fillType;

    public BTCurrentItemIsFilledWithType(BTBlackboard _blackboard, FillType _fillType)
    {
        blackboard = _blackboard;
        fillType = _fillType;
    }

    public override BTNodeStatus Tick()
    {
        //Debug.Log("Item is: " + blackboard.NPC.Item);

        if (blackboard.NPC.Item != null && ((IFillable)blackboard.NPC.Item) != null)
        {
            //Debug.Log("We got a fillable item!");
            if (((IFillable)blackboard.NPC.Item).FillType == fillType) { return BTNodeStatus.Success; }
        }

        return BTNodeStatus.Failed;
    }
}

//using UnityEngine;

//public class BTFillableIsFilledWithType : BTNode
//{
//    private IFillable fillable;
//    private FillType fillType;

//    public BTFillableIsFilledWithType(BTBlackboard _blackboard, IFillable _fillable, FillType _fillType)
//    {
//        Debug.Log("Fillable was: " + _fillable);

//        blackboard = _blackboard;
//        fillable = _fillable;
//        fillType = _fillType;
//    }

//    public override BTNodeStatus Tick()
//    {
//        Debug.Log("Fillable is: " + fillable);

//        if (fillable != null && fillable.FillType == fillType) { return BTNodeStatus.Success; }
//        else { return BTNodeStatus.Failed; }
//    }
//}

