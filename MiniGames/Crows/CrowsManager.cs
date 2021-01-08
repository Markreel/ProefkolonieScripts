using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowsManager : MonoBehaviour
{
    private List<Crows> crowsList = new List<Crows>();
    private List<Crows> crowsAfterTimeJumpList = new List<Crows>();

    private void Awake()
    {
        PopulateCrowsList();
    }

    private void PopulateCrowsList()
    {
        crowsList?.Clear();
        crowsAfterTimeJumpList?.Clear();

        foreach (var _crows in GetComponentsInChildren<Crows>(true))
        {
            if (!_crows.OnlyActiveAfterTimeJump) { crowsList?.Add(_crows); }
            else { crowsAfterTimeJumpList?.Add(_crows); }
        }
    }

    private void ActivateRandomAmountOfCrows(int _amount)
    {
        //Shuffle crowslist
        for (int i = 0; i < crowsList.Count; i++)
        {
            var _temp = crowsList[i];
            int _randomIndex = Random.Range(i, crowsList.Count);

            crowsList[i] = crowsList[_randomIndex];
            crowsList[_randomIndex] = _temp;
        }

        //Activate the given amount of crows
        for (int i = 0; i < crowsList.Count; i++)
        {
            crowsList[i].gameObject.SetActive(i < _amount);
        }
    }

    private bool CheckForEvent()
    {
        foreach (var _crows in crowsList)
        {
            if (_crows.gameObject.activeInHierarchy) { GameManager.Instance.EventManager.StartCrowsEvent(); return true; }
        }

        return false;
    }

    public bool OnNextDay()
    {
        bool _outcome = CheckForEvent();
        ActivateRandomAmountOfCrows(5);

        return _outcome;
    }

    public void OnTimeJumpAlteration()
    {
        foreach (var _crows in crowsAfterTimeJumpList)
        {
            crowsList?.Add(_crows);
        }

        crowsAfterTimeJumpList?.Clear();
    }

}
