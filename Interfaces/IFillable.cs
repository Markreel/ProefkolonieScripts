using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FillType { Empty, Water, Milk }

public interface IFillable
{
    void Fill(FillType _fillType);
    void Empty();

    FillType FillType { get; }
}
