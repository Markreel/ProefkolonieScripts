using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class House : MonoBehaviour, IFocusable
{
    public Transform AccessPoint;
    public Farm CorrespondingFarm;
    public Farm AdjecentCommunityFarm;

    [SerializeField] private List<NPC> residents;
    [Header("Hunger: ")]
    [SerializeField] private PopUpElement starvationIcon;
    [SerializeField] private PopUpBar hungerBar;
    [SerializeField] private float hungerValue = 1;
    [SerializeField] private float starvingThreshold = 0.5f;
    [SerializeField] private float hungerDropPerDay = 0.2f;
    [Space]
    [SerializeField] Seeds seeds;
    [SerializeField] WateringCan wateringCan;

    [Header("Focus Settings: ")]
    [SerializeField] private float zoomAmountOnFocus = 1;
    public float ZoomAmountOnFocus { get { return zoomAmountOnFocus; } set { zoomAmountOnFocus = value; } }

    [HideInInspector] public bool HasNPC = false;

    public Seeds GetSeeds()
    {
        return seeds;
    }

    public WateringCan GetWateringCan()
    {
        return wateringCan;
    }

    public void OnFocus()
    {
        hungerBar.SetActive(true);
        starvationIcon.SetActive(false);
    }

    public void OnUnfocus()
    {
        hungerBar.SetActive(false);
        CheckForStarvation();
    }

    public void AddResident(NPC _npc)
    {
        residents.Add(_npc);
    }

    public void RemoveResident(NPC _npc)
    {
        if (residents.Contains(_npc)) { residents.Remove(_npc); }
    }

    public void ResetHunger()
    {
        hungerValue = 1;
        UpdateHungerUI();
        CheckForStarvation();
        GameManager.Instance.AdjustMoneyAndContentmentLevels(0, 0.01f, 0);
    }

    public void DecreaseHunger()
    {
        hungerValue -= hungerDropPerDay;
        hungerValue = hungerValue < 0 ? 0 : hungerValue;

        UpdateHungerUI();
        if (CheckForStarvation()) { GameManager.Instance.AdjustMoneyAndContentmentLevels(0, -0.05f, 0); };
    }

    private void UpdateHungerUI()
    {
        hungerBar.SetBarValue(hungerValue);
    }

    private bool CheckForStarvation()
    {
        bool _result = hungerValue <= starvingThreshold;

        if (_result) { starvationIcon.SetActive(true); }
        else { starvationIcon.SetActive(false); }

        return _result;
    }
}