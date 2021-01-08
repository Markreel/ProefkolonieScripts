using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [Header("Fading: ")]
    [SerializeField] Image fadeImage;
    [SerializeField] const float fadeDuration = 1f;

    [Header("Ecyclopedia: ")]
    public Encyclopedia Encyclopedia;
    [SerializeField] EncyclopediaItem eventEncyclopediaItem;

    [Header("EndOfDayWindow: ")]
    [SerializeField] CanvasGroup endOfDayCanvasGroup;

    [Header("General Information (Top-Right): ")]
    [SerializeField] Image subcommissionLevelBar;
    private float previousSubCommissionLevel;
    private float currentSubCommissionLevel;
    [SerializeField] Image colonistsLevelBar;
    private float previousColonistsLevel;
    private float currentColonistsLevel;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI dateText;
    [SerializeField] Image timeOfDayImage;
    [SerializeField] Sprite[] timeOfDaySprites;
    [SerializeField] Image incomeImage;
    [SerializeField] Color[] incomeColors;

    [Header("Profession Selection: ")]
    [SerializeField] FoldOutElement ProfessionSelectionFoldOutElement;
    [SerializeField] CanvasGroup ProfessionSelectionCanvasGroup;

    [Header("-------------------------------------------------------------------------------------------------------------------------------------------------", order = 0)]

    [Header("Money Graph: ", order = 1)]
    [SerializeField] bool showMoneyGraph = true;
    [SerializeField] CanvasGroup moneyGraphCanvasGroup;

    [Header("Pause Menu: ")]
    [SerializeField] GameObject pauseMenu;

    //[Header("Monthly Event")]
    //[SerializeField] CanvasGroup monthlyEventCanvasGroup;
    //[SerializeField] TextMeshProUGUI monthlyEventMessageText;
    //[SerializeField] TextMeshProUGUI monthlyEventOptionAText;
    //[SerializeField] TextMeshProUGUI monthlyEventOptionBText;

    [Header("Events: ")]
    [SerializeField] CanvasGroup eventCanvasGroup;
    [SerializeField] TextMeshProUGUI eventNameText;
    [SerializeField] TextMeshProUGUI eventMessageText;
    [SerializeField] Image eventCharacterImage;
    [SerializeField] List<GameObject> currentEventOptions;
    [SerializeField] Transform eventOptionsParent;
    [SerializeField] GameObject eventOptionPrefab;

    [Header("Adjustment Overview: ")]
    public AdjustmentOverview AdjustmentOverview;

    [Header("Time Jump: ")]
    [SerializeField] float timeJumpStartDelay;
    [SerializeField] float timeJumpDuration;
    [SerializeField] float timeJumpEndDelay;
    [SerializeField] AnimationCurve timeJumpCurve;
    [SerializeField] CanvasGroup timeJumpCanvasGroup;
    [SerializeField] TextMeshProUGUI timeJumpYear;
    [SerializeField] TextMeshProUGUI timeJumpMonth;
    private Coroutine timeJumpRoutine;
    private Date dateA;
    private Date dateB;

    private Coroutine activateTimeJumpRoutine;
    private Coroutine activateEventRoutine;

    private void Start()
    {
        DeactivateProfessionSelectionMenu();
        DeactivateEventCanvasGroup();
        DeactivateEndOfDayWindow();
        Fade(true);
    }

    private void LerpFadeImageToBlack(float _t)
    {
        Color _col = Color.Lerp(Color.clear, Color.black, _t);
        fadeImage.color = _col;
    }

    private void LerpFadeImageToClear(float _t)
    {
        Color _col = Color.Lerp(Color.black, Color.clear, _t);
        fadeImage.color = _col;
    }

    public void SetActiveCanvasGroup(CanvasGroup _canvasGroup, bool _value)
    {
        _canvasGroup.alpha = _value ? 1 : 0;
        _canvasGroup.interactable = _value;
        _canvasGroup.blocksRaycasts = _value;
    }

    public void SetActiveCanvasGroup(CanvasGroup _canvasGroup, bool _value, Coroutine _coroutine)
    {
        if (_coroutine != null) { StopCoroutine(_coroutine); }
        _coroutine = StartCoroutine(IESetActiveCanvasGroup(_canvasGroup, _value, _coroutine));
    }

    public IEnumerator IESetActiveCanvasGroup(CanvasGroup _canvasGroup, bool _value, Coroutine _coroutine)
    {
        float _alphaA = _canvasGroup.alpha;
        float _alphaB = _value ? 1 : 0;

        _canvasGroup.interactable = false;
        _canvasGroup.blocksRaycasts = true;

        float _timeKey = 0f;
        while (_timeKey < 1f)
        {
            _timeKey += Time.unscaledDeltaTime / 1f;
            _canvasGroup.alpha = Mathf.Lerp(_alphaA, _alphaB, _timeKey);

            yield return null;
        }

        _canvasGroup.interactable = _value;
        _canvasGroup.blocksRaycasts = _value;

        _coroutine = null;
        yield return null;
    }

    private void PopulateDialogOptions(DialogLine _dialogLine)
    {
        //Clean up old options in game
        foreach (Transform _child in eventOptionsParent.transform)
        {
            Destroy(_child.gameObject);
        }

        //Clean up old options in List
        currentEventOptions.Clear();

        //Create a single button to continue the dialog if there are no options
        if (_dialogLine.Options.Count == 0)
        {
            GameObject _obj = Instantiate(eventOptionPrefab, eventOptionsParent);
            _obj.GetComponent<Button>().onClick.AddListener(GameManager.Instance.EventManager.InvokeNextEventLine);

            _obj.GetComponentInChildren<TextMeshProUGUI>().text = "Doorgaan";
        }

        //Create all options
        else
        {
            foreach (var _option in _dialogLine.Options)
            {
                GameObject _obj = Instantiate(eventOptionPrefab, eventOptionsParent);
                _obj.GetComponent<Button>().onClick.AddListener(_option.InvokeOption);

                _obj.GetComponentInChildren<TextMeshProUGUI>().text = _option.Text;
            }
        }
    }

    public void Fade(bool _in = false, float _duration = fadeDuration, UnityAction _onFinished = null)
    {
        if (_in) { GameManager.Instance.CoroutineHandler.LerpOverTime("Fade", _duration, LerpFadeImageToClear, null, _onFinished, null, true); }
        else { GameManager.Instance.CoroutineHandler.LerpOverTime("Fade", _duration, LerpFadeImageToBlack, null, _onFinished, null, true); }
    }

    public void DisplayDialogLine(DialogLine _dialogLine)
    {
        //if (showMoneyGraph)
        //{
        //    SetActiveCanvasGroup(moneyGraphCanvasGroup, true);
        //}
        //else
        //{
        //    SetActiveCanvasGroup(eventCanvasGroup, true);
        //}

        eventNameText.text = $"{_dialogLine.Name}:";
        eventMessageText.text = _dialogLine.Text;
        eventCharacterImage.sprite = _dialogLine.Sprite;
        eventCharacterImage.gameObject.transform.localScale = _dialogLine.IsFlipped ? new Vector3(-1, 1, 1) : Vector3.one;

        eventEncyclopediaItem.SetValuesAndCheckIfShouldBeEnabled();

        PopulateDialogOptions(_dialogLine);
    }

    public void ActivateEventCanvasGroup()
    {
        if (eventCanvasGroup.interactable) { return; }
        SetActiveCanvasGroup(eventCanvasGroup, true, activateEventRoutine);
    }

    public void DeactivateEventCanvasGroup()
    {
        SetActiveCanvasGroup(eventCanvasGroup, false, activateEventRoutine);
    }

    public void CloseMoneyGraphWindow()
    {
        SetActiveCanvasGroup(moneyGraphCanvasGroup, false);
        //SetActiveCanvasGroup(eventCanvasGroup, true);
    }

    private void SetSubCommissionLevelBarFillAmount(float _t)
    {
        subcommissionLevelBar.fillAmount = Mathf.Lerp(previousSubCommissionLevel, currentSubCommissionLevel, _t);
    }

    private void SetColonistLevelBarFillAmount(float _t)
    {
        colonistsLevelBar.fillAmount = Mathf.Lerp(previousColonistsLevel, currentColonistsLevel, _t); ;
    }

    public void UpdateContentmentLevels(float _subCommissionLevel, float _colonistsLevel, bool _dontLerp = false)
    {
        previousSubCommissionLevel = currentSubCommissionLevel;
        currentSubCommissionLevel = _subCommissionLevel;

        previousColonistsLevel = currentColonistsLevel;
        currentColonistsLevel = _colonistsLevel;

        if (_dontLerp)
        {
            subcommissionLevelBar.fillAmount = currentSubCommissionLevel;
            colonistsLevelBar.fillAmount = currentColonistsLevel;
            return;
        }

        GameManager.Instance.CoroutineHandler.LerpOverTime("SubCommissionLevelUILerp", 1f, SetSubCommissionLevelBarFillAmount);
        GameManager.Instance.CoroutineHandler.LerpOverTime("ColonistsLevelUILerp", 1f, SetColonistLevelBarFillAmount);
    }

    public void UpdateTimeAndDate(float _hours, float _minutes, Date _date)
    {
        timeText.text = string.Format("{0:00} : {1:00}", _hours, _minutes);
        dateText.text = $"{(Months)_date.Month} {_date.Year}";

        if (_hours < 4 || _hours > 22) { timeOfDayImage.sprite = timeOfDaySprites[3]; }
        else if (_hours < 10) { timeOfDayImage.sprite = timeOfDaySprites[0]; }
        else if (_hours < 16) { timeOfDayImage.sprite = timeOfDaySprites[1]; }
        else if (_hours < 22) { timeOfDayImage.sprite = timeOfDaySprites[2]; }
    }

    public void UpdateIncome(float _previousMoney, float _currentMoney)
    {
        bool _profit;
        _profit = _previousMoney > _currentMoney ? false : true;

        incomeImage.transform.localScale = _profit ? Vector3.one : new Vector3(1, -1, 1);
        incomeImage.color = _profit ? incomeColors[0] : incomeColors[1];
    }

    public void ActivateProfessionSelectionMenu(Profession _profession)
    {
        SetActiveCanvasGroup(ProfessionSelectionCanvasGroup, true);

        if (_profession == Profession.None) { ProfessionSelectionFoldOutElement.FoldOut(); }
        else { ProfessionSelectionFoldOutElement.FoldIn(true); }
    }

    public void DeactivateProfessionSelectionMenu()
    {
        SetActiveCanvasGroup(ProfessionSelectionCanvasGroup, false);
    }

    public void ActivateEndOfDayWindow()
    {
        //if (GameManager.Instance.EventManager.IsEndOfMonth) { GameManager.Instance.EventManager.StartEvent(); }
        SetActiveCanvasGroup(endOfDayCanvasGroup, true);  //else { <--- }
    }

    public void DeactivateEndOfDayWindow()
    {
        SetActiveCanvasGroup(endOfDayCanvasGroup, false);
    }

    public void ActivatePauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void DeactivatePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    #region TimeJump
    public void ActivateTimeJumpWindow(Date _dateA, Date _dateB, UnityAction _onFinished)
    {
        timeJumpYear.text = _dateA.Year.ToString();
        timeJumpMonth.text = ((Months)_dateA.Month).ToString();

        dateA = _dateA;
        dateB = _dateB;

        //_onFinished += DeactivateTimeJumpWindow;
        SetActiveCanvasGroup(timeJumpCanvasGroup, true, activateTimeJumpRoutine);

        if (timeJumpRoutine != null) { StopCoroutine(timeJumpRoutine); }
        timeJumpRoutine = StartCoroutine(IELerpTimeJump(timeJumpStartDelay, timeJumpDuration, timeJumpEndDelay, _onFinished));
    }

    public void DeactivateTimeJumpWindow()
    {
        SetActiveCanvasGroup(timeJumpCanvasGroup, false, activateTimeJumpRoutine);
    }

    public IEnumerator IELerpTimeJump(float _startDelay, float _duration, float _endDelay, UnityAction _onFinished)
    {

        int _monthDifference = 12 - dateA.Month + dateB.Month;
        int _additionalYears = Mathf.Abs(dateB.Year - dateA.Year) - 1;

        int _totalMonths = 12 * _additionalYears + _monthDifference;

        int _monthA = dateA.Month;
        int _monthB = dateA.Month + _totalMonths-1;

        //Wait before starting
        yield return new WaitForSecondsRealtime(_startDelay);

        float _timeKey = 0;
        while (_timeKey < 1f)
        {
            _timeKey += Time.unscaledDeltaTime / _duration;

            float _t = timeJumpCurve.Evaluate(_timeKey);

            int _lerpedMonth = Mathf.FloorToInt(Mathf.Lerp(_monthA, _monthB, _t));
            int _lerpedYear = dateA.Year + MathM.DivisionWithoutRemainders(_lerpedMonth, 12);

            timeJumpMonth.text = ((Months)_lerpedMonth - (12 * MathM.DivisionWithoutRemainders(_lerpedMonth, 12)) + 1).ToString();
            timeJumpYear.text = _lerpedYear.ToString();

            yield return null;
        }

        //Wait before ending
        yield return new WaitForSecondsRealtime(_startDelay);

        _onFinished.Invoke();
        timeJumpRoutine = null;

        yield return null;
    }


    #endregion
}
