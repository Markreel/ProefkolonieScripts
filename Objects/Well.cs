using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Well : MonoBehaviour
{
    public FillType FillType;

    public void FillIFillable(IFillable _fillable)
    {
        _fillable.Fill(FillType);
    }
}
