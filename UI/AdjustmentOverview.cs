using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class AdjustmentOverview : MonoBehaviour
{
    [SerializeField] private Image ColonistAdjustmentArrow;
    [SerializeField] private Image SubCommissionAdjustmentArrow;
    [SerializeField] private Image IncomeAdjustmentArrow;
    [Space]
    [SerializeField] private Vector3 topPosition;
    [SerializeField] private Vector3 bottomPosition;
    [Space]
    [SerializeField] private float startDelay;
    [SerializeField] private float inBetweenDelay;
    [SerializeField] private float lerpDuration;
    [SerializeField] private float finishDelay;
    [Space]
    [SerializeField] private UnityEvent onFinished;

    private CanvasGroup canvasGroup;
    private Coroutine activationRoutine;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void LerpArrow(Image _arrow, bool _up, float _t)
    {
        Color _col = _arrow.color;
        _col.a = Mathf.Lerp(1f, 0f, _t);

        _arrow.color = _col;
        _arrow.GetComponent<RectTransform>().localPosition = Vector3.Lerp(Vector3.zero, _up ? topPosition : bottomPosition, _t);
    }

    private IEnumerator IEDisplayAdjustments(float _colonists, float _subcommissions, float _income, UnityAction _onFinished)
    {
        //Fade in self
        yield return GameManager.Instance.UIManager.IESetActiveCanvasGroup(canvasGroup, true, activationRoutine);

        //Wait out start delay
        yield return new WaitForSecondsRealtime(startDelay);

        //Skip if there is no change
        if (_colonists != 0)
        {
            //Lerp colonist arrow
            yield return IELerpArrow(ColonistAdjustmentArrow, _colonists > 0);
            yield return new WaitForSecondsRealtime(inBetweenDelay); //Wait out inbetween delay
        }

        //Skip if there is no change
        if (_subcommissions != 0)
        {
            //Lerp subcommission arrow
            yield return IELerpArrow(SubCommissionAdjustmentArrow, _subcommissions > 0);
            yield return new WaitForSecondsRealtime(inBetweenDelay); //Wait out inbetween delay
        }

        //Skip if there is no change
        if (_income != 0)
        {
            //Lerp income arrow
            yield return IELerpArrow(IncomeAdjustmentArrow, _income > 0);
        }

        //Wait out finish delay
        yield return new WaitForSecondsRealtime(finishDelay);

        //Fade out self
        yield return GameManager.Instance.UIManager.IESetActiveCanvasGroup(canvasGroup, false, activationRoutine);

        //Activate OnFinished event
        _onFinished.Invoke();
        yield return null;
    }

    private IEnumerator IELerpArrow(Image _arrow, bool _up)
    {
        _arrow.color = _up ? Color.green : Color.red;
        _arrow.GetComponent<RectTransform>().localScale = _up ? Vector3.one : new Vector3(1, -1, 1);

        float _timeKey = 0f;
        while (_timeKey < 1f)
        {
            _timeKey += Time.unscaledDeltaTime / lerpDuration;
            float _evaluatedTimeKey = AnimationCurve.EaseInOut(0, 0, 1, 1).Evaluate(_timeKey);

            LerpArrow(_arrow, _up, _evaluatedTimeKey);
            yield return null;
        }

        yield return null;
    }

    private void ResetArrows()
    {
        ColonistAdjustmentArrow.color = Color.clear;
        SubCommissionAdjustmentArrow.color = Color.clear;
        IncomeAdjustmentArrow.color = Color.clear;
    }

    public void DisplayAdjustments(float _colonists, float _subcommissions, float _income, UnityAction _onFinished)
    {
        _onFinished += onFinished.Invoke;
        ResetArrows();

        StopAllCoroutines();
        StartCoroutine(IEDisplayAdjustments(_colonists, _subcommissions, _income, _onFinished));
    }
}
