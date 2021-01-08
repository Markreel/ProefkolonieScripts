using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogLine
{
    public Sprite Sprite;
    public string Name;
    [TextArea] public string Text;
    public bool IsFlipped;

    public List<DialogOption> Options = new List<DialogOption>();

    public UnityEvent OnBeforeLine;
    public UnityEvent OnFinishedLine;
}

[System.Serializable]
public class DialogOption
{
    [HideInInspector] public string Name = "Optie:";
    public string Text;
    public float ColonistAdjustment;
    public float SubcommissionAdjustment;
    public int MoneyAdjustment;
    public UnityEvent OnChooseOption;

    public void InvokeOption()
    {
        //DIT \/ MOET PAS NA DE EVENTS DIE GEVOLGEN ZIJN VAN DE OPTIES (EN DAN EEN ANIMATIE VAN WAT ER GEBEURT IS (icoontje van ding en dan omlaag of omhoog animeren))

        //Save adjustments
        GameManager.Instance.EventManager.SaveAdjustmentLevels(ColonistAdjustment, SubcommissionAdjustment, MoneyAdjustment);

        OnChooseOption.Invoke();
        //GameManager.Instance.AdjustMoneyAndContentmentLevels(SubcommissionAdjustment, ColonistAdjustment, MoneyAdjustment);
    }
}

[System.Serializable]
public class GameEvent
{
    public string Name = "NewEvent";

    public UnityEvent OnBeforeDialog;
    public UnityEvent OnFinishedDialog;

    public Date ActivationDate;
    public bool CloseDialogAfterLastLine = true;

    [Header("Lines: ")]
    public List<DialogLine> DialogLines = new List<DialogLine>();
}

[System.Serializable]
public class TimeJump
{
    public Date From;
    public Date To;
    public UnityEvent OnFinished;
}

public class EventManager : MonoBehaviour
{
    [Header("Colonist Escapes Event: ")]
    public GameEvent ColonistEscapesEvent;
    public GameEvent ColonistEscapesEventA;
    public GameEvent ColonistEscapesEventB;

    [Header("Subcommission Unhappy Event: ")]
    public GameEvent SubcommissionUnhappyEvent;

    [Header("Crows Event: ")]
    public GameEvent CrowsEvent;
    public GameEvent ShipmentEvent;

    [Space]

    public List<GameEvent> ImportantEvents = new List<GameEvent>();
    public bool IsEndOfMonth = false;
    private GameEvent currentEvent;

    [Header("Time Jumps: ")]
    public List<TimeJump> TimeJumps = new List<TimeJump>();
    [HideInInspector] public Date TimeJumpCheckDate;
    private TimeJump currentTimeJump;

    private float colonistAdjustment;
    private float subcommissionAdjustment;
    private float incomeAdjustment;

    private int currentEventDialogLineIndex = 0;

    public void StartEvent(int _index)
    {
        StartEvent(ImportantEvents[_index]);
    }

    public void StartEvent(GameEvent _event)
    {
        currentEventDialogLineIndex = 0;
        currentEvent = _event;
        GameManager.Instance.PauseGame();

        GameManager.Instance.UIManager.DisplayDialogLine(_event.DialogLines[0]);
        GameManager.Instance.UIManager.ActivateEventCanvasGroup();
    }

    public void InvokeNextEventLine()
    {
        currentEventDialogLineIndex++;

        if (currentEvent.CloseDialogAfterLastLine && currentEventDialogLineIndex > currentEvent.DialogLines.Count-1)
        {
            StopEvent();
            return;
        }

        GameManager.Instance.UIManager.DisplayDialogLine(currentEvent.DialogLines[currentEventDialogLineIndex]);
    }

    public void StopColonistEscapesEvent()
    {
        GameManager.Instance.UIManager.DeactivateEventCanvasGroup();
        currentEventDialogLineIndex = 0;
        currentEvent = null;

        GameManager.Instance.UIManager.ActivateEndOfDayWindow();
    }

    public void StopEvent()
    {
        //Display adjustments if there are any
        if (colonistAdjustment != 0 || subcommissionAdjustment != 0 || incomeAdjustment != 0)
        {
            DisplayAdjustmentLevels(StopEventPart2);
        }

        else { StopEventPart2(); }
    }

    private void StopEventPart2()
    {
        GameManager.Instance.UIManager.DeactivateEventCanvasGroup();
        currentEventDialogLineIndex = 0;
        currentEvent = null;

        GameManager.Instance.ResumeGame();
        GameManager.Instance.UIManager.Fade(true, 3f);
    }

