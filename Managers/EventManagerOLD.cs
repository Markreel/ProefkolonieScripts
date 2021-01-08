using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class GameEventOLD
{
    public string Message;

    public string OptionAMessage = "Yes";
    public string OptionBMessage = "No";
    
    [Header("Option A:")]
    public float ColonistAdjustmentA;
    public float SubCommissionAdjustmentA;
    public UnityEvent OnOptionA;

    [Header("Option B:")]
    public float ColonistAdjustmentB;
    public float SubCommissionAdjustmentB;
    public UnityEvent OnOptionB;
}

public class EventManagerOLD : MonoBehaviour
{
    public List<GameEventOLD> MonthlyEvents = new List<GameEventOLD>();
    public bool IsEndOfMonth = false;
    private GameEventOLD currentMonthlyEvent;
    private int monthlyEventIndex = 0;

    public void InvokeMonthlyEvent()
    {
        IsEndOfMonth = false;
        currentMonthlyEvent = MonthlyEvents[monthlyEventIndex];
        monthlyEventIndex++;
        GameManager.Instance.MoneyGraphSystem.GenerateGraph();
        //GameManager.Instance.UIManager.ActivateMonthlyEvent(currentMonthlyEvent);
        GameManager.Instance.PauseGame();
    }

    public void OptionA()
    {
        GameManager.Instance.UIManager.DeactivateEventCanvasGroup();

        //GameManager.Instance.AdjustColonistContentmentLevel(currentMonthlyEvent.ColonistAdjustmentA);
        //GameManager.Instance.AdjustSubCommissionContentmentLevel(currentMonthlyEvent.SubCommissionAdjustmentA);

        currentMonthlyEvent.OnOptionA.Invoke();

        GameManager.Instance.UIManager.ActivateEndOfDayWindow();
    }

    public void OptionB()
    {
        GameManager.Instance.UIManager.DeactivateEventCanvasGroup();

        //GameManager.Instance.AdjustColonistContentmentLevel(currentMonthlyEvent.ColonistAdjustmentB);
        //GameManager.Instance.AdjustSubCommissionContentmentLevel(currentMonthlyEvent.SubCommissionAdjustmentB);

        currentMonthlyEvent.OnOptionB.Invoke();

        GameManager.Instance.UIManager.ActivateEndOfDayWindow();
    }
}
