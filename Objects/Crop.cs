using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CropType { Potato, Vegetable, Wheat }
public enum CropStage { Seed, Sprout, Matured, Withered }

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Crop : MonoBehaviour
{
    [HideInInspector] public FarmingSpot FarmingSpot;

    [SerializeField] int amountPerHarvest;
    [SerializeField] CropType type;

    private CropStage stage = CropStage.Seed;

    private MeshFilter meshFilter;
    private Renderer rend;

    private void Awake()
    {
        FarmingSpot = GetComponent<FarmingSpot>();
        meshFilter = GetComponent<MeshFilter>();
        rend = GetComponent<Renderer>();
    }

    private void Start()
    {
        GameManager.Instance.CropsManager.AddCropToList(this);
    }

    private void UpdateCropVisuals()
    {
        GameObject _visual = GameManager.Instance.CropsManager.GetCropVisual(type, stage);

        meshFilter.mesh = _visual.GetComponent<MeshFilter>().sharedMesh;
        rend.materials = _visual.GetComponent<Renderer>().sharedMaterials;
    }

    public void HarvestCrop(NPC _npc, bool _ownUse = false)
    {
        stage = CropStage.Seed;
        UpdateCropVisuals();

        if (_ownUse)
        {
            _npc.AssignedHouse.ResetHunger();
        }

        else { GameManager.Instance.AdjustMoneyAndContentmentLevels(2, 0, 2); }
        
    }

    public void AdvanceCropStage()
    {
        if ((int)stage < 2) { stage++; }
        if(stage == CropStage.Matured) { FarmingSpot.IsFullGrown = true; }

        UpdateCropVisuals();
    }
}
