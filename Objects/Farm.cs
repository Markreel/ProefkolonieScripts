using System.Collections.Generic;
using UnityEngine;

public class Farm : MonoBehaviour
{
    public List<FarmingSpot> FarmingSpots = new List<FarmingSpot>();

    private void Awake()
    {
        AccumulateFarmingSpots();
    }

    private void AccumulateFarmingSpots()
    {
        foreach (FarmingSpot _spot in GetComponentsInChildren<FarmingSpot>())
        {
            FarmingSpots.Add(_spot);
        }
    }

    public bool IsEverythingPlowed()
    {
        foreach (var _farmingSpot in FarmingSpots)
        {
            if (!_farmingSpot.IsPlowed) { return false; }
        }
        return true;
    }

    public bool IsEverythingSeeded()
    {
        foreach (var _farmingSpot in FarmingSpots)
        {
            if (!_farmingSpot.IsSeeded) { return false; }
        }
        return true;
    }

    public bool IsEverythingWatered()
    {
        foreach (var _farmingSpot in FarmingSpots)
        {
            if (!_farmingSpot.IsWatered) { return false; }
        }
        return true;
    }

    public bool IsEverythingFertilized()
    {
        foreach (var _farmingSpot in FarmingSpots)
        {
            if (!_farmingSpot.IsFertilized) { return false; }
        }
        return true;
    }

    public bool IsEverythingHarvested()
    {
        foreach (var _farmingSpot in FarmingSpots)
        {
            if (_farmingSpot.IsFullGrown) { return false; }
        }
        return true;
    }

    public FarmingSpot ConsecutiveUnseededFarmingSpot()
    {
        if (FarmingSpots.Count < 1) { return null; }

        List<FarmingSpot> _unseededSpots = new List<FarmingSpot>();
        foreach (var _spot in FarmingSpots)
        {
            if (!_spot.IsSeeded) { _unseededSpots.Add(_spot); }
        }

        if(_unseededSpots.Count > 0) { return _unseededSpots[0]; }
        else { return null; }
    }

    public FarmingSpot ConsecutiveUnwateredFarmingSpot()
    {
        if (FarmingSpots.Count < 1) { return null; }

        List<FarmingSpot> _unwateredSpots = new List<FarmingSpot>();
        foreach (var _spot in FarmingSpots)
        {
            if (!_spot.IsWatered) { _unwateredSpots.Add(_spot); }
        }

        if (_unwateredSpots.Count > 0) { return _unwateredSpots[0]; }
        else { return null; }
    }

    public FarmingSpot ConsecutiveUnharvestedFarmingSpot()
    {
        if (FarmingSpots.Count < 1) { return null; }

        List<FarmingSpot> _unharvestedSpots = new List<FarmingSpot>();
        foreach (var _spot in FarmingSpots)
        {
            if (_spot.IsFullGrown) { _unharvestedSpots.Add(_spot); }
        }

        if (_unharvestedSpots.Count > 0) { return _unharvestedSpots[0]; }
        else { return null; }
    }

    public FarmingSpot ClosestUnseededFarmingSpot(NPC _npc)
    {
        List<FarmingSpot> _unseededSpots = new List<FarmingSpot>();
        List<Vector3> _unseededPositions = new List<Vector3>();

        foreach (var _spot in FarmingSpots)
        {
            if (!_spot.IsSeeded) { _unseededSpots.Add(_spot); _unseededPositions.Add(_spot.transform.position); }
        }

        if(_unseededSpots.Count > 1)
        {
            Vector3 _target = _npc.ClosestDestinationByPath(_npc.transform.position, _unseededPositions);
            return _unseededSpots[_unseededPositions.IndexOf(_target)];
        }
        else
        {
            return null;
        }
    }
}
