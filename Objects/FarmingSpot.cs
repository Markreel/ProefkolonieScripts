using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmingSpot : MonoBehaviour
{
    public bool IsPlowed { get; private set; }
    public bool IsSeeded { get; private set; }
    public bool IsWatered { get; private set; }
    public bool IsFertilized { get; private set; } 
    public bool IsFullGrown { get; set; }

    private Crop crop;

    private void Awake()
    {
        crop = GetComponent<Crop>();
    }

    public void Plow()
    {
        IsPlowed = true;
    }

    public void Seed()
    {
        IsSeeded = true;
    }

    public void Water()
    {
        IsWatered = true;
    }

    public void Fertilize()
    {
        IsFertilized = true;
    }

    public void Harvest(NPC _npc)
    {
        IsFullGrown = false;
        crop.HarvestCrop(_npc, (_npc.AssignedFarm == _npc.AssignedHouse.CorrespondingFarm));
    }
}
