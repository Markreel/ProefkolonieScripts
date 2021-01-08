using UnityEngine;

public class WateringCan : MonoBehaviour, IItem, IFillable
{
    public FillType FillType { get; private set; }

    public void Fill(FillType _fillType)
    {
        FillType = _fillType;
    }

    public void Empty()
    {
        FillType = FillType.Empty;
    }
}

