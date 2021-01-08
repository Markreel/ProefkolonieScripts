using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpBar : PopUpElement
{
    [SerializeField] private Image BarImage;
    [SerializeField] private float lerpDuration = 1f;

    private Coroutine setBarValueRoutine;

    public void SetBarValue(float _value)
    {
        if(setBarValueRoutine != null) { StopCoroutine(setBarValueRoutine); }
        setBarValueRoutine = StartCoroutine(IESetBarValue(_value));
    }

    private IEnumerator IESetBarValue(float _valueB)
    {
        float _timeKey = 0;
        float _valueA = BarImage.fillAmount;
        AnimationCurve _curve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        while (_timeKey < 1)
        {
            _timeKey += Time.deltaTime / lerpDuration;
            float _evaluatedTimeKey = _curve.Evaluate(_timeKey);

            BarImage.fillAmount = Mathf.Lerp(_valueA, _valueB, _evaluatedTimeKey);
            yield return null;
        }

        yield return null;
    }
}
