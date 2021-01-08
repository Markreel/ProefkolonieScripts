using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class MathM
{
    /// <summary>
    /// Returns the position out of list "_targetPosList" that is the closest relative to "_subjectedPos".
    /// </summary>
    /// <param name="_subjectedPos"></param>
    /// <param name="_targetPosList"></param>
    /// <returns></returns>
    public static Vector3 ClosestPos(Vector3 _subjectedPos, List<Vector3> _targetPosList)
    {
        Vector3 _closestPos = _targetPosList[0];
        float _closestDistance = Mathf.Infinity;

        foreach (var _targetPos in _targetPosList)
        {
            float _distance = Vector3.Distance(_subjectedPos, _targetPos);

            if (_distance < _closestDistance)
            {
                _closestPos = _targetPos;
                _closestDistance = _distance;
            }
        }

        return _closestPos;
    }

    /// <summary>
    /// Returns the position out of list "_targetPosList" that is the closest relative to "_subjectedPos". (y position is set to 0)
    /// </summary>
    /// <param name="_subjectedPos"></param>
    /// <param name="_targetPosList"></param>
    /// <returns></returns>s
    public static Vector3 ClosestPosFlat(Vector3 _subjectedPos, List<Vector3> _targetPosList)
    {
        _subjectedPos.y = 0;

        for (int i = 0; i < _targetPosList.Count; i++)
        {
            _targetPosList[i] = new Vector3(_targetPosList[i].x, 0, _targetPosList[i].z);
        }

        return ClosestPos(_subjectedPos, _targetPosList);
    }

    public static Vector3 ClampVector3(Vector3 _value, Vector3 _min, Vector3 _max)
    {
        float _x = Mathf.Clamp(_value.x, _min.x, _max.x);
        float _y = Mathf.Clamp(_value.y, _min.y, _max.y);
        float _z = Mathf.Clamp(_value.z, _min.z, _max.z);

        return new Vector3(_x, _y, _z);
    }

    public static int DivisionWithoutRemainders(int a, int b)
    {
        return (a - a % b) / b;
    }

    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }

}
