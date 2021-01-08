using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CropsManager : MonoBehaviour
{
    public GameObject[] PotatoVisuals;
    public GameObject[] VegetableVisuals;
    public GameObject[] WheatVisuals;

    public List<Crop> Crops = new List<Crop>();

    private int amountOfPotatoes { get { return amountOfPotatoes; } set { amountOfPotatoes = (int)Mathf.Clamp(value, 0, Mathf.Infinity); } }
    private int amountOfVegetables { get { return amountOfVegetables; } set { amountOfVegetables = (int)Mathf.Clamp(value, 0, Mathf.Infinity); } }
    private int amountOfWheat { get { return amountOfWheat; } set { amountOfWheat = (int)Mathf.Clamp(value, 0, Mathf.Infinity); } }

    public void AddCropToList(Crop _crop)
    {
        if (!Crops.Contains(_crop)) { Crops.Add(_crop); }
    }

    public GameObject GetCropVisual(CropType _type, CropStage _stage)
    {
        switch (_type)
        {
            default:
            case CropType.Potato: return PotatoVisuals[(int)_stage];
            case CropType.Vegetable: return VegetableVisuals[(int)_stage];
            case CropType.Wheat: return WheatVisuals[(int)_stage];
        }
    }

    public void AddCropsToCollection(CropType _type, int _amount)
    {
        switch (_type)
        {
            case CropType.Potato:
                amountOfPotatoes += _amount;
                break;
            case CropType.Vegetable:
                amountOfVegetables += _amount;
                break;
            case CropType.Wheat:
                amountOfWheat += _amount;
                break;
        }
    }

    public void RemoveCropsFromCollection(CropType _type, int _amount)
    {
        switch (_type)
        {
            case CropType.Potato:
                amountOfPotatoes -= _amount;
                break;
            case CropType.Vegetable:
                amountOfVegetables -= _amount;
                break;
            case CropType.Wheat:
                amountOfWheat -= _amount;
                break;
        }
    }

    public void AdvanceAllCrops()
    {
        foreach (var _crop in Crops)
        {
            if (_crop.FarmingSpot.IsSeeded) { _crop.AdvanceCropStage(); }
        }
    }
}