    public void StartEventOfCurrentDate()
    {
        Date _currentDate = GameManager.Instance.TimeSystem.CurrentDate;
        //Debug.Log(_currentDate.Year.ToString()  + "|" + _currentDate.Month.ToString() + " | " + _currentDate.Day);

        foreach (var _event in ImportantEvents)
        {
            if(_event.ActivationDate.Year == _currentDate.Year && _event.ActivationDate.Month == _currentDate.Month && _event.ActivationDate.Day == _currentDate.Day)
            {
                StartEvent(_event);
                return;
            }
        }

        StopEvent();
    }

    public void StartColonistEscapesEvent()
    {
        StartEvent(ColonistEscapesEvent);
    }

    public void OnColonistEscapesEventOptionA()
    {
        StartEvent(ColonistEscapesEventA);
    }

    public void OnColonistEscapesEventOptionB()
    {
        StartEvent(ColonistEscapesEventB);
    }

    public void StartCrowsEvent()
    {
        StartEvent(CrowsEvent);
    }

    public void StopCrowsEvent()
    {
        if (colonistAdjustment != 0 || subcommissionAdjustment != 0 || incomeAdjustment != 0)
        {
            DisplayAdjustmentLevels(StopCrowsEventPart2);
        }
        else { StopCrowsEventPart2(); }
    }
    
    private void StopCrowsEventPart2()
    {
        if (GameManager.Instance.EventManager.CheckForTimeJump()) { Debug.Log("TIMEJUMP"); return; }
        StartEventOfCurrentDate();
    }

    public void StartShipmentEvent()
    {
        StartEvent(ShipmentEvent);
    }

    //Adjustments
    public void SaveAdjustmentLevels(float _colonist, float _subcommission, float _income)
    {
        colonistAdjustment += _colonist;
        subcommissionAdjustment += _subcommission;
        incomeAdjustment += _income;
    }

    private void DisplayAdjustmentLevels(UnityAction _onFinished)
    {
        GameManager.Instance.UIManager.AdjustmentOverview.DisplayAdjustments(
                colonistAdjustment, subcommissionAdjustment, incomeAdjustment, _onFinished);

        GameManager.Instance.AdjustMoneyAndContentmentLevels(subcommissionAdjustment, colonistAdjustment, incomeAdjustment);

        colonistAdjustment = subcommissionAdjustment =  incomeAdjustment = 0;
    }

    #region TimeJump

    public bool CheckForTimeJump()
    {
        foreach (var _timeJump in TimeJumps)
        {
            //Debug.Log($"Deze time jump is: dag:{ _timeJump.From.Day} maand:{ _timeJump.From.Month} jaar:{ _timeJump.From.Year}");
            //Debug.Log($"Deze time jump moet zijn: dag:{ TimeJumpCheckDate.Day} maand:{ TimeJumpCheckDate.Month} jaar:{ TimeJumpCheckDate.Year}");

            if (_timeJump.From.Year == TimeJumpCheckDate.Year && _timeJump.From.Month == TimeJumpCheckDate.Month && _timeJump.From.Day == TimeJumpCheckDate.Day)
            {
                ActivateTimeJump(_timeJump); 
                return true;
            }
        }
        return false;
    }

    public TimeJump CheckForTimeJumpAndReturnJump()
    {
        foreach (var _timeJump in TimeJumps)
        {
            //Debug.Log($"Deze time jump is: dag:{ _timeJump.From.Day} maand:{ _timeJump.From.Month} jaar:{ _timeJump.From.Year}");
            //Debug.Log($"Deze time jump moet zijn: dag:{ TimeJumpCheckDate.Day} maand:{ TimeJumpCheckDate.Month} jaar:{ TimeJumpCheckDate.Year}");

            if (_timeJump.From.Year == TimeJumpCheckDate.Year && _timeJump.From.Month == TimeJumpCheckDate.Month && _timeJump.From.Day == TimeJumpCheckDate.Day)
            {
                return _timeJump;
            }
        }
        return null;
    }

    private void ActivateTimeJump(TimeJump _timeJump)
    {
        currentTimeJump = _timeJump;

        GameManager.Instance.PauseGame();

        _timeJump.OnFinished.AddListener(DeactivateTimeJump);
        GameManager.Instance.UIManager.ActivateTimeJumpWindow(_timeJump.From, _timeJump.To, _timeJump.OnFinished.Invoke);

    }

    private void DeactivateTimeJump()
    {
        GameManager.Instance.TimeSystem.SetDate(currentTimeJump.To);
        GameManager.Instance.UIManager.DeactivateTimeJumpWindow();
        GameManager.Instance.ResumeGame();
        StartEventOfCurrentDate();
    }
    
    #endregion
}
