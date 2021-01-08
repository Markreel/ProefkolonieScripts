using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructuresManager : MonoBehaviour
{
    [SerializeField] Transform housesParent;

    public Well Well;
    public List<House> Houses = new List<House>();

    private void Awake()
    {
        FillHouseList();
    }

    private void FillHouseList()
    {
        foreach (House _house in housesParent.GetComponentsInChildren<House>(true))
        {
            Houses.Add(_house);
        }
    }

    private void DecreaseHungerForEachHouse()
    {
        foreach (var _house in Houses)
        {
            if (!_house.isActiveAndEnabled) { continue; }
            _house.DecreaseHunger();
        }
    }

    public void OnNextDay()
    {
        DecreaseHungerForEachHouse();
    }
}
